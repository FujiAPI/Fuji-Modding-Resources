using System.Text;
namespace MergeFGDTool
{
	public class Program
	{
		private static string fileName = "..\\Celeste64Merged.fgd";
		private static string searchPath = AppContext.BaseDirectory + "..\\";

		private const string fileNameKey = "filename=";
		private const string searchPathKey = "searchpath=";
		public static void Main(string[] args)
		{
			string? newFileName = args.FirstOrDefault(arg => arg.StartsWith(fileNameKey));
			if(newFileName != null)
			{
				newFileName = newFileName.Substring(fileNameKey.Length);
				if (!string.IsNullOrEmpty(newFileName))
				{
					if (newFileName.EndsWith(".fgd"))
					{
						fileName = newFileName;
					}
					else
					{
						fileName = newFileName + ".fgd";
					}
				}
			}

			string? newSearchPath = args.FirstOrDefault(arg => arg.StartsWith(searchPathKey));
			if (newSearchPath != null)
			{
				newSearchPath = newSearchPath.Substring(searchPathKey.Length);
				if (!string.IsNullOrEmpty(newSearchPath))
				{
					if(Directory.Exists(newSearchPath))
					{
						searchPath = newSearchPath;
					}
					else if( Directory.Exists(AppContext.BaseDirectory + newSearchPath))
					{
						searchPath = AppContext.BaseDirectory + newSearchPath;
					}
					else
					{
						Console.WriteLine($"Provided search path {searchPath} was not valid. Using default path instead");
					}
				}
			}

			string[] files = Directory.GetFiles(searchPath, "*.fgd", SearchOption.AllDirectories);

			StringBuilder sb = new StringBuilder();

			foreach (string file in files)
			{
				if (!file.Contains(fileName))
				{
					sb.AppendLine($"@include \"{Path.GetRelativePath(searchPath, file)}\"");
				}
			}

			Console.WriteLine(sb.ToString());
			File.WriteAllText(fileName, sb.ToString());
		}
	}
}
