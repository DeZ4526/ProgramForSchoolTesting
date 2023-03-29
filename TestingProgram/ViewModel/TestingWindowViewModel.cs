using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestingProgram.Model;
using TestingProgram.Model.Testing;

namespace TestingProgram.ViewModel
{
	internal class TestingWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

		public AnswerForTest answer { get; set; } = new AnswerForTest("test user","", DateTime.Now, Controller.SelectedTest.Questions.Length);
		public Test SelectedTest { get => Controller.SelectedTest; }

		public int SelectedAnswer { get; set; }

		private int SelectedQuestionId = 0;
		public Question SelectedQuestion { get; set; }

		public TestingWindowViewModel()
		{
			SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
		}
		public ICommand NextSlide
		{
			get => new DelegateCommand((obj) => {
				answer.AddAnswer(SelectedTest.Questions[SelectedQuestionId].AnswerMas[SelectedAnswer].IsTrue ? AnswerForTest.AnswerType.Good : AnswerForTest.AnswerType.Bad);
				SelectedQuestionId++;
				SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
				if(SelectedQuestionId >= SelectedTest.Questions.Length)
				{
					answer.End = DateTime.Now;
					Controller.AddAnswer(answer);
				}
			});
		}

	}
}
