using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ME.ECSEditor {

    public enum AssetType {

        Prefab,
        Sprite,
        Texture,
        MonoScript,
        Shader,
        Model,
        Material,
        Scene

    }

    public static class AssetUtils {

        public static string[] GetAssetsGuids(AssetType type, string[] searchInFolders = null) {
            var filter = $"t:{type}";

            if (searchInFolders != null && searchInFolders.Length > 0) {
                return AssetDatabase.FindAssets(filter, searchInFolders);
            }

            return AssetDatabase.FindAssets(filter);
        }

        public static string[] GetAssetsGuids<T>(string[] searchInFolders = null) where T : UnityEngine.Object {
            var filter = $"t:{typeof(T).Name}";

            if (searchInFolders != null && searchInFolders.Length > 0) {
                return AssetDatabase.FindAssets(filter, searchInFolders);
            }

            return AssetDatabase.FindAssets(filter);
        }

        public static string[] GuidsToPaths(string[] guids) {
            if (guids != null) {
                var paths = new string[guids.Length];

                for (var i = 0; i < guids.Length; i++) {
                    paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                }

                return paths;
            }

            return null;
        }

        public static List<T> LoadAssetsFromGuids<T>(string[] guids) where T : UnityEngine.Object {
            var list = new List<T>();

            if (guids != null) {
                list.Capacity = guids.Length;

                for (var i = 0; i < guids.Length; i++) {
                    var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                    if (asset != null) {
                        list.Add(asset);
                    }
                }
            }

            return list;
        }

        public static void ForEachAsset<T>(Action<string, T> action, string[] searchInFolders = null) where T : UnityEngine.Object {
            if (action != null) {
                var guids = AssetUtils.GetAssetsGuids<T>(searchInFolders);
                AssetUtils.ForEachAssetInternal<T>(guids, action);
            }
        }

        public static void ForEachAsset(AssetType type, Action<string, UnityEngine.Object> action, string[] searchInFolders = null) {
            if (action != null) {
                var guids = AssetUtils.GetAssetsGuids(type, searchInFolders);
                AssetUtils.ForEachAssetInternal<UnityEngine.Object>(guids, action);
            }
        }

        public static void ForEachPrefab(Action<string, GameObject> action, string[] searchInFolders = null) {
            if (action != null) {
                var guids = AssetUtils.GetAssetsGuids(AssetType.Prefab, searchInFolders);
                AssetUtils.ForEachAssetInternal<GameObject>(guids, action);
            }
        }

        private static void ForEachAssetInternal<T>(string[] guids, Action<string, T> action, bool showProgressBar = true) where T : UnityEngine.Object {
            const string PROGRESS_BAR_TITLE = "Processing assets";

            if (action != null && guids != null && guids.Length > 0) {
                var count = guids.Length;

                try {
                    for (var i = 0; i < count; i++) {
                        var path = AssetDatabase.GUIDToAssetPath(guids[i]);

                        if (showProgressBar) {
                            EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, path, (float)(i + 1) / (float)count);
                        }

                        var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                        try {
                            action(path, asset);
                        } catch (Exception ex) {
                            Debug.LogException(ex);
                        }
                    }
                } catch (Exception any) {
                    Debug.LogException(any);
                }

                if (showProgressBar) {
                    EditorUtility.ClearProgressBar();
                }
            }
        }

    }

}