using System;
using System.Collections.Generic;
using static TestingProgram.Model.Testing.Question;

namespace TestingProgram.Model.Testing
{
	static class TestingController
	{
		public static class Client
		{
			public static AnswerForTest Answer { get; private set; }
			public static Test Test { get; private set; }
			public static bool Open { get; private set; } = false;
			public static void EndTest()
			{
				Open = false;
				if (Answer != null)
				{
					Answer.End = DateTime.Now;
					OnTestingEnd?.Invoke(Test);
				}
				
			}
			public static void StartTest(Test test, string login, string ip)
			{
				Open = true;
				Answer = new AnswerForTest(login, ip, DateTime.Now, test.Questions.Length);
				OnTestingStart?.Invoke(Test);
			}
			public delegate void onTestingStart(Test test);
			public static event onTestingStart OnTestingStart;
			public delegate void onTestingEnd(Test test);
			public static event onTestingEnd OnTestingEnd;
		}
		public static class Server
		{
			public static List<AnswerForTest> Answers { get; private set; } = new List<AnswerForTest>();

			public static void AddAnswer(AnswerForTest answer)
			{
				Answers.Add(answer);
			}

		}
	}
}