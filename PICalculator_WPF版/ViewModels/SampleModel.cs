using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator_WPF版.ViewModels
{

    public class SampleModel
    {
        public long Sample { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
        public SampleModel() { }


    }
}
