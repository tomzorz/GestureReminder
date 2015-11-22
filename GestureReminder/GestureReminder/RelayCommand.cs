using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestureReminder
{
    public class RelayCommand : ICommand
    {
        private readonly Action _a;

        public RelayCommand(Action a)
        {
            _a = a;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _a.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}