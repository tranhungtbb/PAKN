using PAKNAPI.Common;
using PAKNAPI.Models.SyncData;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Job
{
    /// <summary>
    /// cổng thông tin điện tử tỉnh
    /// </summary>
    public class MyJobFeedBack : IJob
    {
        private readonly IAppSetting _appSetting;
        public MyJobFeedBack(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            await new FeedBackSync(_appSetting).SyncFeedBack();
            return;
        }
    }
}
