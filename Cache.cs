static class DataCache
{
    static string DataDirectory = @"C:\Users\gredw\VsCode\AdventOfCode\Data";

    public static string Get(string store, string key, Func<string> getter)
    {
        var file = Path.Combine(DataDirectory, store, key);
        if (File.Exists(file)) 
        {
            return File.ReadAllText(file);
        }
        else
        {
            var data = getter();
            Directory.CreateDirectory(Path.Combine(DataDirectory, store));
            File.WriteAllText(file, data);
            return data;
        }
    }
}