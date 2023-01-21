using System.Collections.Generic;
using System.Windows;

namespace TestingProgram.Model.Testing
{
	public class Question
	{
		/// <summary>
		/// Текст вопроса
		/// </summary>
		public string Text { get; set; }
		public List<string> IMGUrl { get; set; } = new List<string>();
		public bool Enabled { get; set; } = true;
		/// <summary>
		/// Ответы
		/// </summary>
		public Answer[] AnswerMas { get; set; }

		public Question() { }
		public Question(string Text, Answer[] AnswerMas)
		{
			this.Text = Text;
			this.AnswerMas = AnswerMas;
		}

		public class Answer
		{
			/// <summary>
			/// Текст ответа
			/// </summary>
			public string Text { get; set; }
			/// <summary>
			/// true - правильный ответ
			/// </summary>
			public bool IsTrue { get; set; }

			public Answer() { }
			public Answer(string Text, bool isTrue)
			{
				this.Text= Text;
				this.IsTrue = isTrue;
			}
		}
	}
}