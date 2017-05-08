using Newtonsoft.Json;
using ShutEye.Data;
using System;

[Serializable]
public class DataObject
{
    /// <summary>
    /// Уникальный идентификатор объекта
    /// </summary>
	[JsonProperty("Id")]
    public int Id { get; set; }

    /// <summary>
    /// Имя объекта
    /// </summary>
	[JsonProperty("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Имя объекта
    /// </summary>
	[JsonProperty("LocalizationTextKey")]
    public string LocalizationTextKey { get; set; }

    public DataObject()
    {
        Name = "New";
        Id = -1;
        LocalizationTextKey = "[Emty_key]";
    }

    public DataObject(DataObject old)
    {
        Id = old.Id;
        Name = old.Name;
        LocalizationTextKey = old.LocalizationTextKey;
    }

    public override string ToString()
    {
        return SEJsonConverter.Serialize(this);
    }
}