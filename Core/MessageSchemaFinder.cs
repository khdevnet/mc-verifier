namespace Core
{
    public static class MessageSchemaFinder
    {
        private const string _pattern = $"*{MessageSchema.Extension}";

        public static async Task<Dictionary<string, IReadOnlyCollection<MessageSchema>>> FindAsync(string consumerContractsDirectory)
        {
            var schemas = await Task.WhenAll(FileUtils
                .GetFilesByPattern(consumerContractsDirectory, _pattern)
                .Select(path => MessageSchema.CreateAsync(path)));

            var groupedByTypeName = GroupByTypeName(schemas);


            return groupedByTypeName;
        }

        private static Dictionary<string, IReadOnlyCollection<MessageSchema>> GroupByTypeName(IReadOnlyCollection<MessageSchema> schemas)
        {
            var groupedPathsBySchemaName = new Dictionary<string, List<MessageSchema>>();
            foreach (var schema in schemas)
            {
                if (!groupedPathsBySchemaName.ContainsKey(schema.TypeName))
                {
                    groupedPathsBySchemaName[schema.TypeName] = new List<MessageSchema>();
                }

                groupedPathsBySchemaName[schema.TypeName].Add(schema);
            }
            return groupedPathsBySchemaName
                .ToDictionary(x => x.Key, x => x.Value.ToReadOnly());
        }
    }
}