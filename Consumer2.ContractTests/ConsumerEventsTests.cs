using ApprovalTests;
using ApprovalTests.Reporters;
using Core;
using NJsonSchema;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Consumer2.ContractTests
{
    [UseReporter(typeof(VisualStudioReporter))]
    public class Schemas
    {
        private readonly ITestOutputHelper _logger;

        public Schemas(ITestOutputHelper logger)
            => _logger = logger;

        [Fact]
        public void UserCreatedEvent()
        {
            var schemaJson = JsonSchema.FromType<UserCreatedEvent>().ToJson();

            Approvals.VerifyJson(schemaJson);

            var contractsFolder = Path.Combine(
               FileUtils.GetSolutionDirectory(),
               "ConsumerContracts//Consumer2");

            new FileSchema(contractsFolder, typeof(UserCreatedEvent).Name).Save(schemaJson);
        }
    }
}