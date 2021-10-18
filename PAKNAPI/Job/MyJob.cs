using Microsoft.Extensions.Logging;
using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.AdministrativeSync;
using Quartz;
using System.Threading.Tasks;
[DisallowConcurrentExecution]
public class MyJob : IJob
{
    private readonly ILogger<MyJob> _logger;
    private readonly IAppSetting _appSetting;
    public MyJob(ILogger<MyJob> logger, IAppSetting appSetting)
    {
        _logger = logger;
        _appSetting = appSetting;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        
        await new AdministrativeSync(_appSetting).SyncThuTucHanhChinh();
        return;
    }
}