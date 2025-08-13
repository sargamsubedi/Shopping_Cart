using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress,ErrorMessage="email is invalid")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length should be atleasst 6 character")]
        public string Password { get; set; }

    }
}
