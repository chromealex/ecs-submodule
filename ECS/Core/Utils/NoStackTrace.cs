
namespace ME.ECS {

    [System.Flags]
    public enum LogTypeFlags {
        /// <summary>
        ///   <para>LogType used for Errors.</para>
        /// </summary>
        Error = 0x1,
        /// <summary>
        ///   <para>LogType used for Asserts. (These could also indicate an error inside Unity itself.)</para>
        /// </summary>
        Assert = 0x2,
        /// <summary>
        ///   <para>LogType used for Warnings.</para>
        /// </summary>
        Warning = 0x4,
        /// <summary>
        ///   <para>LogType used for regular log messages.</para>
        /// </summary>
        Log = 0x8,
        /// <summary>
        ///   <para>LogType used for Exceptions.</para>
        /// </summary>
        Exception = 0x10,
    }

    public readonly struct NoStackTrace : System.IDisposable {

        public static NoStackTrace All => new NoStackTrace(LogTypeFlags.Log | LogTypeFlags.Warning | LogTypeFlags.Error | LogTypeFlags.Exception | LogTypeFlags.Assert);
        public static NoStackTrace Logs => new NoStackTrace(LogTypeFlags.Log);
        public static NoStackTrace Warnings => new NoStackTrace(LogTypeFlags.Warning);
        public static NoStackTrace Errors => new NoStackTrace(LogTypeFlags.Error);
        public static NoStackTrace Asserts => new NoStackTrace(LogTypeFlags.Assert);
        
        private readonly UnityEngine.StackTraceLogType logType;
        private readonly UnityEngine.StackTraceLogType assertType;
        private readonly UnityEngine.StackTraceLogType errorType;
        private readonly UnityEngine.StackTraceLogType exceptionType;
        private readonly UnityEngine.StackTraceLogType warningType;

        public NoStackTrace(LogTypeFlags logTypesFlags) {

            this.logType = UnityEngine.Application.GetStackTraceLogType(UnityEngine.LogType.Log);
            this.assertType = UnityEngine.Application.GetStackTraceLogType(UnityEngine.LogType.Assert);
            this.errorType = UnityEngine.Application.GetStackTraceLogType(UnityEngine.LogType.Error);
            this.exceptionType = UnityEngine.Application.GetStackTraceLogType(UnityEngine.LogType.Exception);
            this.warningType = UnityEngine.Application.GetStackTraceLogType(UnityEngine.LogType.Warning);
            
            if ((logTypesFlags & LogTypeFlags.Log) != 0) UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Log, UnityEngine.StackTraceLogType.None);
            if ((logTypesFlags & LogTypeFlags.Assert) != 0) UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);
            if ((logTypesFlags & LogTypeFlags.Error) != 0) UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Error, UnityEngine.StackTraceLogType.None);
            if ((logTypesFlags & LogTypeFlags.Exception) != 0) UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Exception, UnityEngine.StackTraceLogType.None);
            if ((logTypesFlags & LogTypeFlags.Warning) != 0) UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Warning, UnityEngine.StackTraceLogType.None);

        }
		    
        public void Dispose() {

            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Log, this.logType);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, this.assertType);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Error, this.errorType);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Exception, this.exceptionType);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Warning, this.warningType);

        }

    }
    
}