using System.Collections.Generic;

namespace TasksControl.Interfaces.Services
{
    public interface IControlService
    {
        string Init();
        IList<string> Monitoring();
        string Restart(string urlService);

    }
}
