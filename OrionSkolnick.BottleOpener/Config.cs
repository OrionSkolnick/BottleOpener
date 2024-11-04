/*
 * This file just loads the configuration parameters for the mod, you can add and set the defaults here
 */

using System.Text.Json.Serialization;

namespace GDWeave.BottleOpener;

public class Config {
    [JsonInclude] public bool DrunkAmplification = true; //toggleable setting which controls whether or not the effects of drunkness are amplified
}
