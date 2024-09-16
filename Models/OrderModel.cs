using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class OrderModel
    {
        public int OrderID {  get; set; }
        [Required(ErrorMessage ="Order Date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order Number is required")]
        [RegularExpression(@"^ORD\d+$", ErrorMessage = "ex- ORD001")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerID {  get; set; }

        [Required(ErrorMessage = "Payment Mode is required")]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        //[MinLength(0)]
        [Range(1,int.MaxValue,ErrorMessage ="Total Amount must be grater than 0")]
        public Decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping Address is required")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public int UserID { get; set; }

    }

    

   
}
