using ApprovalTests;
using ApprovalTests.Reporters;
using Events.Core;
using NJsonSchema;
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

            Utils.SaveSchema(schemaJson, "ConsumerContracts//Consumer2", nameof(UserCreatedEvent));
        }
    }
}