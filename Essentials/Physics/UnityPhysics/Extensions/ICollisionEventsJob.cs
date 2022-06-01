using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace ME.ECS.Essentials.Physics
{
    // INTERNAL UnityPhysics interface for jobs that iterate through the list of collision events produced by the solver.
    // Important: Only use inside UnityPhysics code! Jobs in other projects should implement ICollisionEventsJob.
    [JobProducerType(typeof(ICollisionEventJobExtensions.CollisionEventJobProcess<>))]
    public interface ICollisionEventsJobBase
    {
        void Execute(CollisionEvent collisionEvent);
    }

    // Interface for jobs that iterate through the list of collision events produced by the solver.
    public interface ICollisionEventsJob : ICollisionEventsJobBase
    {
    }

    public static class ICollisionEventJobExtensions
    {
        // Default Schedule() implementation for ICollisionEventsJob.
        public static unsafe JobHandle Schedule<T>(this T jobData, JobHandle inputDeps = default)
            where T : struct, ICollisionEventsJobBase
        {
            return ScheduleUnityPhysicsCollisionEventsJob(jobData, inputDeps);
        }

        // Schedules a collision events job only for UnityPhysics simulation
        internal static unsafe JobHandle ScheduleUnityPhysicsCollisionEventsJob<T>(T jobData, JobHandle inputDeps)
            where T : struct, ICollisionEventsJobBase
        {
            //SafetyChecks.CheckAreEqualAndThrow(SimulationType.UnityPhysics, simulation.Type);

            var internalData = ME.ECS.Worlds.current.ReadSharedDataOneShot<ME.ECS.Essentials.Physics.Components.PhysicsOneShotInternal>();
            var data = new CollisionEventJobData<T>
            {
                UserJobData = jobData,
                EventReader = internalData.collisionEvents,
            };

            // Ensure the input dependencies include the end-of-simulation job, so events will have been generated
            //inputDeps = JobHandle.CombineDependencies(inputDeps, simulation.FinalSimulationJobHandle);
            var parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref data), CollisionEventJobProcess<T>.Initialize(), inputDeps, ScheduleMode.Single);
            return JobsUtility.Schedule(ref parameters);
        }

        internal unsafe struct CollisionEventJobData<T> where T : struct
        {
            public T UserJobData;
            [NativeDisableContainerSafetyRestriction] public CollisionEvents EventReader;
        }

        internal struct CollisionEventJobProcess<T> where T : struct, ICollisionEventsJobBase
        {
            static IntPtr jobReflectionData;

            public static IntPtr Initialize()
            {
                if (jobReflectionData == IntPtr.Zero)
                {
                    jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(CollisionEventJobData<T>), typeof(T), (ExecuteJobFunction)Execute);
                }
                return jobReflectionData;
            }

            public delegate void ExecuteJobFunction(ref CollisionEventJobData<T> jobData, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

            public unsafe static void Execute(ref CollisionEventJobData<T> jobData, IntPtr additionalData,
                IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
            {
                foreach (CollisionEvent collisionEvent in jobData.EventReader)
                {
                    jobData.UserJobData.Execute(collisionEvent);
                }
            }
        }
    }
}
