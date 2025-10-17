namespace ValeraProject.DTOs
{
    public class ValeraDto
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Cheerfulness { get; set; }
        public int Fatigue { get; set; }
        public int Money { get; set; }
    }

    public class ActionRequestDto
    {
        public string Action { get; set; } = string.Empty;
    }
}