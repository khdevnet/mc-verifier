using Core;
using NJsonSchema;
using NJsonSchema.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using System;
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
        private readonly ITestOutputHelper _logger;

        public ProducerEventsTests(ITestOutputHelper logger)
            => _logger = logger;

        [Fact]
        public async Task MessagesValidation()
        {
            var eventsType = ReflectionUtils.FindTypes("^Producer.*Event$", "^Producer.*Command$");

            var consumerContractsDirectory = Path.Combine(
                FileUtils.GetSolutionDirectory(),
                ConsumerContracts);

            var groupedPathsByTypeName = await MessageSchemaFinder.FindAsync(consumerContractsDirectory);

            foreach (var eventType in eventsType)
            {
                AssertSchema(eventType, groupedPathsByTypeName);
            }
        }

        private void AssertSchema(Type eventType, Dictionary<string, IReadOnlyCollection<MessageSchema>> groupedPathsByTypeName)
        {
            var schemasByTypeName = groupedPathsByTypeName.GetValueOrDefault(eventType.Name) ?? new List<MessageSchema>();

            foreach (var schema in schemasByTypeName)
            {
                var errors = schema.Validate(eventType);

                if (errors.Any())
                    _logger.WriteLine(SerializeErrors(errors));

                Assert.False(errors.Any(),
                    $"Validation failed: {Environment.NewLine}" +
                    $"Event type: {eventType.FullName} {Environment.NewLine}" +
                    $"Schema Path: {schema.FullPath}");
            }
        }

        private static string SerializeErrors(IReadOnlyCollection<MessageSchemaValidationError> errors)
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
    }
}