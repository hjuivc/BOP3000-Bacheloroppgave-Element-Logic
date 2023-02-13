namespace WebStoreElementLogic.Data
{
    public class ProductInfo : ICloneable
    {
        public string? Name { get; set; }
<<<<<<< Updated upstream:WebStoreElementLogic/WebStoreElementLogic/Data/ProductInfo.cs
        public string? Desc { get; set; } = "Description for the product goes here..";
        public string? Id { get; set; } = "0";
=======
        public string? Desc { get; set; }
        public int? Id { get; set; } = 0;
        public string? URL { get; set; } = "images/default.jpg";
>>>>>>> Stashed changes:WebStoreElementLogic/WebStoreElementLogic/Entities/Product.cs

        public ProductInfo(int number)
        {
            Name = "Product " + (1 + number);
        }

        public ProductInfo() 
        {
            Name = "";
            Desc = "";
            Id = "";
        }


        public ProductInfo(string? name, string? desc, string? id)
        {
            Name = name;
            Desc = desc;
            Id = id;
        }

        public static List<ProductInfo> generateList(int size)
        {
            List<ProductInfo> infoList = new List<ProductInfo>();

            for (int j = 0; j < size; j++)
            {
                infoList.Add(new ProductInfo(j));
            }

            return infoList;
        }

        public object Clone()
        {
            return new ProductInfo(Name, Desc, Id);
        }

        public static void Change(ProductInfo changed, ProductInfo changer)
        {
            changed.Name = changer.Name;
            changed.Desc = changer.Desc;
            changed.Id = changer.Id;
        }
    }
}
