using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TestingProgram.Model.Testing;
using TestingProgram.Model;

namespace TestingProgram.ViewModel
{
          internal class ResultsWindowViewModel : INotifyPropertyChanged
          {
                    public event PropertyChangedEventHandler PropertyChanged;
                    public void OnPropertyChanged([CallerMemberName] string prop = "")
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

                    public List<AnswerForTest> answers { get; set; } = new List<AnswerForTest>();
                    public AnswerForTest[] Answers { get => answers.ToArray(); }

                    public ResultsWindowViewModel()
                    {
                              Controller.GetAnswerToServer += Controller_GetAnswerToServer;
                    }

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
          }
}
