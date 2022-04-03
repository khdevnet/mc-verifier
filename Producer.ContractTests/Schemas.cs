using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using McVerifier;
using Xunit;

namespace Producer.ContractTests;

[UseApprovalSubdirectory("Approvals")]
[UseReporter(typeof(VisualStudioReporter))]
public class Schemas
{
    [Fact]
    public void UserCreatedEvent()
        => MessageSchema
            .Create(typeof(UserCreatedEvent))
            .Verify();

    [Fact]
    public void CreateUserCommand()
        => MessageSchema
            .Create(typeof(CreateUserCommand))
            .Verify();
}
