using League.BL.Domein;

namespace TestProjectDomein
{
    public class UnitTestSpeler
    {
        [Fact]
        public void ZetId_Valid()
        {
            Speler s = new Speler(10,"jos",185,78);
        }
    }
}