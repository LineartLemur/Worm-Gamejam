using System.IO;

    public class FilePathExtensions {
        public static void EnsureDirectoryExitst(string name)
        {
            if(string.IsNullOrEmpty(name)) return;
            if(Directory.Exists(name)) return;
            EnsureDirectoryExitst(Path.GetDirectoryName(name));
            Directory.CreateDirectory(name);
        }
    }