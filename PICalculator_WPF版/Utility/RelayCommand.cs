using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculator_WPF版.Utility
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> _callback;
        public RelayCommand(Action<object> callback)
        {
            _callback = callback;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _callback.Invoke(parameter);
        }
    }
}
