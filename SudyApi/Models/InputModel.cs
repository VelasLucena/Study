namespace SudyApi.Models
{
    public class InputModel
    {
        public string? Url { get; set; }

        public string? Request { get; set; }

        public string? Response { get; set; }

        public TimeSpan? TimeRequest { get; set; }

        public int? UserId { get; set; }

        public InputModel(string request, string response, TimeSpan timeRequest, string url)
        {
            Request = request != null ? request : string.Empty;
            Response = response != null ? response : string.Empty;
            UserId = UserLogged.UserId;
            TimeRequest = timeRequest;
            Url = url;
        }

        public InputModel() { }
    }
}
