using StudandoApi.Properties.Enuns;
using SudyApi.Properties.Attributes;
using SudyApi.Properties.Resources;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels
{
    public class RegisterUserViewModel
    {
        [DataType(DataType.Text, ErrorMessageResourceName = nameof(MessageClient.MC0001), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0002), ErrorMessageResourceType = typeof(MessageClient))]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessageResourceName = nameof(MessageClient.MC0003), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0004), ErrorMessageResourceType = typeof(MessageClient))]
        public string Email { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceName = nameof(MessageClient.MC0003), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0004), ErrorMessageResourceType = typeof(MessageClient))]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = nameof(MessageClient.MC0005), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0006), ErrorMessageResourceType = typeof(MessageClient))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0007), ErrorMessageResourceType = typeof(MessageClient))]
        public int Age { get; set; }

        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0008), ErrorMessageResourceType = typeof(MessageClient))]
        [ValidationCpf(ErrorMessageResourceName = nameof(MessageClient.MC0009), ErrorMessageResourceType = typeof(MessageClient))]
        public string Cpf { get; set; }

        //[RegularExpression(@"[a-zA-Z]", ErrorMessageResourceName = nameof(MessageClient.MC0018), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0010), ErrorMessageResourceType = typeof(MessageClient))]
        public string Rg { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceName = nameof(MessageClient.MC0012), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0011), ErrorMessageResourceType = typeof(MessageClient))]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0013), ErrorMessageResourceType = typeof(MessageClient))]
        public string Address { get; set; }

        [DataType(DataType.PostalCode, ErrorMessageResourceName = nameof(MessageClient.MC0014), ErrorMessageResourceType = typeof(MessageClient))]
        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0015), ErrorMessageResourceType = typeof(MessageClient))]
        public string Cep { get; set; }

        [Required(ErrorMessageResourceName = nameof(MessageClient.MC0017), ErrorMessageResourceType = typeof(MessageClient))]
        public GenderType Gender { get; set; }
    }
}
