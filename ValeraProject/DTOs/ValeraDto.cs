namespace ValeraProject.DTOs
{
    public class ValeraDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Cheerfulness { get; set; }
        public int Fatigue { get; set; }
        public int Money { get; set; }
    }

    public class CreateValeraDto
    {
        public string Name { get; set; } = "Valera";
        public int Health { get; set; } = 100;
        public int Mana { get; set; } = 0;
        public int Cheerfulness { get; set; } = 0;
        public int Fatigue { get; set; } = 0;
        public int Money { get; set; } = 100;
    }

    public class ActionRequestDto
    {
        public string Action { get; set; } = string.Empty;
    }
}