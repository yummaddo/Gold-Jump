namespace Game.Core.Abstraction
{
    public interface ISetting
    {
        public bool LoadNexLevel{ get;  set; }
        public bool GoadAlivePlayer { get; set; }
        public float LoadNexLevelTime { get; set; }
        public float ReloadSceneTime { get; set; }
    }
}