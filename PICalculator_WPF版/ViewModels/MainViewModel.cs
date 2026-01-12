using PICalculator_WPF版.Presenters;
using PICalculator_WPF版.Utility;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PICalculator_WPF版.Contract.PIMissionContract;

namespace PICalculator_WPF版.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : ISapleModelView
    {
        public IPICalculatorPresenter pIMissionPresenter;
        public ObservableCollection<SampleModel> sampleModels { get; set; } = new ObservableCollection<SampleModel>();
        public long Sample { get; set; }
        public SampleModel SampleModel;
        public ICommand AddCommand { get; set; }

        public MainViewModel()
        {
            pIMissionPresenter = new PICalculatorPresenters(this);
            AddCommand = new RelayCommand((x) =>
            {
                this.AddSample();
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
            pIMissionPresenter.CreateSampleModel(Sample);

        }

        public void ResponseSampleModel(SampleModel sampleModel)
        {
            SampleModel = sampleModel;
            sampleModels.Add(SampleModel);
            Sample = 0;
        }
    }
}
