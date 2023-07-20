using StudandoApi.Properties.Enuns;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SudyApi.Models
{
    public class UserInformation
    {
        [Key]
        public int UserInformationId { get; set; }

        public string? PhoneNumber { get; set; }

        public int? Age { get; set; }

        public string? Cpf { get; set; }

        public string? Rg { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

        public string? Cep { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public UserInformation() { }

        public UserInformation(RegisterUserViewModel viewModel)
        {
            PhoneNumber = viewModel.PhoneNumber;
            Age = viewModel.Age;
            Cpf = viewModel.Cpf;
            Rg = viewModel.Rg;
            Gender = viewModel.Gender;
            Birthday = viewModel.Birthday;
            Address = viewModel.Address;
            Cep = viewModel.Cep;
            CreationDate = DateTime.Now;
        }

        public void Update(EditUserViewModel viewModel)
        {
            PhoneNumber = viewModel.PhoneNumber != null ? viewModel.PhoneNumber : PhoneNumber;
            Age = viewModel.Age != null ? viewModel.Age : Age;
            Cpf = viewModel.Cpf != null ? viewModel.Cpf : Cpf;
            Rg = viewModel.Rg != null ? viewModel.Rg : Rg;
            Gender = viewModel.Gender != null ? viewModel.Gender : Gender;
            Birthday = viewModel.Birthday != null ? viewModel.Birthday : Birthday;
            Address = viewModel.Address != null ? viewModel.Address : Address;
            Cep = viewModel.Cep != null ? viewModel.Cep : Cep;
            UpdateDate = DateTime.Now;
        }
    }
}
