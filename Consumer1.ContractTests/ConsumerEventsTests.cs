using ApprovalTests;
using ApprovalTests.Reporters;
using Core;
using NJsonSchema;
using System.IO;
using Xunit;

namespace Consumer1.ContractTests
{
    [UseReporter(typeof(VisualStudioReporter))]
    public class Schemas
    {
        [Fact]
        public void UserCreatedEvent()
        {
            var schema = MessageSchema
                 .Create(typeof(UserCreatedEvent));

            Approvals.VerifyJson(schema.Json);

            var contractsFolder = Path.Combine(
                FileUtils.GetSolutionDirectory(),
                "ConsumerContracts//Consumer1");

            schema.Save(contractsFolder);
        }
    }
}