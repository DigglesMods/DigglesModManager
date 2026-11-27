using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigglesModManager
{
    /// <summary>
    /// Reads and writes the JSON files of the mod manager with System.Text.Json.
    /// The options are chosen to be compatible with the files that were written/read with Newtonsoft.Json before.
    /// </summary>
    public static class JsonHelper
    {
        public static readonly JsonSerializerOptions Options = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                WriteIndented = true,
                // keep umlauts etc. readable in diggles-mod-manager.json
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            // enums are written as strings in config.json ("bool", "data", "warning", ...); numbers are accepted, too
            options.Converters.Add(new JsonStringEnumConverter());
            // deserialize "object" properties to bool/long/double/string like Newtonsoft.Json did (instead of JsonElement)
            options.Converters.Add(new PrimitiveObjectConverter());
            return options;
        }

        public static T ReadFile<T>(string path)
        {
            var json = ReadJsonText(path);
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        /// <summary>
        /// Reads a JSON file as text. Files with BOM are decoded accordingly, files without BOM as UTF-8.
        /// Some mods have config files in the ANSI code page without BOM (which worked with the .NET Framework
        /// versions of the mod manager): if the content is no valid UTF-8, the ANSI code page is used.
        /// </summary>
        public static string ReadJsonText(string path)
        {
            var bytes = File.ReadAllBytes(path);
            using (var reader = new StreamReader(new MemoryStream(bytes), StrictUtf8, true))
            {
                try
                {
                    return reader.ReadToEnd(); // detects BOM (UTF-8, UTF-16, UTF-32)
                }
                catch (DecoderFallbackException)
                {
                    return GameEncoding.Ansi.GetString(bytes);
                }
            }
        }

        private static readonly Encoding StrictUtf8 = new UTF8Encoding(false, true);

        public static void WriteFile<T>(string path, T value)
        {
            var json = JsonSerializer.Serialize(value, Options);
            File.WriteAllText(path, json, GameEncoding.Json);
        }

        /// <summary>
        /// Converts JSON primitives to the CLR types Newtonsoft.Json used for "object" properties.
        /// </summary>
        private sealed class PrimitiveObjectConverter : JsonConverter<object>
        {
            public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.True:
                        return true;
                    case JsonTokenType.False:
                        return false;
                    case JsonTokenType.Null:
                        return null;
                    case JsonTokenType.String:
                        return reader.GetString();
                    case JsonTokenType.Number:
                        if (reader.TryGetInt64(out var l))
                        {
                            return l;
                        }
                        return reader.GetDouble();
                    default:
                        // arrays and objects are not used for mod variables
                        using (var document = JsonDocument.ParseValue(ref reader))
                        {
                            return document.RootElement.Clone();
                        }
                }
            }

            public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }
                if (value.GetType() == typeof(object))
                {
                    writer.WriteStartObject();
                    writer.WriteEndObject();
                    return;
                }
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }
}
