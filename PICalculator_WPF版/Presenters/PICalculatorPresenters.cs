using PICalculator_WPF版.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PICalculator_WPF版.Contract.PIMissionContract;

namespace PICalculator_WPF版.Presenters
{
    internal class PICalculatorPresenters : IPICalculatorPresenter
    {
        ISapleModelView SapleModelView;
        public PICalculatorPresenters(ISapleModelView sapleModelView)
        {
            SapleModelView = sapleModelView;
        }
        public async Task CreateSampleModel(long sampleValue)
        {
            SampleModel sampleModel = new SampleModel();
            sampleModel.Sample = sampleValue;
            PIMission pIMission = new PIMission(sampleValue);
            sampleModel.Value = await pIMission.Calculate();
            sampleModel.Time = DateTime.Now;
            SapleModelView.ResponseSampleModel(sampleModel);
        }


    }
}
