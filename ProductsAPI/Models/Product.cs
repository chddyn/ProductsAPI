namespace ProductsAPI.Models
{
    public class Product
    {

        public int productId { get; set; }

        public string productName { get; set; } = null!;
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

    }
}
