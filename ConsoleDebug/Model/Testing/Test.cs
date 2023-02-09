using TestingProgram.Model.Testing.TestConverters;

namespace TestingProgram.Model.Testing
{
	public partial class Test
	{
		/// <summary>
		/// Заголовок теста
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Текст теста
		/// </summary>
		public string Text { get; set; }
		/// <summary>
		/// Вопросы
		/// </summary>
		public Question[] Questions { get; set; }

		public Test() { }
		public Test(ITestConverter converter, string test)
		{
			Test t = converter.GetTest(test);
			Title = t.Title;
			Text = t.Text;
			Questions = t.Questions;
		}
		/// <summary>
		/// Тест
		/// </summary>
		/// <param name="Title">Заголовок текста</param>
		/// <param name="Text">Текст теста</param>
		/// <param name="Questions">Вопросы теста</param>
		public Test(string Title, string Text, Question[] Questions)
		{
			this.Title = Title;
			this.Text = Text;
			this.Questions = Questions;
		}
	}
}