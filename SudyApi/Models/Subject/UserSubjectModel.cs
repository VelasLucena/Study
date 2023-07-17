namespace SudyApi.Models.Subject
{
    public class UserSubjectModel
    {
        public int UserSubjectId { get; set; }

        public int? UserId { get; set; }

        public SubjectModel Subject { get; set; }
    }
}
