using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using McVerifier;
using McVerifier.Utils;
using System.IO;
using Xunit;

namespace Consumer1.ContractTests;

[UseApprovalSubdirectory("Approvals")]
[UseReporter(typeof(VisualStudioReporter))]
public class Schemas
{
    [Fact]
    public void UserCreatedEvent()
    {
        var contractsFolder = Path.Combine(
            FileUtils.GetSolutionDirectory(),
            "ConsumerContracts//Consumer1");

        MessageSchema
            .Create(typeof(UserCreatedEvent))
            .Verify()
            .Save(contractsFolder);
    }
}
