using PICalculator_WPF版.Utility;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculator_WPF版.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SampleModel
    {
        public long Sample { get; set; }
        public DateTime Time { get; set; }
        public double UsedTimes { get; set; }
        public double Value { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
        public States StatesShow { get; set; }

        public bool ButtonEnable => StatesShow != States.Completed;
        public ICommand StopSampleModelCommand { get; set; }

        public SampleModel()
        {
            TokenSource = new CancellationTokenSource();
            StopSampleModelCommand = new RelayCommand((x) =>
            {
                this.StopSampleModel(x);
            });
        }
        public void StopSampleModel(Object obj)
        {
            this.TokenSource.Cancel();
        }

    }
}
