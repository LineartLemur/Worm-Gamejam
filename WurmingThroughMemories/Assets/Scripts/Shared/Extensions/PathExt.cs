using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

public static class PathExt
{
	struct File
	{
		public string prefix;
		public string path;
	}

	public const char separator = '/';

	public static string[] Split(string path)
	{
		return Regex.Split(Sanitize(path), ""+separator);
	}

	public static string Combine(IEnumerable<string> pathPieces)
	{
		return string.Join(""+separator, pathPieces);
	}

	public static void EnsureDirectory(string path)
	{
		Directory.CreateDirectory(path);
	}

	public static string Sanitize(string path)
	{
		path = path.Replace('\\', separator);
		path = path.Replace('/', separator);
		return path;
	}

	private static string[] SplitPath(string path)
	{
		return Sanitize(path).Split(separator);
	}

	private static string SanitizeDirectory(string dir)
	{
		dir = Sanitize(dir);
		var split = SplitPath(dir);
		List<string> finalParts = new List<string>();
		for (int i = 0; i < split.Length; ++i)
		{
			var part = split[i];
			if (part.Length == 0 || part == ".") continue;
			if (part == ".." && finalParts.Count > 0)
			{
				// pop the first non-.. final part
				int idx = finalParts.Count - 1;
				while (idx >= 0 && finalParts[idx] == "..") --idx;
				if (idx >= 0)
				{
					finalParts.RemoveAt(finalParts.Count-1);
				}
				else
				{
					finalParts.Add(part);
				}
				continue;
			}
			finalParts.Add(part);
		}

		string ss = "";
		foreach (string part in finalParts)
		{
			if (ss.Length > 0) ss += Path.DirectorySeparatorChar;
			ss += part;
		}
		return ss;
	}
	public static string GetRelativePath(string rootPath, string subPath)
	{
		rootPath = SanitizeDirectory(rootPath);
		subPath = SanitizeDirectory(subPath);
		if (!subPath.Contains(rootPath)) throw new Exception($"{subPath} is no sub path of root path {rootPath}.");
		subPath = subPath.Replace(rootPath, "");
		if (subPath.StartsWith(""+ separator)) subPath = subPath.Substring(1);
		return subPath;
	}

	public static IEnumerable<string> GetFilesRecursively(string path)
	{
		foreach (string file in Directory.GetFiles(path))
		{
			yield return file;
		}
		foreach (string dir in Directory.GetDirectories(path))
		{
			foreach (string file in GetFilesRecursively(dir))
			{
				yield return file;
			}
		}
	}
}
