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

        private MessageSchema(string fullPath, JsonSchema schema)
        {
            _consumerContractsDirectory = Path.GetDirectoryName(fullPath);
            _typeName = GetTypeNameFromPath(fullPath);
            _schema = schema;
            _schema.AllowAdditionalProperties = true;
            _schemaJson = _schema.ToJson();
        }

        private MessageSchema(Type type)
        {
            var schema = JsonSchema.FromType(type);
            schema.AllowAdditionalProperties = true;
            _schema = schema;
            _schemaJson = schema.ToJson();
            _typeName = type.Name;
        }

        public static MessageSchema Create(Type type)
            => new MessageSchema(type);

        public static async Task<MessageSchema> CreateAsync(string fullPath)
        {
            var schema = await JsonSchema.FromFileAsync(fullPath);
            return new MessageSchema(fullPath, schema);
        }

        public string Json
            => _schemaJson;

        public string FullPath
            => GetFullPath(_consumerContractsDirectory);

        public string TypeName => _typeName;

        public string CreateSample()
           => AutoDataGenerator.GetFromJsonSchema(Json);

        public IReadOnlyCollection<MessageSchemaValidationError> Validate(Type type)
        {
            var sampleJsonData = Create(type).CreateSample();
            return _schema.Validate(sampleJsonData)
                .Select(e => ToError(e)).ToReadOnly();
        }

        private static MessageSchemaValidationError ToError(ValidationError e)
            => new MessageSchemaValidationError(e.Kind.ToString(), e.Property, e.Path, e.LineNumber, e.LinePosition);

        private string GetFullPath(string consumerContractsDirectory)
            => Path.Combine(consumerContractsDirectory, GetFileName());

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