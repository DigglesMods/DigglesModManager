using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace DigglesModManager.Model
{
    public class ModDirectoryCondition
    {
        /// <summary>
        /// The type of the directory condition. [REQUIRED]
        /// </summary>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public ModDirectoryConditionType Type { get; set; }

        /// <summary>
        /// ID of the variable or mod that has to be checked. [REQUIRED]
        /// (Type Mod: mod directory name; Type Variable: mod variable id)
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// The variable has to have this value to evaluate the condtion as true.
        /// (required for type "variable")
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "value", Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate)]
        public object Value { get; set; }

        public bool IsTrue(Mod mod, List<Mod> activeMods)
        {
            switch (Type)
            {
                case ModDirectoryConditionType.Mod:
                    foreach (var activeMod in activeMods)
                    {
                        if (activeMod.ModDirectoryName.Equals(Id))
                        {
                            return true;
                        }
                    }
                    //no warning, because the existence of a mod is checked.
                    break;
                case ModDirectoryConditionType.Variable:
                    if (Value == null)
                    {
                        Log.Warning($"{mod.ModDirectoryName}: Variable with ID \"{Id}\" needs a value.");
                        break;
                    }
                    foreach (var modVariable in mod.Config.SettingsVariables)
                    {
                        if (modVariable.ID.Equals(Id))
                        {
                            return modVariable.Value.Equals(Value);
                        }
                    }
                    Log.Warning($"{mod.ModDirectoryName}: Variable with ID \"{Id}\" not found");
                    break;
                default:
                    Log.Warning($"{mod.ModDirectoryName}: Unhandled ModDirectroyConditionType \"{Type}\"");
                    break;
            }
            return false;
        }
    }
}
