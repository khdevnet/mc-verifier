using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Core;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Consumer2.ContractTests
{
    [UseApprovalSubdirectory("Approvals")]
    [UseReporter(typeof(VisualStudioReporter))]
    public class Schemas
    {
        private readonly ITestOutputHelper _logger;

        public Schemas(ITestOutputHelper logger)
            => _logger = logger;

        [Fact]
        public void UserCreatedEvent()
        {
            var contractsFolder = Path.Combine(
              FileUtils.GetSolutionDirectory(),
              "ConsumerContracts//Consumer2");

            MessageSchema
                .Create(typeof(UserCreatedEvent))
                .Verify()
                .Save(contractsFolder);
        }
    }
}