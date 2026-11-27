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

        /// <summary>
        /// Messages about the dependencies of a mod, which are shown in the mod manager.
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
            return messages;
        }
    }
}
