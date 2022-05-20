namespace ME.ECS.DataConfigGenerator.DataParsers {

    public struct Fp4Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(fp4) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new fp4(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]), float.Parse(prs[3]));
            return true;

        }

    }

    public struct Fp3Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(fp3) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new fp3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]));
            return true;

        }

    }
    
    public struct Fp2Parser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(fp2) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = new fp2(float.Parse(prs[0]), float.Parse(prs[1]));
            return true;

        }

    }
    
    public struct FpQuaternionParser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(fpquaternion) == fieldType;
        }
        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = fpquaternion.Euler(new fp3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2])));
            return true;

        }

    }
    
    public struct FpParser : IParser, IDefaultParser {

        public bool IsValid(System.Type fieldType) {
            return typeof(fp) == fieldType;
        }

        public bool Parse(string data, System.Type fieldType, out object result) {
            
            var prs = data.Split(';');
            result = (fp)(float.Parse(prs[0]));
            return true;

        }

    }
    
}