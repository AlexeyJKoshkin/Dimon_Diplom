using Newtonsoft.Json;

namespace ShutEye.Data.Objects
{
    public class DPrefab : DataObject
    {
        /// <summary>
        /// Префаб предмета для отображения в игровом мире
        /// </summary>
		[JsonProperty("Prefab")]
        public string Prefab { get; set; }

        [JsonProperty("Icon")]
        public string Icon { get; set; }

        [JsonProperty("VieweSprite")]
        public string ViewSprite { get; set; }

        public DPrefab() : base()
        {
            Prefab = string.Empty;
            Icon = string.Empty;
        }

        public DPrefab(DPrefab old)
        {
            Prefab = old.Prefab;
            Icon = old.Icon;
        }
    }
}