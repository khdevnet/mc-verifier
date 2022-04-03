using FluentAssertions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core
{
    public class MessageTypesApprovals
    {
        private IMessageVerifierTestOutputLogger _logger;

        private MessageTypesApprovals(IMessageVerifierTestOutputLogger logger)
            => _logger = logger;

        public static MessageTypesApprovals Create(IMessageVerifierTestOutputLogger logger)
            => new MessageTypesApprovals(logger);

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
}
