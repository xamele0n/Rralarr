using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Rralarr.Common;

public static class ReflectionExtensions
{
    public static Type FindTypeByName(this Type typeAssembly, string name)
    {
        return typeAssembly.Assembly.GetExportedTypes().SingleOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
    
    static JsonSerializer _serializer = JsonSerializer.Create(new JsonSerializerSettings
    {
        Converters =
        {
            new ProviderDefinitionConverter()
        }
    });

    public static TR DynamicCast<T, TR>(T obj)
    {
        using var ms = new MemoryStream();
        using var w = new StreamWriter(ms, leaveOpen: true);
        _serializer.Serialize(w, obj);
        w.Flush();
        ms.Position = 0;
       
        return _serializer.Deserialize<TR>(new JsonTextReader(new StreamReader(ms)));
    }
}

public class ProviderDefinitionConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, JObject.FromObject(value));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = serializer.Deserialize<JObject>(reader);
        if (jObject == null)
        {
            return null;
        }
        var discriminator = jObject.GetValue("ConfigContract", StringComparison.OrdinalIgnoreCase)?.Value<string>();
        var subtype = objectType.FindTypeByName(discriminator);
        var o = existingValue ?? Activator.CreateInstance(objectType);
        var contract = (JsonObjectContract) serializer.ContractResolver.ResolveContract(objectType);
        var settingsProperty = contract.Properties.GetClosestMatchProperty("Settings");
        var settings = Activator.CreateInstance(subtype);
        settingsProperty.ValueProvider.SetValue(o, settings);
        serializer.Populate(jObject.CreateReader(), o);
        return o;
    }
    
    

    public override bool CanConvert(Type objectType)
    {
        return objectType.Name.EndsWith("Definition");
    }
}