using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.AdministrativeSync;
using PAKNAPI.Models.SyncData;
using Quartz;
using System.Threading.Tasks;
[DisallowConcurrentExecution]
public class MyJobDichVuCongQuocGia : IJob
{
    private readonly ILogger<MyJobAdministrative> _logger;
    private readonly IAppSetting _appSetting;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public MyJobDichVuCongQuocGia(ILogger<MyJobAdministrative> logger, IAppSetting appSetting, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _appSetting = appSetting;
        _hostingEnvironment = hostEnvironment;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await new DichVuCongQuocGiaSync(_appSetting, _hostingEnvironment).SyncDichVuCongQuocGia();
        return;
    }
}