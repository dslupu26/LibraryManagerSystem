using System.Reflection;

namespace Repositories
{
    public static class SQLInstructionProvider
    {
        public static List<string> GetQueries(String fileName)
        {
            List<string> queries;
            queries = new List<string>();
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(SQLInstructionProvider)).Location), fileName);

            Console.WriteLine($"Looking for SQL file at: {filePath}");

            var fileContent = File.ReadAllText(filePath);
            var splitQueries = fileContent.Split(new[] { "GO", "go", "Go" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var query in splitQueries)
            {
                var trimmedQuery = query.Trim(); //removes leading and trailing whitespaces 

                if (!string.IsNullOrWhiteSpace(trimmedQuery))
                {
                    queries.Add(trimmedQuery);
                }
            }

            return queries;
        }

    }
}   
