using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst;
using ME.ECS;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using ME.ECS.Mathematics;
using ME.ECS.Essentials.Physics;

namespace ME.ECS {

    [JobProducerType(typeof(IJobParallelForDeferExtensions.JobParallelForDeferProducer<>))]
    public interface IJobParallelForDefer
    {
        /// <summary>
        /// Implement this method to perform work against a specific iteration index.
        /// </summary>
        /// <param name="index">The index of the Parallel for loop at which to perform work.</param>
        void Execute(int index);
    }

    public static class IJobParallelForDeferExtensions
    {
        internal struct JobParallelForDeferProducer<T> where T : struct, IJobParallelForDefer
        {
            internal static readonly SharedStatic<IntPtr> jobReflectionData = SharedStatic<IntPtr>.GetOrCreate<JobParallelForDeferProducer<T>>();

            [Preserve]
            internal static void Initialize()
            {
                if (jobReflectionData.Data == IntPtr.Zero)
                    jobReflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(T), (ExecuteJobFunction)Execute);
            }

            public delegate void ExecuteJobFunction(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            public unsafe static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
            {
                while (true)
                {
                    if (!JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out int begin, out int end))
                        break;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf(ref jobData), begin, end - begin);
#endif
                    for (var i = begin; i < end; ++i)
                        jobData.Execute(i);
                }
            }
        }

        /// <summary>
        /// This method is only to be called by automatically generated setup code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void EarlyJobInit<T>()
            where T : struct, IJobParallelForDefer
        {
            JobParallelForDeferProducer<T>.Initialize();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void CheckReflectionDataCorrect(IntPtr reflectionData)
        {
            if (reflectionData == IntPtr.Zero)
                throw new InvalidOperationException("Reflection data was not set up by a call to Initialize()");
        }

        /// <summary>
        /// Schedule the job for execution on worker threads.
        /// list.Length is used as the iteration count.
        /// Note that it is required to embed the list on the job struct as well.
        /// </summary>
        /// <param name="jobData">The job and data to schedule.</param>
        /// <param name="list">list.Length is used as the iteration count.</param>
        /// <param name="innerloopBatchCount">Granularity in which workstealing is performed. A value of 32, means the job queue will steal 32 iterations and then perform them in an efficient inner loop.</param>
        /// <param name="dependsOn">Dependencies are used to ensure that a job executes on workerthreads after the dependency has completed execution. Making sure that two jobs reading or writing to same data do not run in parallel.</param>
        /// <returns>JobHandle The handle identifying the scheduled job. Can be used as a dependency for a later job or ensure completion on the main thread.</returns>
        public static unsafe JobHandle Schedule<T, U>(this T jobData, NativeList<U> list, int innerloopBatchCount,
            JobHandle dependsOn = new JobHandle())
            where T : struct, IJobParallelForDefer
            where U : unmanaged
        {
            void* atomicSafetyHandlePtr = null;
            // Calculate the deferred atomic safety handle before constructing JobScheduleParameters so
            // DOTS Runtime can validate the deferred list statically similar to the reflection based
            // validation in Big Unity.
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safety = NativeListUnsafeUtility.GetAtomicSafetyHandle(ref list);
            atomicSafetyHandlePtr = UnsafeUtility.AddressOf(ref safety);
#endif
            return ScheduleInternal(ref jobData, innerloopBatchCount,
                NativeListUnsafeUtility.GetInternalListDataPtrUnchecked(ref list),
                atomicSafetyHandlePtr, dependsOn);
        }

        /// <summary>
        /// Schedule the job for execution on worker threads.
        /// list.Length is used as the iteration count.
        /// Note that it is required to embed the list on the job struct as well.
        /// </summary>
        /// <param name="jobData">The job and data to schedule. In this variant, the jobData is
        /// passed by reference, which may be necessary for unusually large job structs.</param>
        /// <param name="list">list.Length is used as the iteration count.</param>
        /// <param name="innerloopBatchCount">Granularity in which workstealing is performed. A value of 32, means the job queue will steal 32 iterations and then perform them in an efficient inner loop.</param>
        /// <param name="dependsOn">Dependencies are used to ensure that a job executes on workerthreads after the dependency has completed execution. Making sure that two jobs reading or writing to same data do not run in parallel.</param>
        /// <returns>JobHandle The handle identifying the scheduled job. Can be used as a dependency for a later job or ensure completion on the main thread.</returns>
        public static unsafe JobHandle ScheduleByRef<T, U>(this ref T jobData, NativeList<U> list, int innerloopBatchCount,
            JobHandle dependsOn = new JobHandle())
            where T : struct, IJobParallelForDefer
            where U : unmanaged
        {
            void* atomicSafetyHandlePtr = null;
            // Calculate the deferred atomic safety handle before constructing JobScheduleParameters so
            // DOTS Runtime can validate the deferred list statically similar to the reflection based
            // validation in Big Unity.
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safety = NativeListUnsafeUtility.GetAtomicSafetyHandle(ref list);
            atomicSafetyHandlePtr = UnsafeUtility.AddressOf(ref safety);
#endif
            return ScheduleInternal(ref jobData, innerloopBatchCount,
                NativeListUnsafeUtility.GetInternalListDataPtrUnchecked(ref list),
                atomicSafetyHandlePtr, dependsOn);
        }

        /// <summary>
        /// Schedule the job for execution on worker threads.
        /// forEachCount is a pointer to the number of iterations, when dependsOn has completed.
        /// This API is unsafe, it is recommended to use the NativeList based Schedule method instead.
        /// </summary>
        /// <param name="jobData">The job and data to schedule.</param>
        /// <param name="forEachCount">*forEachCount is used as the iteration count.</param>
        /// <param name="innerloopBatchCount">Granularity in which workstealing is performed. A value of 32, means the job queue will steal 32 iterations and then perform them in an efficient inner loop.</param>
        /// <param name="dependsOn">Dependencies are used to ensure that a job executes on workerthreads after the dependency has completed execution. Making sure that two jobs reading or writing to same data do not run in parallel.</param>
        /// <returns>JobHandle The handle identifying the scheduled job. Can be used as a dependency for a later job or ensure completion on the main thread.</returns>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static unsafe JobHandle Schedule<T>(this T jobData, int* forEachCount, int innerloopBatchCount,
            JobHandle dependsOn = new JobHandle())
            where T : struct, IJobParallelForDefer
        {
            var forEachListPtr = (byte*)forEachCount - sizeof(void*);
            return ScheduleInternal(ref jobData, innerloopBatchCount, forEachListPtr, null, dependsOn);
        }

        /// <summary>
        /// Schedule the job for execution on worker threads.
        /// forEachCount is a pointer to the number of iterations, when dependsOn has completed.
        /// This API is unsafe, it is recommended to use the NativeList based Schedule method instead.
        /// </summary>
        /// <param name="jobData">The job and data to schedule. In this variant, the jobData is
        /// passed by reference, which may be necessary for unusually large job structs.</param>
        /// <param name="forEachCount">*forEachCount is used as the iteration count.</param>
        /// <param name="innerloopBatchCount">Granularity in which workstealing is performed. A value of 32, means the job queue will steal 32 iterations and then perform them in an efficient inner loop.</param>
        /// <param name="dependsOn">Dependencies are used to ensure that a job executes on workerthreads after the dependency has completed execution. Making sure that two jobs reading or writing to same data do not run in parallel.</param>
        /// <returns>JobHandle The handle identifying the scheduled job. Can be used as a dependency for a later job or ensure completion on the main thread.</returns>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static unsafe JobHandle ScheduleByRef<T>(this ref T jobData, int* forEachCount, int innerloopBatchCount,
            JobHandle dependsOn = new JobHandle())
            where T : struct, IJobParallelForDefer
        {
            var forEachListPtr = (byte*)forEachCount - sizeof(void*);
            return ScheduleInternal(ref jobData, innerloopBatchCount, forEachListPtr, null, dependsOn);
        }

        private static unsafe JobHandle ScheduleInternal<T>(ref T jobData,
            int innerloopBatchCount,
            void* forEachListPtr,
            void *atomicSafetyHandlePtr,
            JobHandle dependsOn) where T : struct, IJobParallelForDefer
        {
            IJobParallelForDeferExtensions.EarlyJobInit<T>();
            var reflectionData = JobParallelForDeferProducer<T>.jobReflectionData.Data;
            CheckReflectionDataCorrect(reflectionData);
            var scheduleParams = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), reflectionData, dependsOn, ScheduleMode.Parallel);
            return JobsUtility.ScheduleParallelForDeferArraySize(ref scheduleParams, innerloopBatchCount, forEachListPtr, atomicSafetyHandlePtr);
        }
    }
}