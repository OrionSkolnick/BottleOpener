using System.Text.Json.Serialization;

namespace GDWeave.BottleOpener;

public class Config {
    [JsonInclude] public bool DrunkAmplification = true;
}
