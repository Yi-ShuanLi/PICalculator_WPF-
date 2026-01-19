using PICalculator_WPF版.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICalculator_WPF版
{
    [AddINotifyPropertyChangedInterface]
    internal class PIMission
    {
        public SampleModel SampleModel { get; set; }


        public PIMission(SampleModel sampleModel)
        {
            SampleModel = sampleModel;
            SampleModel.StatesShow = States.Running;
        }
        public async Task<double> Calculate()
        {
            //long ROW_COUNT = SampleSize;
            //long batchcount = 2_500_000;
            //long times = ROW_COUNT % batchcount == 0 ? ROW_COUNT / batchcount : (ROW_COUNT / batchcount) + 1;
            //List<Task> tasks = new List<Task>();
            //int sum = 0;
            //for (int i = 0; i < times; i++)
            //{
            //    long index = i;
            //    long start = index * batchcount;
            //    long end = index * batchcount + batchcount;
            //    tasks.Add(GetIntAsync(start, batchcount));
            //}
            //sum+= await Task.WhenAll(tasks);
            if (this.SampleModel.TokenSource.IsCancellationRequested)
            {
                SampleModel.StatesShow = States.Stopped;
                return 0;
            }
            try
            {
                return 4.0 * await GetIntAsync(0, this.SampleModel.Sample) / (this.SampleModel.Sample - 1);
            }
            catch (Exception ex)
            {
                SampleModel.StatesShow = States.Stopped;
                return 0;
            }
        }


        public async Task<long> GetIntAsync(long start, long count)
        {
            //object key = new object();
            //Random random = new Random();
            //int total = 0;

            //await Parallel.ForAsync(0, this.SampleModel.Sample, new ParallelOptions() { MaxDegreeOfParallelism = 6, CancellationToken = this.SampleModel.TokenSource.Token }, (x, token) =>
            //{


            //    double num1 = Random.Shared.NextDouble(); //random.NextDouble();
            //    double num2 = Random.Shared.NextDouble();//random.NextDouble();
            //    if (Math.Pow(num1, 2) + Math.Pow(num2, 2) < 1)
            //    {
            //        //lock (key)
            //        //{
            //        //    total++;
            //        //}
            //        Interlocked.Increment(ref total);
            //    }

            //    return ValueTask.CompletedTask;
            //});
            //return total;




            long ROW_COUNT = SampleModel.Sample;
            long batchcount = 2_500_000;
            long times = ROW_COUNT % batchcount == 0 ? ROW_COUNT / batchcount : (ROW_COUNT / batchcount) + 1;
            Random random = new Random();
            long total = 0;
            double num1 = 0;
            double num2 = 0;
            await Parallel.ForAsync(0, times, new ParallelOptions() { MaxDegreeOfParallelism = 6, CancellationToken = this.SampleModel.TokenSource.Token }, (x, token) =>
            {
                long index = x;
                long start = index * batchcount;
                long end = (index * batchcount + batchcount) >= ROW_COUNT ? ROW_COUNT : (index * batchcount + batchcount);
                long sum = 0;
                Console.WriteLine($"第{index + 1}批計算中，從第{start}到第{end}資料");
                for (long i = start; i < end; i++)
                {
                    num1 = Random.Shared.NextDouble();
                    num2 = Random.Shared.NextDouble();
                    if ((num1 * num1) + (num2 * num2) < 1)
                        sum++;
                }
                Interlocked.Add(ref total, sum);
                return ValueTask.CompletedTask;
            });


            return total;

        }
    }
}
