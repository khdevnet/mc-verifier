using NJsonSchema;
using NJsonSchema.Validation;

namespace Core
{
    public class MessageSchema
    {
        public const string Suffix = ".schema";
        public const string Extension = ".schema.json";
        private readonly string _consumerContractsDirectory;
        private readonly string _typeName;
        private readonly string _schemaJson;
        private readonly JsonSchema _schema;

        private MessageSchema(string fullPath)
        {
            _consumerContractsDirectory = Path.GetDirectoryName(fullPath);
            _typeName = GetTypeNameFromPath(fullPath);
            _schema = JsonSchema.FromFileAsync(fullPath).GetAwaiter().GetResult();
            _schema.AllowAdditionalProperties = true;
            _schemaJson = _schema.ToJson();
        }

        private MessageSchema(Type type)
        {
            _schema = JsonSchema.FromType(type);
            _schema.AllowAdditionalProperties = true;
            _typeName = type.Name;
            _schemaJson = _schema.ToJson();
        }

        public static MessageSchema Create(Type type)
            => new MessageSchema(type);

        public static MessageSchema Create(string fullPath)
           => new MessageSchema(fullPath);

        public string Json
            => _schemaJson;

        public string FullPath
            => GetFullPath(_consumerContractsDirectory);

        private string GetFullPath(string consumerContractsDirectory)
            => Path.Combine(consumerContractsDirectory, GetFileName());

        public string TypeName => _typeName;

        public IReadOnlyCollection<MessageSchemaValidationError> Validate(Type type)
        {
            var sampleJsonData = AutoDataGenerator.GetFromJsonSchema(Create(type).Json);
            var errors = _schema.Validate(sampleJsonData);

            return errors.Select(e => ToError(e)).ToReadOnly();
        }

        private static MessageSchemaValidationError ToError(ValidationError e)
        {
            return new MessageSchemaValidationError(e.Kind.ToString(), e.Property, e.Path, e.LineNumber, e.LinePosition);
        }

        private string GetFileName()
            => $"{_typeName}{Suffix}.json";

        public string Read()
            => File.ReadAllText(FullPath);

        public void Save(string consumerContractsDirectory)
        {
            FileUtils.CreateDirectory(consumerContractsDirectory);

            string schemaFilePath = GetFullPath(consumerContractsDirectory);
            File.WriteAllText(schemaFilePath, Json);
        }

        private string GetTypeNameFromPath(string fullPath)
            => RemoveSuffix(
                Path.GetFileNameWithoutExtension(fullPath),
                Suffix);


        private static string RemoveSuffix(string text, string suffix)
        {
            var suffixIndex = text.IndexOf(suffix);
            var newstr = text.Remove(suffixIndex);
            return newstr;
        }
    }
}