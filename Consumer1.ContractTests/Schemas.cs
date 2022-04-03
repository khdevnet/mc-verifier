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
        // Folder where generated json schema contract file will be save.
        var contractsFolder = Path.Combine(
            FileUtils.GetSolutionDirectory(),
            "ConsumerContracts//Consumer1");

        MessageSchema
            .Create(typeof(UserCreatedEvent))
            .Verify() // Verify message json schema using ApprovalTests
            .Save(contractsFolder); // if verification successful then save schema
    }
}
