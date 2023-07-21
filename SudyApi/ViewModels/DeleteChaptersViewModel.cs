using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels
{
    public class DeleteChaptersViewModel
    {
        [Required]
        public int ChapterId { get; set; }
    }
}
