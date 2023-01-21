using System.Collections.Generic;

namespace TestingProgram.Model.Testing.TestConverters
{
	internal class STSTConvertor : ITestConverter
	{
		public Test GetTest(string test)
		{
			Test obj = new Test();
			string[] LineTest = test.Split('\n');
			for (int i = 0; i < LineTest.Length; i++) LineTest[i] = LineTest[i].TrimStart('	');
			if (LineTest[0].Substring(0, "stst 0.3".Length) == LineTest[LineTest.Length - 1].Substring(0, "stst 0.3".Length) && LineTest[0].Substring(0, "stst 0.3".Length) == "stst 0.3")
			{
				//Text = test;
				obj.Title = LineTest[1];
				List<Question> Questions = new List<Question>();
				List<Question.Answer> Answers = new List<Question.Answer>();
				Question question = null;
				for (int i = 2; i < LineTest.Length - 1; i++)
				{
					if (!LineTest[i].StartsWith("//"))
					{
						if (LineTest[i].StartsWith(";"))
						{
							if (question != null)
							{
								question.AnswerMas = Answers.ToArray();
								Answers.Clear();
								Questions.Add(question);
							}

							question = new Question
							{
								Text = LineTest[i].Substring(1)
							};
						}
						else if (question.Text != "")
						{
							if (!LineTest[i].StartsWith(":"))
								Answers.Add(new Question.Answer(LineTest[i].StartsWith("*") ? LineTest[i].Substring(1) : LineTest[i].TrimStart(' '), LineTest[i].StartsWith("*")));
							else
								question.IMGUrl.Add(LineTest[i].Substring(1));
						}
					}
				}
				if (question.Text != "")
				{
					question.AnswerMas = Answers.ToArray();
					Questions.Add(question);
				}
				obj.Questions = Questions.ToArray();
			}
			return obj;
		}

		public string GetText(Test test)
		{
			bool tabs = false;
			string strTest = "stst 0.3\n";
			strTest += (tabs ? "	" : "") + test.Title + "\n";
			for (int i = 0; i < test.Questions.Length; i++)
			{
				strTest += (tabs ? "	" : "") + ";" + test.Questions[i].Text + "\n";
				for (int a = 0; a < test.Questions[i].IMGUrl.Count; a++)
					strTest += (tabs ? "		" : "") + ":" + test.Questions[i].IMGUrl[a] + "\n";
				for (int a = 0; a < test.Questions[i].AnswerMas.Length; a++)
				{
					strTest += (tabs ? "		" : "") + (test.Questions[i].AnswerMas[a].IsTrue ? "*" : "");
					if (!test.Questions[i].AnswerMas[a].IsTrue && (test.Questions[i].AnswerMas[a].Text.StartsWith("*") || test.Questions[i].AnswerMas[a].Text.StartsWith(";"))) strTest += " ";
					strTest += test.Questions[i].AnswerMas[a].Text + "\n";
				}
			}
			strTest += "stst 0.3";
			return strTest;
		}
	}
}
