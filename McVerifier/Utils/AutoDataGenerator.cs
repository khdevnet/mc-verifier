using Json.Schema;
using Json.Schema.DataGeneration;

namespace McVerifier.Utils;

public static class AutoDataGenerator
{
    public static string GetFromJsonSchema(string schemaJson)
    {
        var schema = JsonSchema.FromText(schemaJson);
        var generationResult = schema.GenerateData();
        var sampleData = generationResult.Result;
        return sampleData.ToString();
    }
}
