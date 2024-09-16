using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class BillsModel
    {
        public int BillID { get; set; }

        [Required(ErrorMessage = "Bill Number is required")]
        [RegularExpression(@"^BILL\d+$", ErrorMessage = "ex- BILL001")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Bill Date is required")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Order Number is required")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Amount must be grater than 0")]
        public Decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public Decimal Discount { get; set; }

        [Required(ErrorMessage = "Net Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Net Amount must be grater than 0")]
        public Decimal NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }

    }
}
