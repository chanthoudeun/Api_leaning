using System.Text.Json.Serialization;

namespace Api_leaning.Models
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3,

    }
}