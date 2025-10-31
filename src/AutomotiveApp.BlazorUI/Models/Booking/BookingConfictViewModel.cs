namespace AutomotiveApp.BlazorUI.Models.Booking
{
    public class BookingConflictViewModel
    {
        public Guid SessionId { get; set; }
        public required DateTime SessionDate { get; set; }
        public required string CourseName { get; set; } = string.Empty;
        public string DisplayDate => SessionDate.ToString("dddd, dd MMM yyyy");
    }
}
