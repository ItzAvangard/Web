using System;

namespace ValeraProject.Models
{
    public class Valera
    {
        public int Health { get; private set; }
        public int Mana { get; private set; }
        public int Cheerfulness { get; private set; }
        public int Fatigue { get; private set; }
        public int Money { get; private set; }

        public Valera(int health = 100, int mana = 0, int cheerfulness = 0, int fatigue = 0, int money = 100)
        {
            Health = ValidateRange(health, 0, 100, "Health");
            Mana = ValidateRange(mana, 0, 100, "Mana");
            Cheerfulness = ValidateRange(cheerfulness, -10, 10, "Cheerfulness");
            Fatigue = ValidateRange(fatigue, 0, 100, "Fatigue");
            Money = money;
        }

        public bool GoToWork()
        {
            if (Mana >= 50 || Fatigue >= 10)
                return false;

            Cheerfulness = Math.Max(-10, Cheerfulness - 5);
            Mana = Math.Max(0, Mana - 30);
            Money += 100;
            Fatigue = Math.Min(100, Fatigue + 70);
            
            return true;
        }

        public void ContemplateNature()
        {
            Cheerfulness = Math.Min(10, Cheerfulness + 1);
            Mana = Math.Max(0, Mana - 10);
            Fatigue = Math.Min(100, Fatigue + 10);
        }

        public bool DrinkWineAndWatchTV()
        {
            if (Money < 20)
                return false;

            Cheerfulness = Math.Max(-10, Cheerfulness - 1);
            Mana = Math.Min(100, Mana + 30);
            Fatigue = Math.Min(100, Fatigue + 10);
            Health = Math.Max(0, Health - 5);
            Money -= 20;
            
            return true;
        }

        public bool GoToBar()
        {
            if (Money < 100)
                return false;

            Cheerfulness = Math.Min(10, Cheerfulness + 1);
            Mana = Math.Min(100, Mana + 60);
            Fatigue = Math.Min(100, Fatigue + 40);
            Health = Math.Max(0, Health - 10);
            Money -= 100;
            
            return true;
        }

        public bool DrinkWithMarginals()
        {
            if (Money < 150)
                return false;

            Cheerfulness = Math.Min(10, Cheerfulness + 5);
            Health = Math.Max(0, Health - 80);
            Mana = Math.Min(100, Mana + 90);
            Fatigue = Math.Min(100, Fatigue + 80);
            Money -= 150;
            
            return true;
        }

        public void SingInMetro()
        {
            int originalMana = Mana;
            
            Cheerfulness = Math.Min(10, Cheerfulness + 1);
            Mana = Math.Min(100, Mana + 10);
            
            int earnings = 10;
            if (originalMana > 40 && originalMana < 70)
            {
                earnings += 50;
            }
            Money += earnings;
            
            Fatigue = Math.Min(100, Fatigue + 20);
        }

        public void Sleep()
        {
            if (Mana < 30)
            {
                Health = Math.Min(100, Health + 90);
            }
            
            if (Mana > 70)
            {
                Cheerfulness = Math.Max(-10, Cheerfulness - 3);
            }
            
            Mana = Math.Max(0, Mana - 50);
            Fatigue = Math.Max(0, Fatigue - 70);
        }

        private int ValidateRange(int value, int min, int max, string propertyName)
        {
            if (value < min || value > max)
                throw new ArgumentException($"{propertyName} must be between {min} and {max}");
            return value;
        }
    }
}
