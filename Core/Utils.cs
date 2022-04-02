namespace Events.Core
{
    public static class Utils
    {
        public static string GetSchemaName(string consumerContractsDirectory, string typeName)
        {
            return Path.Combine(consumerContractsDirectory, $"{typeName}.schema.json");
        }

        public static void CreateDirectory(string consumerContractsDirectory)
        {
            if (Directory.Exists(consumerContractsDirectory))
                Directory.Delete(consumerContractsDirectory, true);

            Directory.CreateDirectory(consumerContractsDirectory);
        }

        public static string GetSolutionDirectory()
        {
            var path = new List<string> { Directory.GetCurrentDirectory() };
            path.AddRange(GetPathDeep(4));
            return Path.GetFullPath(Path.Combine(path.ToArray()));
        }

        public static IEnumerable<string> GetPathDeep(int deep)
        {
            return Enumerable.Range(1, deep).Select(c => @"..\");
        }

        public static void SaveSchema(string schemaJson, string folderName, string modelName)
        {
            var solutionDirectory = Utils.GetSolutionDirectory();
            var consumerContractsDirectory = Path.Combine(solutionDirectory, folderName);

            Utils.CreateDirectory(consumerContractsDirectory);

            string schemaName = Utils.GetSchemaName(consumerContractsDirectory, modelName);
            File.WriteAllText(schemaName, schemaJson);
        }

        public static string GetSchemaFullPath(string folderName, string modelName)
        {
            var solutionDirectory = Utils.GetSolutionDirectory();
            var consumerContractsDirectory = Path.Combine(solutionDirectory, folderName);

            string schemaName = Utils.GetSchemaName(consumerContractsDirectory, modelName);
            return schemaName;
        }
    }
}