using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestingProgram.Model;
using TestingProgram.Model.Testing;
using static TestingProgram.Model.Testing.Question;

namespace TestingProgram.ViewModel
{
	internal class TestingWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

		private AnswerForTest answer;
		public AnswerForTest Answer { get => answer; set
			{
				answer = value;
				OnPropertyChanged();
			}
		}
		public Test SelectedTest { set { Controller.SelectedTest = value; OnPropertyChanged(); } get => Controller.SelectedTest; }

		private Answer selectedAnswer;

		public Answer SelectedAnswer
		{
			get => selectedAnswer; set
			{
				selectedAnswer = value;
				OnPropertyChanged();
			}
		}


		private int SelectedQuestionId = 0;
		private Question selectedQuestion;
		public Question SelectedQuestion { get => selectedQuestion; set
			{
				selectedQuestion = value;
				OnPropertyChanged();
			}
		}

		public TestingWindowViewModel()
		{
			Controller.StartTestingToClient += Controller_StartTestingToClient;
		}

		private void Controller_StartTestingToClient(Test test)
		{
			answer = new AnswerForTest("test user", "", DateTime.Now, Controller.SelectedTest.Questions.Length);
			SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
		}

		public ICommand NextSlide
		{
			get => new DelegateCommand((obj) => {
				if (SelectedAnswer == null) return;
				answer.AddAnswer(SelectedAnswer.IsTrue ? AnswerForTest.AnswerType.Good : AnswerForTest.AnswerType.Bad);
				SelectedQuestionId++;
				
				if(SelectedQuestionId >= SelectedTest.Questions.Length)
				{
					answer.End = DateTime.Now;
					Controller.AddAnswer(answer);
					
				}
				else SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
			});
		}

	}
}
