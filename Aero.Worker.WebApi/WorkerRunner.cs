using System.Diagnostics;
using Aero.Base;

namespace Aero.Worker.WebApi;

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
                Arguments = $"{workerPath} --userName={userName} --guid={guid}",
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