using Core;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Producer.ContractTests
{
    /// get all events schemas from folder
    /// find all types in current assembly by schema name
    /// validate schemas from folder and apply it for current assembly types
    /// init current assembly type using json sample data generator
    public class ConsumersContract
    {
        private const string ConsumerContracts = "ConsumerContracts";
        private readonly ITestOutputHelper _logger;

        public ConsumersContract(ITestOutputHelper logger)
            => _logger = logger;

        [Fact]
        public async Task Verify()
        {
            var messagesSchemaRootFolder = Path.Combine(
                FileUtils.GetSolutionDirectory(),
                ConsumerContracts);

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
}