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
        public long SampleSize { get; set; }
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
            int sum = await GetIntAsync(0, SampleSize);
            return 4.0 * sum / (SampleSize - 1);
        }
        public PIMission(long sampleSize)
        {
            SampleSize = sampleSize;
        }

        public async Task<int> GetIntAsync(long start, long count)
        {
            long ROW_COUNT = SampleSize;
            long batchcount = 2_500_000;
            long times = ROW_COUNT % batchcount == 0 ? ROW_COUNT / batchcount : (ROW_COUNT / batchcount) + 1;
            Random random = new Random();
            int total = 0;
            await Parallel.ForAsync(0, times, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, (x, token) =>
            {

                long index = x;
                long start = index * batchcount;
                long end = (index * batchcount + batchcount) >= ROW_COUNT ? ROW_COUNT : (index * batchcount + batchcount);
                int sum = 0;
                Console.WriteLine($"第{index + 1}批計算中，從第{start}到第{end}資料");
                for (long i = start; i < end; i++)
                {
                    double num1 = random.NextDouble();
                    double num2 = random.NextDouble();
                    if (Math.Pow(num1, 2) + Math.Pow(num2, 2) < 1)
                        sum++;
                }
                total += sum;
                return ValueTask.CompletedTask;
            });
            return total;
            //return await Task.Run(() =>
            //{
            //    Random random = new Random();
            //    int sum = 0;
            //    for (long i = start; i <= count; i++)
            //    {
            //        double num1 = random.NextDouble();
            //        double num2 = random.NextDouble();
            //        if (Math.Pow(num1, 2) + Math.Pow(num2, 2) < 1)
            //            sum++;
            //    }
            //    return sum;
            //});
        }
    }
}
