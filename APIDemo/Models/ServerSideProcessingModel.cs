namespace APIDemo.Models
{
    public class ServerSideProcessingModel
    {
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 0;
        public int OrderIndex { get; set; } = 0;
        public string OrderBy { get; set; }
        public string OrderByAsc { get; set; } = "ASC";

        public string ConvertOrderBy(int orderIndex)
        {
            string result = string.Empty;

            switch (orderIndex)
            {
                case 0:
                    result = "Id";
                    break;
                case 1:
                    result = "Name";
                    break;
                case 2:
                    result = "Gender";
                    break;
                case 3:
                    result = "Blood";
                    break;
                case 4:
                    result = "Height";
                    break;
                case 5:
                    result = "Weight";
                    break;
                case 6:
                    result = "Coupon";
                    break;
            }

            return result;
        }
    }
}