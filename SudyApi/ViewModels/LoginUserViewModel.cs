using SudyApi.Properties.Resources;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels
{
    public class LoginUserViewModel
    {
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = nameof(MessageClient.MC0003), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0004), ErrorMessageResourceType = typeof(MessageClient))]
        public string Email { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceName = nameof(MessageClient.MC0018), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0006), ErrorMessageResourceType = typeof(MessageClient))]
        public string Password { get; set; }
    }
}
