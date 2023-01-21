using System.IO;

namespace TestingProgram.Model.Testing.TestConverters
{
	static class TestToFile
	{
		public static Test OpenTest(ITestConverter converter, string path)
		=>	converter != null && File.Exists(path) ? 
			converter.GetTest(File.ReadAllText(path)) : 
			new Test();

		public static void SaveTest(ITestConverter converter, Test test, string path)
		{
			if (converter != null)
				File.WriteAllText(path, converter.GetText(test));
		}
	}
}
