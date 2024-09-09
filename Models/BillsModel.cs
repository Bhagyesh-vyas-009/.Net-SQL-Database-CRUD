using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class BillsModel
    {
        public int BillID { get; set; }

        [Required(ErrorMessage = "Bill Number is required")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Bill Date is required")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        public Decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public Decimal Discount { get; set; }

        [Required(ErrorMessage = "Net Amount is required")]
        public Decimal NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }

    }
}
