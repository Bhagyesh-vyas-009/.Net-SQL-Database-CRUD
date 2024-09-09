using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Home Address is required")]
        public string HomeAddress {  get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "GST NO is required")]
        public string GSTNO { get; set; }

        [Required(ErrorMessage = "City Name is required")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "PinCode is required")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Net Amount is required")]
        public Decimal NetAmount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }
    }
}
