using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.Account
{
    public class ChangePasswordRequestDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string newPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "New password and confirmation password must match.")]
        public string ConfirmationNewPassword { get; set; }

    }
}
