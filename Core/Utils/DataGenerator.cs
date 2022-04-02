using Json.Schema;
using Json.Schema.DataGeneration;

namespace Producer.ContractTests
{
    public static class AutoDataGenerator
    {
        public static string Get(string fileName)
        {
            var schema = JsonSchema.FromFile(fileName);
            var generationResult = schema.GenerateData();
            var sampleData = generationResult.Result;
            return sampleData.ToString();
        }
    }
}
