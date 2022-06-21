using ME.ECS.Mathematics;

namespace ME.ECS.DataConfigGenerator.DataParsers {

    public struct Fp4Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(float4) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new float4(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]), float.Parse(prs[3]));
            return true;

        }

    }

    public struct Fp3Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(float3) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new float3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]));
            return true;

        }

    }
    
    public struct Fp2Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(float2) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new float2(float.Parse(prs[0]), float.Parse(prs[1]));
            return true;

        }

    }
    
    public struct FpQuaternionParser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(quaternion) == fieldType;
        }
        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = quaternion.Euler(new float3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2])));
            return true;

        }

    }
    
    public struct FpParser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(sfloat) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = (sfloat)(float.Parse(prs[0]));
            return true;

        }

    }
    
}