using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class UserRegisterModel
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must contain at least 8 characters")]
        public string Password { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No must be of 10 digits")]
        [Required(ErrorMessage = "Mobile No is required")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
