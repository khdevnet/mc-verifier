namespace Core
{
    public class FileSchema
    {
        private readonly string _consumerContractsDirectory;
        private readonly string _typeName;

        public FileSchema(string consumerContractsDirectory, string typeName)
        {
            _consumerContractsDirectory = consumerContractsDirectory;
            _typeName = typeName;
        }

        public string GetFullPath()
            => Path.Combine(_consumerContractsDirectory, GetName());

        private string GetName()
            => $"{_typeName}.schema.json";

        public void Save(string schemaJson)
        {
            FileUtils.CreateDirectory(_consumerContractsDirectory);

            string schemaFilePath = GetFullPath();
            File.WriteAllText(schemaFilePath, schemaJson);
        }

        //public static string GetSchemaFullPath()
        //{
        //    var solutionDirectory = FileSystemUtils.GetSolutionDirectory();
        //    var consumerContractsDirectory = Path.Combine(solutionDirectory, folderName);

        //    string schemaName = GetFilePath();
        //    return schemaName;
        //}
    }

    //public static class FileSchema
    //{
    //    public static string GetName(string consumerContractsDirectory, string typeName)
    //    {
    //        return Path.Combine(consumerContractsDirectory, $"{typeName}.schema.json");
    //    }

    //    public static void Save(string schemaJson, string folderName, string modelName)
    //    {
    //        var solutionDirectory = FileSystemUtils.GetSolutionDirectory();
    //        var consumerContractsDirectory = Path.Combine(solutionDirectory, folderName);

    //        FileSystemUtils.CreateDirectory(consumerContractsDirectory);

    //        string schemaName = GetName(consumerContractsDirectory, modelName);
    //        File.WriteAllText(schemaName, schemaJson);
    //    }

    //    public static string GetSchemaFullPath(string folderName, string modelName)
    //    {
    //        var solutionDirectory = FileSystemUtils.GetSolutionDirectory();
    //        var consumerContractsDirectory = Path.Combine(solutionDirectory, folderName);

    //        string schemaName = GetName(consumerContractsDirectory, modelName);
    //        return schemaName;
    //    }
    //}
}