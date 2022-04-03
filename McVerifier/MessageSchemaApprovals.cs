using ApprovalTests;

namespace McVerifier;

public static class MessageSchemaApprovals
{
    public static MessageSchema Verify(this MessageSchema current)
    {
        Approvals.VerifyJson(current.Json);
        return current;
    }
}
