using Xunit;
using ValeraProject.Models;

namespace ValeraProject.Tests
{
    public class ValeraTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            var valera = new Valera();
            Assert.Equal(100, valera.Health);
            Assert.Equal(0, valera.Mana);
            Assert.Equal(0, valera.Cheerfulness);
            Assert.Equal(0, valera.Fatigue);
            Assert.Equal(100, valera.Money);
        }

        [Fact]
        public void GoToWork_Success_WhenConditionsMet()
        {
            var valera = new Valera(mana: 30, fatigue: 5);
            var result = valera.GoToWork();
            Assert.True(result);
            Assert.Equal(-5, valera.Cheerfulness);
            Assert.Equal(200, valera.Money);
        }

        [Fact]
        public void GoToWork_Fail_WhenManaTooHigh()
        {
            var valera = new Valera(mana: 60, fatigue: 5);
            var result = valera.GoToWork();
            Assert.False(result);
        }

        [Fact]
        public void DrinkWineAndWatchTV_DecreasesMoneyAndHealth()
        {
            var valera = new Valera(money: 100);
            var result = valera.DrinkWineAndWatchTV();
            Assert.True(result);
            Assert.Equal(80, valera.Money);
            Assert.Equal(95, valera.Health);
        }

        [Fact]
        public void SingInMetro_GivesBonus_WhenManaInRange()
        {
            var valera = new Valera(mana: 50, money: 100);
            valera.SingInMetro();
            Assert.Equal(160, valera.Money);
        }

        [Fact]
        public void Sleep_RestoresHealth_WhenManaLow()
        {
            var valera = new Valera(health: 50, mana: 20, fatigue: 80);
            valera.Sleep();
            Assert.Equal(100, valera.Health);
            Assert.Equal(10, valera.Fatigue);
        }
    }
}
