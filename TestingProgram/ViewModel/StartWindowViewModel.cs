using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestingProgram.Model;
using TestingProgram.Model.Testing.TestConverters;
using TestingProgram.View;

namespace TestingProgram.ViewModel
{
	internal class StartWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

		ResultsWindow resultsWindow = new ResultsWindow();
		TestingWindow testingWindow = new TestingWindow();

		private string pathTestFile = "Select file";
		public string PathTestFile { get => pathTestFile;  set
			{
				pathTestFile = value;
				OnPropertyChanged();
			}
		}

		private string ip;
		private int port;

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
				}
			});
		}

		private void Controller_StartTestingToClient(Model.Testing.Test test)
		{
			testingWindow.Show();
		}

		public ICommand StartServer
		{
			get => new DelegateCommand((obj) =>
			{
				if (Controller.StartServer(Port))
				{
					Controller.StartTestingToServer += Controller_StartTestingToServer;
					try
					{
						string testText = File.ReadAllText(PathTestFile);
						ITestConverter converter = new STSTConvertor();
						Controller.StartTesting(new Model.Testing.Test(converter, testText));
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			});
		}

		private void Controller_StartTestingToServer(Model.Testing.Test test)
		{
			resultsWindow.Show();
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
	}
}
