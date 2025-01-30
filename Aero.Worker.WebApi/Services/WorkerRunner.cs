using System.Diagnostics;
using Aero.Base;
using Aero.Base.Attributes;
using Aero.Base.Constants;
using Aero.Worker.WebApi.Models;

namespace Aero.Worker.WebApi.Services;

[Injectable]
public class WorkerRunner
{
    private readonly IUserService _userService;

    public WorkerRunner(IUserService userService)
    {
        _userService = userService;
    }

    public WorkerLaunchResponse RunWorkerOutOfProcess(WorkerLaunchRequest workerLaunchRequest)
    {
        ArgumentNullException.ThrowIfNull(workerLaunchRequest.WorkerName);
        ArgumentNullException.ThrowIfNull(workerLaunchRequest.Module);
        
        try
        {
            //TODO 
            var workerPath = "";
            var guid = Guid.NewGuid().ToString();
            var userName = _userService.GetUserName();
            
            var processStartInfo = new ProcessStartInfo
            {
                //TODO 
                Arguments = $"{workerPath} --{WorkerArgumentNames.WorkerName}={workerLaunchRequest.WorkerName} --{WorkerArgumentNames.ModuleCode}={workerLaunchRequest.Module} --{WorkerArgumentNames.UserName}={userName} --{WorkerArgumentNames.Guid}={guid}",
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                RedirectStandardError = true,
            };
            
            Process.Start(processStartInfo);
        }
        catch (Exception e)
        {
            //TODO 
            Console.WriteLine(e);
            throw;
        }

        return new WorkerLaunchResponse(WorkerId: 0, WorkerStatus.Pending);
    }
}