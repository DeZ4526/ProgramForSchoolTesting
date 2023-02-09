using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingProgram.Model.Testing;
using TestingProgram.Model.Testing.TestConverters;

namespace ConsoleDebugTest.Model.Testing.TestConverters
{
	static class TestToByteArray
	{
		public static Test GetTest(ITestConverter converter, byte[] bytes)
			=> converter.GetTest(Encoding.UTF32.GetString(bytes));
		
		public static byte[] GetBytes(ITestConverter converter, Test test)
			=> Encoding.UTF32.GetBytes(converter.GetText(test));
	}
}
