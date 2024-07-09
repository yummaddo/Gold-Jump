namespace Game.GamePlay.Boosts
{
    public interface IBoost
    {
        public string BoostPrefsTitle { get; set; }
        public int Price { get; set; }
        public void Activate();
    }
}