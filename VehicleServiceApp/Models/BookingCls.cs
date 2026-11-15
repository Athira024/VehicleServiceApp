namespace VehicleServiceApp.Models
{
    public class BookingCls
    {
        public int Booking_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Vehicle_Id { get; set; }
        public int Service_Id { get; set; }
        public DateTime Booking_Date { get; set; }
        public DateTime Service_Date { get; set; }
        public string? Service_Mode { get; set; }
    }
}
