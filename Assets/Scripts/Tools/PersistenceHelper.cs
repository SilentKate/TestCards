using System.IO;

public static class PersistenceHelper
{
    public static string GetPath(string path, string fileName)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return Path.Combine(path, fileName);
    }
}