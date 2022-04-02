using Events.Core;
using NJsonSchema;
using NJsonSchema.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;
using System;
using Producer.ContractTests;
using Xunit.Abstractions;
using System.Text.Json.Serialization;

namespace Producer.ContractTests
{
    /// get all events schemas from folder
    /// find all types in current assembly by schema name
    /// validate schemas from folder and apply it for current assembly types
    /// init current assembly type using json sample data generator
    public class ProducerEventsTests
    {
        private const string ConsumerContracts = "ConsumerContracts";
        private const string SchemaSuffix = ".schema";
        private readonly ITestOutputHelper _logger;

        public ProducerEventsTests(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public async Task UserCreatedEvent_ContractValidation()
        {
            Type[] eventsType = GetClassTypes("Producer", "Event");

            var consumerContractsDirectory = Path.Combine(Utils.GetSolutionDirectory(), ConsumerContracts);

            var schemasDict = Utils.GetFilesByPattern(consumerContractsDirectory, $"*{SchemaSuffix}.json");

            var groupedPathsBySchemaName = GroupByValue(schemasDict);

            var groupedPathsByTypeName = groupedPathsBySchemaName.ToDictionary(x => RemoveSuffix(x.Key, SchemaSuffix), x => x.Value);

            foreach (var eventType in eventsType)
            {
                await ValidateEvent(eventType, groupedPathsByTypeName);
            }
        }

        private static string RemoveSuffix(string text, string suffix)
        {
            var suffixIndex = text.IndexOf(suffix);

            var newstr = text.Remove(suffixIndex);
            return newstr;

        }

        private static Dictionary<string, List<string>> GroupByValue(Dictionary<string, string> schemasDict)
        {
            var groupedPathsBySchemaName = new Dictionary<string, List<string>>();
            foreach (var keyValue in schemasDict)
            {
                if (!groupedPathsBySchemaName.ContainsKey(keyValue.Value))
                {
                    groupedPathsBySchemaName[keyValue.Value] = new List<string>();
                }

                groupedPathsBySchemaName[keyValue.Value].Add(keyValue.Key);
            }
            return groupedPathsBySchemaName;
        }

        private Type[] GetClassTypes(string namespacePrefix, string typeSuffix)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(t => t.FullName.StartsWith(namespacePrefix))
                .ToArray();

            var eventsType = assemblies.SelectMany(assembl => GetTypesBySuffix(assembl, typeSuffix)).ToArray();
            return eventsType;
        }

        private async Task ValidateEvent(Type eventType, Dictionary<string, List<string>> groupedPathsByTypeName)
        {
            var schemasByTypeName = groupedPathsByTypeName.GetValueOrDefault(eventType.Name) ?? new List<string>();

            foreach (var schemaFilePath in schemasByTypeName)
            {
                var sampleData = DataGenerator.Get(schemaFilePath);

                var sampleDataObj = JsonSerializer.Deserialize(sampleData, eventType);

                var errors = await Validate(schemaFilePath, sampleDataObj);

                if (errors.Any())
                    _logger.WriteLine(SerializeErrors(errors));

                Assert.False(errors.Any(), $"Validation failed for: {eventType.FullName}");
            }
        }

        private static string SerializeErrors(ICollection<ValidationError> errors)
            => JsonSerializer.Serialize(
                errors,
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                });

        private static async Task<ICollection<ValidationError>> Validate<TModel>(string schemaName, TModel model)
        {
            var schema = await JsonSchema.FromFileAsync(schemaName);
            schema.AllowAdditionalProperties = true;
            string jsonString = JsonSerializer.Serialize(model);

            var errors = schema.Validate(jsonString);
            return errors;
        }

        private Type[] GetTypesBySuffix(Assembly assembly, string suffix)
        {
            return
              assembly.GetTypes()
                      .Where(t => t.FullName.EndsWith(suffix))
                      .ToArray();
        }
    }
}