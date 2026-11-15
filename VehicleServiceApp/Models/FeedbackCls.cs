namespace VehicleServiceApp.Models
{
    public class FeedbackCls
    {
        public int Feedback_Id { get; set; }
        public int Customer_Id { get; set; }
        public string? Feedback { get; set; }
        public string? Reply { get; set; }
        public DateTime F_Date { get; set; }
    }
}
