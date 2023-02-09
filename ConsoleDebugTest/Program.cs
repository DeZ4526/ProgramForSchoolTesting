using DeZ.Settings;
using TestingProgram.Model;
using TestingProgram.Model.ClientServer.Client;
using TestingProgram.Model.Testing;

namespace ConsoleDebugTest
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.Write("S - Server\n" +
				"C - Client");
			if (Console.ReadLine() == "S")
			{
				Console.WriteLine("Server\nEnter port:");
				Controller.StartServer(int.Parse(Console.ReadLine() ?? "4526"));
				Question[] questions = new Question[3];
				Question.Answer[] answer = new Question.Answer[2];
				answer[0] = new Question.Answer("1", true);
				answer[1] = new Question.Answer("2", false);

				Question.Answer[] answer1 = new Question.Answer[2];
				answer1[0] = new Question.Answer("1_1", true);
				answer1[1] = new Question.Answer("1_2", false);

				Question.Answer[] answer2 = new Question.Answer[2];
				answer2[0] = new Question.Answer("2_1", true);
				answer2[1] = new Question.Answer("2_2", false);
				questions[0] = new Question("1", answer);
				questions[1] = new Question("1", answer1);
				questions[2] = new Question("1", answer2);
				Test test = new Test("Test", "test 1", questions);
				Console.WriteLine("Enter to any key to start testing");
				Console.ReadLine();
				Controller.StartTestingToServer += Controller_StartTestingToServer;
				Controller.GetAnswerToServer += Controller_GetAnswerToServer;
				Controller.StartTesting(test);
			}
			else
			{
				Console.WriteLine("Client\nEnter ip and port");
				Controller.Connect(Console.ReadLine() ?? "127.0.0.1", int.Parse(Console.ReadLine() ?? "4526"));
				Controller.StartTestingToClient += Controller_StartTestingToClient;
			}
		}

		private static void Controller_GetAnswerToServer(AnswerForTest answer)
		{
			
			Console.WriteLine("{Login}|{Ip}|{Start}|{End}|{Good}|{Bad}|{Unresolved}|{All}\n" + answer.ToString());
		}

		private static void Controller_StartTestingToServer(Test test)
		{
			ConsoleWTest(test);
		}

		private static void Controller_StartTestingToClient(Test test)
		{
			//ConsoleWTest(test);
			Console.WriteLine(test.Text);
			
			AnswerForTest answer = new AnswerForTest("Test", "Test", DateTime.Now, test.Questions.Length);
			for (int i = 0; i < test.Questions.Length; i++)
			{
				Console.WriteLine("-Вопрос :" + test.Questions[i].Text);
				for (int j = 0; j < test.Questions[i].AnswerMas.Length; j++)
				{
					Console.WriteLine("--[" + j + "]Answer:" + test.Questions[i].AnswerMas[j].Text);
				}
				Console.Write("--Введите номер ответа : ");
				int ans = int.Parse(Console.ReadLine() ?? "0");
				answer.AddAnswer(test.Questions[i].AnswerMas[ans].IsTrue ? AnswerForTest.AnswerType.Good : AnswerForTest.AnswerType.Bad);
			}
			answer.End = DateTime.Now;
			Console.WriteLine(answer.ToString());
			Controller.AddAnswer(answer);
		}
		private static void ConsoleWTest(Test test)
		{
			Console.WriteLine(test.Text);
			for (int i = 0; i < test.Questions.Length; i++)
			{
				Console.WriteLine("-Text:" + test.Questions[i].Text);
				Console.WriteLine("-IMGUrl:" + test.Questions[i].IMGUrl);
				for (int j = 0; j < test.Questions[i].AnswerMas.Length; j++)
				{
					Console.WriteLine("--Answer:" + test.Questions[i].AnswerMas[j].Text + " | " + test.Questions[i].AnswerMas[j].IsTrue);
				}
			}
		}
	}
}