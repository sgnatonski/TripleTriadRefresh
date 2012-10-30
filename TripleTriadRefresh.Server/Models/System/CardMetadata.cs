namespace TripleTriadRefresh.Server.Models.System
{
    public class CardMetadata
    {
        public int Level { get; set; }
        public string Image { get; set; }
        public CardStrength Strength { get; set; }
        public Elemental Elemental { get; set; }
    }
}