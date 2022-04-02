using ApprovalTests;
using ApprovalTests.Reporters;
using Core;
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
            var schema = MessageSchema
                 .Create(typeof(UserCreatedEvent));

            Approvals.VerifyJson(schema.Json);

            var contractsFolder = Path.Combine(
               FileUtils.GetSolutionDirectory(),
               "ConsumerContracts//Consumer2");

            schema.Save(contractsFolder);
        }
    }
}