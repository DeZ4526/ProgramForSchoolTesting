namespace TestingProgram.Model.Testing.TestConverters
{
	public interface ITestConverter
	{
		string GetText(Test test);
		Test GetTest(string test);
	}
}
