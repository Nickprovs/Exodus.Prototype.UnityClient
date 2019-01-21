using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Types;
using Microsoft.AspNet.SignalR.Client;


namespace Assets.Interfaces
{
    public interface IUcSignalrProxy
    {

        IHubProxy HubProxy { get; set; }

        HubConnection Connection { get; set; }

        void ConnectAsync(string ipAddress, string port);

        void RequestStartupData();

        Task<int> AddSource(UcSource source);

        Task<int> RemoveSource(int sourceId);

        Task<int> AddSourceInstance(UcSourceInstance sourceInstance);

        Task<int> RemoveSourceInstance(int sourceInstanceId);

        Task<int> ModifySourceInstance(UcSourceInstance sourceInstance);

        Task<int> AddDigitalWall(UcDigitalWall digitalWall);

        Task<int> AddSpaceSession(UcSpaceSession spaceSession, int digitalWallId);

        Task<int> RemoveSpaceSession(int spaceSessionId);

    }
}
