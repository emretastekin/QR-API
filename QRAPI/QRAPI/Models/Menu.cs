namespace QRAPI.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public List<Food>? Foods { get; set; }
    }
}
