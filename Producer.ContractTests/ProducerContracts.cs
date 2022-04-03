using McVerifier;
using McVerifier.Abstractions.Loggers;
using McVerifier.Utils;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Producer.ContractTests;

public class ProducerContracts
{
    private const string ConsumerContracts = "ConsumerContracts";
    private readonly ITestOutputHelper _logger;

    public ProducerContracts(ITestOutputHelper logger)
        => _logger = logger;

    [Fact]
    public async Task Verify()
    {
        // Consumers message contracts root folder
        var messagesSchemaRootFolder = Path.Combine(
            FileUtils.GetSolutionDirectory(),
            ConsumerContracts);

        // verify producer message types using consumers contracts
        await MessageTypesApprovals
            .Create(new SchemaVerifierTestOutputLogger(_logger))
            .Verify(messagesSchemaRootFolder, "^Producer.*Event$", "^Producer.*Command$");
    }
}

public class SchemaVerifierTestOutputLogger : IMessageVerifierTestOutputLogger
{
    private readonly ITestOutputHelper _logger;

    public SchemaVerifierTestOutputLogger(ITestOutputHelper logger)
        => _logger = logger;

    public void Log(string message) => _logger.WriteLine(message);
}
