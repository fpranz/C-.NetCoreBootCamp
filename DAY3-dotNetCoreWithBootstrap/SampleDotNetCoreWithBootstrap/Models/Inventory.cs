namespace SampleDotNetCoreWithBootstrap
{
    public class Inventory {

        public Inventory (int ItemID, string ItemName, string ItemDescription, int ItemCode, int Quantity, string CreateDate)
        {
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.ItemDescription = ItemDescription;
            this.ItemCode = ItemCode;
            this.Quantity = Quantity;
            this.CreateDate = CreateDate;

        }

        public int ItemID {get; set;}
        public string? ItemName {get; set;}
        public string? ItemDescription {get; set;}
        public int ItemCode {get; set;}
        public int Quantity {get; set;}
        public string? CreateDate {get; set;}
    }
}
