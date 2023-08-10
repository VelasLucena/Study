using SudyApi.Properties.Enuns;

namespace SudyApi.Models
{
    public class DataOptionsModel
    {
        public int Take { get; set; } = 100;

        public int Skip { get; set; } = 0;

        public bool IsTracking { get; set; } = false;

        public Ordering Ordering { get; set; } = Ordering.Desc;

        public string KeyOrder { get; set; }
    }
}
