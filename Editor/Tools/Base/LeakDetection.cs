namespace ME.ECSEditor.Tools {

    using UnityEditor;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Collections;
    using UnityEngine;
    using Unity.Jobs.LowLevel.Unsafe;

    public static class LeakDetection {

        private static int savedJobWorkerCount = JobsUtility.JobWorkerCount;
        private const string USE_JOB_THREADS = "ME.ECS/Jobs/Use Job Threads";

        [MenuItem(LeakDetection.USE_JOB_THREADS, false)]
        private static void SwitchUseJobThreads() {
            if (JobsUtility.JobWorkerCount > 0) {
                LeakDetection.savedJobWorkerCount = JobsUtility.JobWorkerCount;
                try {
                    JobsUtility.JobWorkerCount = 0;
                } catch (System.ArgumentOutOfRangeException e) when (e.ParamName == "JobWorkerCount") {
                    UnityEngine.Debug.LogWarning("Disabling Job Threads requires Unity Version 2020.1.a15 or newer");
                }
            } else {
                JobsUtility.JobWorkerCount = LeakDetection.savedJobWorkerCount;
                if (LeakDetection.savedJobWorkerCount == 0) {
                    JobsUtility.ResetJobWorkerCount();
                }
            }
        }

        [MenuItem(LeakDetection.USE_JOB_THREADS, true)]
        private static bool SwitchUseJobThreadsValidate() {
            Menu.SetChecked(LeakDetection.USE_JOB_THREADS, JobsUtility.JobWorkerCount > 0);

            return true;
        }

        private const string DEBUGGER_MENU = "ME.ECS/Jobs/Jobs Debugger";

        [MenuItem(LeakDetection.DEBUGGER_MENU, false)]
        private static void SwitchJobsDebugger() {
            JobsUtility.JobDebuggerEnabled = !JobsUtility.JobDebuggerEnabled;
        }

        [MenuItem(LeakDetection.DEBUGGER_MENU, true)]
        private static bool SwitchJobsDebuggerValidate() {
            Menu.SetChecked(LeakDetection.DEBUGGER_MENU, JobsUtility.JobDebuggerEnabled);
            return true;
        }

        private const string LEAK_OFF = "ME.ECS/Leak Detection/Off";
        private const string LEAK_ON = "ME.ECS/Leak Detection/On";
        private const string LEAK_DETECTION_FULL = "ME.ECS/Leak Detection/Full Stack Traces (Expensive)";

        [MenuItem(LeakDetection.LEAK_OFF)]
        private static void SwitchLeaksOff() {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [MenuItem(LeakDetection.LEAK_ON)]
        private static void SwitchLeaksOn() {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Enabled;
        }

        [MenuItem(LeakDetection.LEAK_DETECTION_FULL)]
        private static void SwitchLeaksFull() {
            NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
        }

        [MenuItem(LeakDetection.LEAK_OFF, true)]
        private static bool SwitchLeaksOffValidate() {
            Menu.SetChecked(LeakDetection.LEAK_OFF, NativeLeakDetection.Mode == NativeLeakDetectionMode.Disabled);
            Menu.SetChecked(LeakDetection.LEAK_ON, NativeLeakDetection.Mode == NativeLeakDetectionMode.Enabled);
            Menu.SetChecked(LeakDetection.LEAK_DETECTION_FULL, NativeLeakDetection.Mode == NativeLeakDetectionMode.EnabledWithStackTrace);
            return true;
        }

    }

}