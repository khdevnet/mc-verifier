using FluentAssertions;
using McVerifier.Abstractions.Loggers;
using McVerifier.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace McVerifier;

/// get all consumer message contracts schemas from the folder
/// find all producer message types in current assembly by regex pattern
/// init producer meesage type using sample data generator
/// and verify using consumers message contracts 
public class MessageTypesApprovals
{
    private readonly IMessageVerifierTestOutputLogger _logger;

    private MessageTypesApprovals(IMessageVerifierTestOutputLogger logger)
        => _logger = logger;

    public static MessageTypesApprovals Create(IMessageVerifierTestOutputLogger logger)
        => new(logger);

    public async Task Verify(string consumerMessagesRootFolder, params string[] findTypeRegex)
    {
        var messagesTypes = ReflectionUtils.FindTypes(findTypeRegex).ToList();
        var groupedPathsByTypeName = await MessageSchemaFinder.FindAsync(consumerMessagesRootFolder);

        messagesTypes
            .ForEach(type => VerifyMessageType(type, groupedPathsByTypeName));
    }

    private void VerifyMessageType(Type eventType, Dictionary<string, IReadOnlyCollection<MessageSchema>> groupedPathsByTypeName)
    {
        var schemasByTypeName = groupedPathsByTypeName.GetValueOrDefault(eventType.Name) ?? new List<MessageSchema>();

        foreach (var schema in schemasByTypeName)
        {
            var errors = schema.Validate(eventType);

            if (errors.Any())
                _logger.Log(SerializeErrors(errors));

            errors.Any().Should().BeFalse(
                $"Validation failed: {Environment.NewLine}" +
                $"Event type: {eventType.FullName} {Environment.NewLine}" +
                $"Schema Path: {schema.FullPath}");
        }
    }

    private string SerializeErrors(IReadOnlyCollection<MessageSchemaValidationError> errors)
        => JsonSerializer.Serialize(
            errors,
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
            });
}
