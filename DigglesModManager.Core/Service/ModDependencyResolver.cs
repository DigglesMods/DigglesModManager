using DigglesModManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigglesModManager.Service
{
    /// <summary>
    /// Handles the dependencies between mods (config.json: "dependencies").
    /// Mods are identified by their directory name.
    /// </summary>
    public static class ModDependencyResolver
    {
        private static IEnumerable<ModDependency> DependenciesOf(Mod mod)
        {
            return mod.Config?.Dependencies?.Where(d => !string.IsNullOrWhiteSpace(d?.Id)) ?? Enumerable.Empty<ModDependency>();
        }

        private static bool IsSameMod(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns all mods that have to be activated together with the given mod (required dependencies,
        /// recursively), in the order in which they should be applied. The given mod itself is not part of the result.
        /// </summary>
        /// <param name="mod">the mod to be activated</param>
        /// <param name="installedMods">all installed mods</param>
        /// <param name="missing">ids of required mods that are not installed</param>
        public static List<Mod> GetRequiredMods(Mod mod, IEnumerable<Mod> installedMods, out List<string> missing)
        {
            var installed = installedMods.ToList();
            var result = new List<Mod>();
            var missingIds = new List<string>();
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { mod.ModDirectoryName };

            void Visit(Mod current)
            {
                foreach (var dependency in DependenciesOf(current).Where(d => d.Required))
                {
                    if (!visited.Add(dependency.Id))
                    {
                        continue;
                    }
                    var dependencyMod = installed.FirstOrDefault(m => IsSameMod(m.ModDirectoryName, dependency.Id));
                    if (dependencyMod == null)
                    {
                        missingIds.Add(dependency.Id);
                        continue;
                    }
                    Visit(dependencyMod);
                    result.Add(dependencyMod);
                }
            }

            Visit(mod);
            missing = missingIds;
            return result;
        }

        /// <summary>
        /// Returns the active mods which require the given mod (directly or indirectly).
        /// </summary>
        public static List<Mod> GetDependentMods(Mod mod, IEnumerable<Mod> activeMods)
        {
            var active = activeMods.ToList();
            var result = new List<Mod>();
            var queue = new Queue<Mod>();
            queue.Enqueue(mod);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var other in active)
                {
                    if (other == mod || result.Contains(other))
                    {
                        continue;
                    }
                    if (DependenciesOf(other).Any(d => d.Required && IsSameMod(d.Id, current.ModDirectoryName)))
                    {
                        result.Add(other);
                        queue.Enqueue(other);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the ids of required mods of the given mod that are not active.
        /// </summary>
        public static List<string> GetMissingRequiredMods(Mod mod, IEnumerable<Mod> activeMods)
        {
            var active = activeMods.ToList();
            return DependenciesOf(mod)
                .Where(d => d.Required && !active.Any(m => IsSameMod(m.ModDirectoryName, d.Id)))
                .Select(d => d.Id)
                .ToList();
        }

        /// <summary>
        /// Sorts the active mods so that each mod comes after all active mods it depends on (required and optional).
        /// Otherwise the given order is kept as far as possible (stable topological sort).
        /// </summary>
        /// <param name="activeMods">the active mods in the order chosen by the user</param>
        /// <param name="circularMods">mods with circular dependencies (they keep their order)</param>
        public static List<Mod> SortByDependencies(IList<Mod> activeMods, out List<Mod> circularMods)
        {
            var remaining = activeMods.ToList();
            var result = new List<Mod>();
            circularMods = new List<Mod>();

            bool DependsOnRemaining(Mod mod)
            {
                return DependenciesOf(mod).Any(d => remaining.Any(r => r != mod && IsSameMod(r.ModDirectoryName, d.Id)));
            }

            while (remaining.Count > 0)
            {
                // the first mod (in user order) whose dependencies are all placed already
                var next = remaining.FirstOrDefault(m => !DependsOnRemaining(m));
                if (next == null)
                {
                    // circular dependency: keep the user order of the rest
                    circularMods.AddRange(remaining);
                    result.AddRange(remaining);
                    break;
                }
                result.Add(next);
                remaining.Remove(next);
            }
            return result;
        }

        private static IEnumerable<ModIncompatibility> IncompatibilitiesOf(Mod mod)
        {
            return mod.Config?.Incompatibilities?.Where(i => !string.IsNullOrWhiteSpace(i?.Id)) ?? Enumerable.Empty<ModIncompatibility>();
        }

        /// <summary>
        /// Returns the incompatibility between two mods or null. It is enough if one of both mods declares it.
        /// If both mods declare it, the stricter type wins.
        /// </summary>
        public static ModIncompatibility GetIncompatibility(Mod a, Mod b)
        {
            if (a == b || IsSameMod(a.ModDirectoryName, b.ModDirectoryName))
            {
                return null;
            }
            return IncompatibilitiesOf(a).Where(i => IsSameMod(i.Id, b.ModDirectoryName))
                .Concat(IncompatibilitiesOf(b).Where(i => IsSameMod(i.Id, a.ModDirectoryName)))
                .OrderBy(i => i.Type) // Error before Warning
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks if two mods are incompatible (with the given type, or with any type if null).
        /// </summary>
        public static bool AreIncompatible(Mod a, Mod b, ModIncompatibilityType? type = null)
        {
            var incompatibility = GetIncompatibility(a, b);
            return incompatibility != null && (type == null || incompatibility.Type == type);
        }

        /// <summary>
        /// Returns the mods of the given list that are incompatible with the given mod (with the given type, or with any type if null).
        /// </summary>
        public static List<Mod> GetIncompatibleMods(Mod mod, IEnumerable<Mod> mods, ModIncompatibilityType? type = null)
        {
            return mods.Where(other => AreIncompatible(mod, other, type)).ToList();
        }

        /// <summary>
        /// Returns all pairs of incompatible mods in the given list (with the given type, or with any type if null).
        /// </summary>
        public static List<(Mod, Mod)> GetIncompatiblePairs(IList<Mod> mods, ModIncompatibilityType? type = null)
        {
            var pairs = new List<(Mod, Mod)>();
            for (var i = 0; i < mods.Count; i++)
            {
                for (var j = i + 1; j < mods.Count; j++)
                {
                    if (AreIncompatible(mods[i], mods[j], type))
                    {
                        pairs.Add((mods[i], mods[j]));
                    }
                }
            }
            return pairs;
        }

        /// <summary>
        /// Messages about the dependencies and incompatibilities of a mod, which are shown in the mod manager.
        /// </summary>
        /// <param name="mod">the mod</param>
        /// <param name="installedMods">all installed mods (to show their names and detect missing mods)</param>
        public static List<ModMessage> GetDependencyMessages(Mod mod, IEnumerable<Mod> installedMods)
        {
            var installed = installedMods.ToList();
            var messages = new List<ModMessage>();
            foreach (var dependency in DependenciesOf(mod))
            {
                var dependencyMod = installed.FirstOrDefault(m => IsSameMod(m.ModDirectoryName, dependency.Id));
                var name = dependencyMod != null ? $"\"{dependencyMod}\"" : $"\"{dependency.Id}\"";
                ModTranslationString text;
                ModMessageType type;
                if (dependencyMod == null && dependency.Required)
                {
                    type = ModMessageType.Error;
                    text = new ModTranslationString($"Requires the mod {name}, which is not installed.")
                    {
                        de = $"Benötigt die Mod {name}, die nicht installiert ist."
                    };
                }
                else if (dependency.Required)
                {
                    type = ModMessageType.Info;
                    text = new ModTranslationString($"Requires the mod {name} (it is activated automatically).")
                    {
                        de = $"Benötigt die Mod {name} (wird automatisch aktiviert)."
                    };
                }
                else
                {
                    if (dependencyMod == null)
                    {
                        continue;
                    }
                    type = ModMessageType.Info;
                    text = new ModTranslationString($"Is applied after the mod {name}, if it is active.")
                    {
                        de = $"Wird nach der Mod {name} angewendet, falls diese aktiv ist."
                    };
                }
                messages.Add(new ModMessage { Type = type, Content = text });
            }
            // incompatibilities declared by this mod or by the other mod
            foreach (var other in installed)
            {
                var incompatibility = GetIncompatibility(mod, other);
                if (incompatibility == null)
                {
                    continue;
                }
                ModTranslationString text;
                if (incompatibility.Type == ModIncompatibilityType.Error)
                {
                    text = new ModTranslationString($"Cannot be used together with the mod \"{other}\".")
                    {
                        de = $"Kann nicht zusammen mit der Mod \"{other}\" verwendet werden."
                    };
                }
                else
                {
                    text = new ModTranslationString($"Not fully compatible with the mod \"{other}\": if both are active, the changes of the mod higher in the list win.")
                    {
                        de = $"Nicht vollständig kompatibel mit der Mod \"{other}\": Sind beide aktiv, gewinnen die Änderungen der Mod, die weiter oben steht."
                    };
                }
                if (incompatibility.Reason != null)
                {
                    text.en += $" {incompatibility.Reason.getString("en")}";
                    text.de += $" {incompatibility.Reason.getString("de")}";
                }
                messages.Add(new ModMessage { Type = ModMessageType.Warning, Content = text });
            }
            return messages;
        }
    }
}
