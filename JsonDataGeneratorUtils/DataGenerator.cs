using Json.Schema;
using Json.Schema.DataGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.ContractTests
{
    public static class DataGenerator
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
