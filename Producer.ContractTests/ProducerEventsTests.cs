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

namespace Events.Producer.ContractTests
{
    public class ProducerEventsTests
    {
        [Fact]
        // Take all schema files from folder and run validation
        public async Task UserCreatedEvent_ContractValidation()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(t => t.FullName.StartsWith("Producer"))
                .ToArray();

            var events = assemblies.SelectMany(assembl => GetTypesBySuffix(assembl, "Event"));

            var schemaName = Utils.GetSchemaFullPath("ConsumerContracts", nameof(UserCreatedEvent));

            var errors = await Validate(schemaName, new UserCreatedEvent { Id = 10, Name = "asd" });
            Assert.False(errors.Any());

            /// get all events schemas from folder
            /// find all types in current assembly by name
            /// validate schemas from folder and apply it for current assembly types
            /// init current assembly type using autofaker/autofixture/json sample generator
        }

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