namespace BerAuto.DataContext.Entities
{
    public enum RentalStatus
    {
        Pending,
        Approved,
        Rejected,
        PickedUp,
        Returned
    }

    public class Rental
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestAddress { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public RentalStatus Status { get; set; }
        public decimal TotalCost { get; set; }
    }
}