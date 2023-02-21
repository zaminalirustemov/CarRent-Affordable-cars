namespace CarRent_CarRentalWebApp.Models
{
    public class CarPeculiarity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int PeculiarityId { get; set; }

        public Car? Car { get; set; }
        public Peculiarity? Peculiarity { get; set; }
    }
}
