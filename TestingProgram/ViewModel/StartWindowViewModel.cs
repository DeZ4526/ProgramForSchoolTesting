using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestingProgram.Model;
using TestingProgram.View;

namespace TestingProgram.ViewModel
{
	internal class StartWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		
		ResultsWindow resultsWindow = new ResultsWindow();


		public ICommand StartClient
		{
			get => new DelegateCommand((obj) => {

			});
		}

		public ICommand StartServer
		{
			get => new DelegateCommand((obj) => {
				if (Controller.StartServer(4526))
				{
					Controller.StartTestingToServer += Controller_StartTestingToServer;
				}
			});
		}

		private void Controller_StartTestingToServer(Model.Testing.Test test)
		{
			resultsWindow.Show();
		}
	}
}
