using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ME.ECS.DataConfigGenerator {

    [InitializeOnLoad]
    public class EditorCoroutines {

        // This is my callable function
        public static IEnumerator StartCoroutine(IEnumerator cor) {
            
            EditorCoroutines.inProgress.Add(cor);
            cor.MoveNext();
            return cor;
            
        }

        /// <summary>
        ///  Coroutine to execute. Manage by the EasyLocalization_script
        /// </summary>
        private static List<IEnumerator> inProgress = new List<IEnumerator>();

        static EditorCoroutines() {
            
            EditorApplication.update += EditorCoroutines.ExecuteCoroutine;
            
        }

        private static void ExecuteCoroutine() {
            
            if (EditorCoroutines.inProgress.Count <= 0) {
                //  Debug.LogWarning("ping");
                return;
            }

            //Debug.LogWarning("exec");

            for (int i = 0; i < EditorCoroutines.inProgress.Count; ++i) {

                var ienum = EditorCoroutines.inProgress[i].Current as IEnumerator;
                if (ienum != null) {

                    ienum.MoveNext();
                    EditorCoroutines.inProgress.Add(ienum);

                }
                
                if (EditorCoroutines.inProgress[i].MoveNext() == false) {
                    
                    EditorCoroutines.inProgress.RemoveAt(i);
                    --i;

                }

            }
            
        }

    }

}