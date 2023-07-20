using StudandoApi.Properties.Enuns;
using SudyApi.Properties.Attributes;
using SudyApi.Properties.Resources;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int UserInformationId { get; set; }

        [DataType(DataType.Text, ErrorMessageResourceName = nameof(MessageClient.MC0001), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessageResourceName = nameof(MessageClient.MC0003), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Email { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceName = nameof(MessageClient.MC0003), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Password { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = nameof(MessageClient.MC0005), ErrorMessageResourceType = typeof(MessageClient))]
        public string? PhoneNumber { get; set; }

        public int? Age { get; set; }

        [ValidationCpf(ErrorMessageResourceName = nameof(MessageClient.MC0009), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Cpf { get; set; }

        //[RegularExpression(@"[a-zA-Z]", ErrorMessageResourceName = nameof(MessageClient.MC0018), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Rg { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceName = nameof(MessageClient.MC0012), ErrorMessageResourceType = typeof(MessageClient))]
        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

        [DataType(DataType.PostalCode, ErrorMessageResourceName = nameof(MessageClient.MC0014), ErrorMessageResourceType = typeof(MessageClient))]
        public string? Cep { get; set; }

        public Gender? Gender { get; set; }
    }
}
