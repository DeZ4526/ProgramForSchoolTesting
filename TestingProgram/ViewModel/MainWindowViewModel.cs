using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestingProgram.Model;
using TestingProgram.Model.Testing;
using TestingProgram.View;
using static TestingProgram.Model.Testing.Question;

namespace TestingProgram.ViewModel
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


		public MainWindowViewModel()
		{
            Controller.GetAnswerToServer += Controller_GetAnswerToServer;
        }

		private string pathTestFile = "Select file";
		public string PathTestFile { get => pathTestFile;  set
			{
				pathTestFile = value;
				OnPropertyChanged();
			}
		}

		private string ip = "127.0.0.1";
		private int port = 4526;

		public string Ip
		{
			get => ip; set
			{
				ip = value;
				OnPropertyChanged();
			}
		}
		public int Port { get => port; set
			{
				port = value;
				OnPropertyChanged();
			}
		}
		public Visibility VisibilityStartServer
		{
			get => visibilityStartServer;
			set
			{
				visibilityStartServer = value;
				OnPropertyChanged();
			}
		}
		public Visibility VisibilityConnectServer
		{
			get => visibilityConnectServer;
			set
			{
				visibilityConnectServer = value;
				OnPropertyChanged();
			}
		}
		public Visibility VisibilityStartMenu
		{
			get => visibilityStartMenu;
			set
			{
				visibilityStartMenu = value;
				OnPropertyChanged();
			}
		}

		private Visibility visibilityStartServer { get; set; } = Visibility.Hidden;
		private Visibility visibilityConnectServer { get; set; } = Visibility.Hidden;
		private Visibility visibilityStartMenu { get; set; } = Visibility.Visible;


        public Visibility VisibilityStartMenuAll
        {
            get => visibilityStartMenuAll;
            set
            {
                visibilityStartMenuAll = value;
                OnPropertyChanged();
            }
        }
        private Visibility visibilityStartMenuAll { get; set; } = Visibility.Visible;

        public Visibility VisibilityTestingMenu
        {
            get => visibilityTestingMenu;
            set
            {
                visibilityTestingMenu = value;
                OnPropertyChanged();
            }
        }
        private Visibility visibilityTestingMenu { get; set; } = Visibility.Hidden;


        public Visibility VisibilityResultsMenu
        {
            get => visibilityResultsMenu;
            set
            {
                visibilityResultsMenu = value;
                OnPropertyChanged();
            }
        }
        private Visibility visibilityResultsMenu { get; set; } = Visibility.Hidden;

        public ICommand ShowStartServer
		{
			get => new DelegateCommand((obj) =>
			{
				VisibilityStartMenu = Visibility.Hidden;
				VisibilityConnectServer = Visibility.Hidden;
				VisibilityStartServer = Visibility.Visible;
			});
		}
		public ICommand ShowConnectServer
		{
			get => new DelegateCommand((obj) =>
			{
				VisibilityStartMenu = Visibility.Hidden;
				VisibilityConnectServer = Visibility.Visible;
				VisibilityStartServer = Visibility.Hidden;
			});
		}
		public ICommand ShowStartMenu
		{
			get => new DelegateCommand((obj) =>
			{
				VisibilityStartMenu = Visibility.Visible;
				VisibilityConnectServer = Visibility.Hidden;
				VisibilityStartServer = Visibility.Hidden;
			});
		}
		public ICommand StartClient
		{
			get => new DelegateCommand((obj) =>
			{

				if (Controller.Connect(Ip, Port))
				{
					Controller.StartTestingToClient += Controller_StartTestingToClient;
					MessageBox.Show("Done!");
				}
			});
		}

		private void Controller_StartTestingToClient(Model.Testing.Test test)
		{
            answer = new AnswerForTest("test user", "", DateTime.Now, Controller.SelectedTest.Questions.Length);
            SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
            VisibilityStartMenuAll = Visibility.Hidden;
            VisibilityTestingMenu = Visibility.Visible;
            //            testingWindow.Dispatcher.Invoke(() => testingWindow.Show());
        }

		public ICommand StartServer
		{
			get => new DelegateCommand((obj) =>
			{
				if (Controller.StartServer(Port))
				{
					if(PathTestFile == "Select file") {
                        MessageBox.Show("Please selected test file");
						return;
                    }

                    Controller.StartTestingToServer += Controller_StartTestingToServer;
					try
					{
						Controller.SetSelectedTest(PathTestFile);
						Controller.StartTesting(Controller.SelectedTest);
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
                else MessageBox.Show("Sever doesn't start");
            });
		}
		//Results
		private void Controller_StartTestingToServer(Model.Testing.Test test)
		{
            VisibilityStartMenuAll= Visibility.Hidden;
            VisibilityResultsMenu = Visibility.Visible;
		}

		public ICommand SelectTestFile
		{
			get => new DelegateCommand((obj) =>
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.ShowDialog();

				PathTestFile = dialog.FileName;
			});
		}


        public List<AnswerForTest> answers { get; set; } = new List<AnswerForTest>();
        public AnswerForTest[] Answers { get => answers.ToArray(); }


        private void Controller_GetAnswerToServer(AnswerForTest answer)
        {
            answers.Add(answer);
            OnPropertyChanged("Answers");
        }

        public ICommand EndTesting
        {
            get => new DelegateCommand((obj) =>
            {
                Controller.EndTesting();
            });
        }
        //Testing
        private AnswerForTest answer;
        public AnswerForTest Answer
        {
            get => answer; set
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
        public Question SelectedQuestion
        {
            get => selectedQuestion; set
            {
                selectedQuestion = value;
                OnPropertyChanged();
            }
        }

        

        public ICommand NextSlide
        {
            get => new DelegateCommand((obj) => {
                if (SelectedAnswer == null) return;
                answer.AddAnswer(SelectedAnswer.IsTrue ? AnswerForTest.AnswerType.Good : AnswerForTest.AnswerType.Bad);
                SelectedQuestionId++;

                if (SelectedQuestionId >= SelectedTest.Questions.Length)
                {
                    answer.End = DateTime.Now;
                    Controller.AddAnswer(answer);

                }
                else SelectedQuestion = SelectedTest.Questions[SelectedQuestionId];
            });
        }





    }
}
