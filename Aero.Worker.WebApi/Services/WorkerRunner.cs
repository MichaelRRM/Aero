using System.Diagnostics;
using Aero.Base;
using Aero.Base.Constants;

namespace Aero.Worker.WebApi.Services;

public class WorkerRunner : IWorkerRunner
{
    private readonly IUserService _userService;

    public WorkerRunner(IUserService userService)
    {
        _userService = userService;
    }

    public void RunOutOfProcess()
    {
        try
        {
            //TODO 
            var workerPath = "";
            var guid = Guid.NewGuid().ToString();
            var userName = _userService.GetUserName();
            
            var processStartInfo = new ProcessStartInfo
            {
                //TODO 
                Arguments = $"{workerPath} --{WorkerArgumentNames.UserName}={userName} --{WorkerArgumentNames.Guid}={guid}",
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
    }
}