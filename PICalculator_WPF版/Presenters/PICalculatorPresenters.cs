using PICalculator_WPF版.ViewModels;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using static PICalculator_WPF版.Contract.PIMissionContract;

namespace PICalculator_WPF版.Presenters
{
    internal class PICalculatorPresenters : IPICalculatorPresenter
    {
        ISapleModelView SapleModelView;
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1, 1);
        ConcurrentQueue<long> sampleValues = new ConcurrentQueue<long>();
        // Channel<long> channel = Channel.CreateUnbounded<long>();
        ConcurrentBag<SampleModel> _cache = new ConcurrentBag<SampleModel>();
        Dictionary<long, SampleModel> keyValuePairs = new Dictionary<long, SampleModel>();
        CancellationTokenSource tokenSource = null;
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        public PICalculatorPresenters(ISapleModelView sapleModelView)
        {
            SapleModelView = sapleModelView;
        }
        public void FetchCompletedMission()
        {
            if (_cache.Count > 0)
            {
                var results = _cache.ToList();

                _cache.Clear();
            }

        }

        public SampleModel SendMissionRequest(long sampleSize)
        {
            if (keyValuePairs.ContainsKey(sampleSize) || tokenSource.Token.IsCancellationRequested)
                return null;
            SampleModel sampleModel = new SampleModel();
            sampleModel.Sample = sampleSize;
            sampleModel.Time = DateTime.Now;
            sampleModel.StatesShow = States.Pending;
            keyValuePairs.Add(sampleSize, sampleModel);
            sampleValues.Enqueue(sampleSize);
            //autoResetEvent.Set();//開門
            Locker.Release();
            return sampleModel;
        }

        public async void StartMission()
        {
            tokenSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                while (true)
                {
                    Locker.Wait();

                    if (sampleValues.TryDequeue(out var sampleSize))
                    {
                        if (tokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        SampleModel sampleModel = keyValuePairs[sampleSize];
                        sampleModel.StatesShow = States.Pending;
                        PIMission pIMission = new PIMission(sampleModel);
                        Stopwatch totalStopwatch = new Stopwatch();
                        totalStopwatch.Start();
                        sampleModel.Value = await pIMission.Calculate();
                        totalStopwatch.Stop();
                        sampleModel.UsedTimes = totalStopwatch.Elapsed.TotalSeconds;
                        sampleModel.StatesShow = States.Completed;
                        if (sampleModel.Value == 0)
                        {
                            keyValuePairs.Remove(sampleSize);
                        }
                        else
                        {
                            _cache.Add(sampleModel);
                        }
                    }

                }
            });


            #region 第二版
            //await Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        autoResetEvent.WaitOne();//閘門
            //        sampleValues.TryDequeue(out var sampleSize);
            //        if (tokenSource.Token.IsCancellationRequested)
            //        {
            //            break;
            //        }
            //        SampleModel sampleModel = keyValuePairs[sampleSize];
            //        sampleModel.StatesShow = States.Pending;
            //        PIMission pIMission = new PIMission(sampleModel);
            //        sampleModel.Value = await pIMission.Calculate();
            //        sampleModel.StatesShow = States.Completed;

            //        if (sampleModel.Value == 0)
            //        {
            //            keyValuePairs.Remove(sampleSize);
            //        }
            //        else
            //        {
            //            _cache.Add(sampleModel);
            //        }
            //    }

            //});
            #endregion

            #region 第一版
            //await foreach (long sampleSize in channel.Reader.ReadAllAsync())
            //{
            //    if (tokenSource.Token.IsCancellationRequested)
            //    {
            //        break;
            //    }
            //    SampleModel sampleModel = keyValuePairs[sampleSize];
            //    sampleModel.StatesShow = States.Pending;
            //    PIMission pIMission = new PIMission(sampleModel);
            //    sampleModel.Value = await pIMission.Calculate();
            //    sampleModel.StatesShow = States.Completed;

            //    if (sampleModel.Value == 0)
            //    {
            //        keyValuePairs.Remove(sampleSize);
            //    }
            //    else
            //    {
            //        _cache.Add(sampleModel);
            //    }
            //}
            //await Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        if (tokenSource.Token.IsCancellationRequested)
            //        {
            //            break;
            //        }
            //        if (!sampleValues.IsEmpty)
            //        {
            //            if (!sampleValues.TryDequeue(out long sampleSize))
            //                continue;

            //            SampleModel sampleModel = keyValuePairs[sampleSize];
            //            sampleModel.StatesShow = States.Pending;
            //            PIMission pIMission = new PIMission(sampleModel);
            //            sampleModel.Value = await pIMission.Calculate();
            //            sampleModel.StatesShow = States.Completed;

            //            if (sampleModel.Value == 0)
            //            {
            //                keyValuePairs.Remove(sampleSize);
            //            }
            //            else
            //            {
            //                _cache.Add(sampleModel);
            //            }
            //        }
            //    }
            //});
            #endregion
        }

        public void StoptMission()
        {
            tokenSource.Cancel();
        }
    }
}
