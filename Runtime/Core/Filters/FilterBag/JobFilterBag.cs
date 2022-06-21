namespace ME.ECS {
    
    using Unity.Jobs;
    using Unity.Jobs.LowLevel.Unsafe;
    using Unity.Collections.LowLevel.Unsafe;

    [JobProducerType(typeof(IFilterBagJobExtensions.JobProcess<,>))]
    public interface IJobFilterBag<T> where T : struct, IFilterBag {
        void Execute(ref T bag);
    }

    public static class IFilterBagJobExtensions {
        
        public static JobHandle Schedule<T, TBag>(this T jobData, TBag bag, JobHandle inputDeps = default) where T : struct, IJobFilterBag<TBag> where TBag : struct, IFilterBag {
            return ScheduleJob(jobData, bag, inputDeps);
        }

        internal static unsafe JobHandle ScheduleJob<T, TBag>(T jobData, TBag bag, JobHandle inputDeps) where T : struct, IJobFilterBag<TBag> where TBag : struct, IFilterBag {
            
            var data = new JobData<T, TBag> {
                jobData = jobData,
                bag = bag,
            };

            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data), JobProcess<T, TBag>.Initialize(), inputDeps, ScheduleMode.Single);
            return JobsUtility.ScheduleParallelFor(ref parameters, bag.Count, MathUtils.GetScheduleBatchCount(bag.Count));
            
        }

        internal struct JobData<T, TBag> where T : struct where TBag : struct, IFilterBag {
            public T jobData;
            public TBag bag;
        }

        internal struct JobProcess<T, TBag> where T : struct, IJobFilterBag<TBag> where TBag : struct, IFilterBag {
            
            private static System.IntPtr jobReflectionData;

            public static System.IntPtr Initialize() {
                if (jobReflectionData == System.IntPtr.Zero) {
                    jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(JobData<T, TBag>), typeof(T), (ExecuteJobFunction)Execute);
                }
                return jobReflectionData;
            }

            public delegate void ExecuteJobFunction(ref JobData<T, TBag> jobData, System.IntPtr additionalData, System.IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            public static void Execute(ref JobData<T, TBag> jobData, System.IntPtr additionalData, System.IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex) {

                for (int i = 0; i < jobData.bag.Count; ++i) {
                    jobData.bag.BeginForEachIndex(i);
                    jobData.jobData.Execute(ref jobData.bag);
                    jobData.bag.EndForEachIndex();
                }
                
            }
        }
    }

}