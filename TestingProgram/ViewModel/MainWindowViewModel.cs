using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TestingProgram.ViewModel
{
	class MainWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

		//public AnswerForTest answer = new AnswerForTest(,, DateTime.Now, );


		public ICommand ToAnswer
		{
			get => new DelegateCommand((obj) => { 
				
			});
		}
	}
}
