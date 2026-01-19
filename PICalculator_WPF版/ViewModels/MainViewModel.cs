using PICalculator_WPF版.Presenters;
using PICalculator_WPF版.Utility;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static PICalculator_WPF版.Contract.PIMissionContract;

namespace PICalculator_WPF版.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : ISapleModelView
    {
        public IPICalculatorPresenter pIMissionPresenter;
        public ObservableCollection<SampleModel> sampleModels { get; set; } = new ObservableCollection<SampleModel>();
        public long Sample { get; set; } // SampleSizeTxt.Text
        public SampleModel SampleModel;
        public System.Threading.Timer timer { get; set; }
        [AlsoNotifyFor(nameof(RunningButtonText))]
        public bool IsStopOrReStart { get; set; } = true;
        public string RunningButtonText => IsStopOrReStart ? "停止" : "重新開始";

        public ICommand AddCommand { get; set; }
        public ICommand StopCommand { get; set; }



        public MainViewModel()
        {
            pIMissionPresenter = new PICalculatorPresenters(this);
            pIMissionPresenter.StartMission();
            timer = new System.Threading.Timer(x =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.pIMissionPresenter.FetchCompletedMission();

                });
            }, null, 0, 1000);

            AddCommand = new RelayCommand((x) =>
            {
                this.AddSample();
            });
            StopCommand = new RelayCommand((x) =>
            {
                this.StopSample();
            });

        }
        public void AddSample()
        {
            SampleModel sample = sampleModels.Where(x => x.Sample == Sample).FirstOrDefault();
            if (sample != null)
            {
                Sample = 0;
                return;
            }
            SampleModel sampleModel = pIMissionPresenter.SendMissionRequest(Sample);
            sampleModels.Add(sampleModel);
            Sample = 0;
        }

        public void StopSample()
        {
            if (IsStopOrReStart)
            {
                pIMissionPresenter.StoptMission();
            }
            else
            {
                pIMissionPresenter.StartMission();
            }
            IsStopOrReStart = !IsStopOrReStart;
        }


    }
}
