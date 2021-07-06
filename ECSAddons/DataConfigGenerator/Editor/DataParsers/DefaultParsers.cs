using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.DataConfigGenerator.DataParsers {

    public struct Vector2IntParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Vector2Int) == fieldType;
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new Vector2Int(int.Parse(prs[0]), int.Parse(prs[1]));
            return true;

        }

    }

    public struct Vector3IntParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Vector3Int) == fieldType;
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new Vector3Int(int.Parse(prs[0]), int.Parse(prs[1]), int.Parse(prs[2]));
            return true;

        }

    }

    public struct Vector2Parser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Vector2) == fieldType;
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new Vector2(float.Parse(prs[0]), float.Parse(prs[1]));
            return true;

        }

    }

    public struct Vector3Parser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Vector3) == fieldType;
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new Vector3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]));
            return true;

        }

    }

    public struct Vector4Parser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Vector4) == fieldType;
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new Vector4(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]), float.Parse(prs[3]));
            return true;

        }

    }

    public struct DataConfigParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Object).IsAssignableFrom(fieldType);
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            if (DataConfigGenerator.TryToParse("config://", data, out var configName) == true) {

                result = DataConfigGenerator.GetConfig(configName);
                return true;

            }

            result = null;
            return false;

        }

    }

    public struct ScriptableObjectParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Object).IsAssignableFrom(fieldType);
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            if (DataConfigGenerator.TryToParse("so://", data, out var path) == true) {

                result = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                return true;

            }

            result = null;
            return false;

        }

    }

    public struct ViewParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Object).IsAssignableFrom(fieldType);
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            if (DataConfigGenerator.TryToParse("view://", data, out var viewPath) == true) {

                result = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(viewPath).GetComponent<ME.ECS.Views.ViewBase>();
                return true;

            }

            result = null;
            return false;

        }

    }

    public struct GameObjectParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Object).IsAssignableFrom(fieldType);
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            if (DataConfigGenerator.TryToParse("go://", data, out var goPath) == true) {

                result = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(goPath);
                return true;

            }

            result = null;
            return false;

        }

    }

    public struct ComponentParser : IParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(Object).IsAssignableFrom(fieldType);
        }

        public bool Parse(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {
            
            if (DataConfigGenerator.TryToParse("component://", data, out var componentPath) == true) {

                result = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(componentPath).GetComponent(fieldType);
                return true;

            }

            result = null;
            return false;

        }

    }

}