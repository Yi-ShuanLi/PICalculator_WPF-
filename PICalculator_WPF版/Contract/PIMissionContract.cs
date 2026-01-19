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

        }
        public interface IPICalculatorPresenter
        {
            /// <summary>
            /// 啟動執行緒任務，不斷接收來自Queue裡面的資料，並開始執行模擬計算
            /// </summary>
            void StartMission();

            /// <summary>
            /// 暫停執行緒任務，停止接收Queue的資料
            /// </summary>
            void StoptMission();

            /// <summary>
            /// 取得當前已經完成模擬計算的任務，會自動呼叫 <see cref="ISapleModelView.UpdateDataView()"/>
            /// </summary>
            void FetchCompletedMission();


            /// <summary>
            /// 傳入指定的sampeSize次數來執行PI模擬計算
            /// </summary>
            /// <param name="sampleSize">決定執行多少模擬次數</param>
            SampleModel SendMissionRequest(long sampleSize);


            //Task CreateSampleModel(long sampleValue);
        }
    }
}
