namespace LostDogApp.Models
{
    public class LostDogReport
    {
        public int Id { get; set; }
        public string DogName { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string UserId { get; set; }
    }
}
