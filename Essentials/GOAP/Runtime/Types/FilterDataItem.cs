namespace ME.ECS.Essentials.GOAP {

    public struct FilterDataItem {

        public int typeId;
        public byte hasData;
        public UnsafeData data;

        public static FilterDataItem Create(int typeId, byte hasData, UnsafeData data) {
            var item = new FilterDataItem() {
                typeId = typeId,
                hasData = hasData,
                data = data,
            };
            return item;
        }

    }

}