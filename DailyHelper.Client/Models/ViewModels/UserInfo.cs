using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DailyHelper.Client.Models.ViewModels
{
    public class UserInfo
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}