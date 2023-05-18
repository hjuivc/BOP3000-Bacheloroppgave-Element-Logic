using System.Runtime.CompilerServices;
using WebStoreElementLogic.Data;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Entities
{
    public class Stock
    {
        // The stock should only have Id, Quantity and ExpProductId
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ExpProductId { get; set; }
        public Stock() { }

        public Stock(int id, int quantity, int expProductId)
        {
            Id = id;
            Quantity = quantity;
            ExpProductId = expProductId;
        }
    }
}