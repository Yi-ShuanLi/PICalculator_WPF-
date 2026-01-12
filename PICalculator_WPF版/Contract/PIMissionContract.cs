using PICalculator_WPF版.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator_WPF版.Contract
{
    public class PIMissionContract
    {
        public interface ISapleModelView
        {
            void ResponseSampleModel(SampleModel sampleModel);
        }
        public interface IPICalculatorPresenter
        {
            Task CreateSampleModel(long sampleValue);
        }
    }
}
