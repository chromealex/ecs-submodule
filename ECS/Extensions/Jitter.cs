using System.Linq;

namespace ME.ECS.Extensions {

    public class Jitter {

        private System.Threading.Thread[] jitterThreads;

        private class ThreadState {

            public int offset;
            public int count;

        }

        public void Start() {

            var list = new System.Collections.Generic.List<System.Reflection.MethodInfo>();
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(x => (x.FullName.Contains("ProjectX") || x.FullName.Contains("ECS")) == true && x.FullName.Contains("Editor") == false)) {
                foreach (var type in asm.GetTypes()) {
                    try {
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                    } catch (System.Exception) { }

                    foreach (var method in type.GetMethods((System.Reflection.BindingFlags)(long)-1)) {
                        if ((method.Attributes & System.Reflection.MethodAttributes.Abstract) == System.Reflection.MethodAttributes.Abstract) {
                            continue;
                        }

                        list.Add(method);
                    }
                }
            }

            var threads = UnityEngine.SystemInfo.processorCount - 1;
            using (NoStackTrace.All) UnityEngine.Debug.Log($"[Jitter] Initialized with thread count {threads} while processor count {UnityEngine.SystemInfo.processorCount} (including main thread)");
            if (threads < 1) threads = 1;
            this.jitterThreads = new System.Threading.Thread[threads];
            var countPerThread = list.Count / threads;
            for (int j = 0; j < threads; ++j) {

                var jitter = new System.Threading.Thread((st) => {

                    var state = (ThreadState)st;
                    var count = 0;
                    var countGenerics = 0;
                    var methodTime = 0L;
                    var sw = new System.Diagnostics.Stopwatch();
                    var swMethod = new System.Diagnostics.Stopwatch();
                    System.Reflection.MethodInfo largestMethod = null;
                    sw.Start();
                    UnityEngine.Debug.Log($"[Jitter] Started: {state.offset}..{(state.offset + state.count)}");
                    for (int i = state.offset; i < state.offset + state.count; ++i) {

                        var method = list[i];
                        swMethod.Reset();
                        swMethod.Start();
                        if (method.IsGenericMethod == true) {

                            var m = method.MakeGenericMethod(method.GetGenericArguments());
                            System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(m.MethodHandle);
                            ++countGenerics;

                        } else if (method.IsGenericMethodDefinition == true) {

                            var gm = method.GetGenericMethodDefinition();
                            System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(gm.MethodHandle);
                            ++countGenerics;

                        } else {

                            try {
                                var ptr = method.MethodHandle.GetFunctionPointer();
                            } catch (System.Exception) { }

                            var body = method.GetMethodBody();
                            System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);

                        }

                        swMethod.Stop();
                        if (swMethod.ElapsedMilliseconds > methodTime) {

                            methodTime = swMethod.ElapsedMilliseconds;
                            largestMethod = method;

                        }

                        ++count;

                    }

                    sw.Stop();

                    if (largestMethod != null) {

                        UnityEngine.Debug.LogWarning($"[Jitter] methods: {count} included generics: {countGenerics}, elapsed: {sw.ElapsedMilliseconds}ms (largest method: {methodTime}ms: {largestMethod} [{largestMethod.Module}])");

                    } else {
                        
                        UnityEngine.Debug.LogWarning($"[Jitter] methods: {count} included generics: {countGenerics}, elapsed: {sw.ElapsedMilliseconds}ms");

                    }

                });

                jitter.Priority = System.Threading.ThreadPriority.Lowest;
                jitter.Start(new ThreadState() {
                    offset = countPerThread * j,
                    count = countPerThread,
                });

                this.jitterThreads[j] = jitter;

            }

        }

        public void Dispose() {

            if (this.jitterThreads != null) {

                for (int i = 0; i < this.jitterThreads.Length; ++i) {

                    if (this.jitterThreads[i] != null) this.jitterThreads[i].Abort();

                }

            }

            this.jitterThreads = null;

        }

    }

}