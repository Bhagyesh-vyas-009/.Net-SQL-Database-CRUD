using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductID {  get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be grater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be grater than 0")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Amount must be grater than 0")]
        public Decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }
    }
}
