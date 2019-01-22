using Assets.Interfaces;
using Assets.Scenes.Scripts.Signals;
using Assets.Types;
using Assets.Types.Mappers;
using Exodus.Common.Data.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Utilities
{
    public class UcSignalrProxy : IUcSignalrProxy, IDisposable
    {
        #region Fields

        private IUcProfile _profile;

        private readonly Dictionary<StartupCacheKey, IEnumerable<object>> _startupCache = new Dictionary<StartupCacheKey, IEnumerable<object>>();

        private bool _startupCacheProcessed;

        private IUnityMainThreadDispatcher _unityMainThreadDispatcher;

        private SignalBus _signalBus;

        private ICommonValues _commonValues;

        #endregion

        #region Properties

        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }
        public ConnectionState ConnectionState { get; set; } = ConnectionState.Disconnected;

        #endregion

        #region Methods

        #region Dependency Injection
        [Inject]
        public void Setup(IUcProfile profile, IUnityMainThreadDispatcher unityMainThreadDispatcher, SignalBus signalBus, ICommonValues commonValues)
        {
            this._profile = profile;
            this._unityMainThreadDispatcher = unityMainThreadDispatcher;
            this._signalBus = signalBus;
            this._commonValues = commonValues;
        }

        #endregion

        #region Connection Handling

        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        public async void ConnectAsync(string ipAddress, string port)
        {
            var ServerUri = "http://" + ipAddress + ":" + port;


            this.Connection = new HubConnection(ServerUri);
            this.Connection.Closed += OnConnectionClosed;
            this.Connection.StateChanged += this.Connection_StateChanged;
            HubProxy = Connection.CreateHubProxy("MyHub");

            this.WireReceiveMethods();

            try
            {
                await this.Connection.Start();
                this.RequestStartupData();
            }
            catch (HttpRequestException)
            {
                Debug.LogError("Unable to connect to server: Start server before connecting clients.");
                return;
            }
        }

        private void Connection_StateChanged(StateChange args)
        {
            this.ConnectionState = args.NewState;

            switch (args.NewState)
            {
                case ConnectionState.Connected:
                    this._signalBus.Fire(new ConnectedSignal(args.OldState, args.NewState));
                    break;
                case ConnectionState.Connecting:
                    this._signalBus.Fire(new ConnectingSignal(args.OldState, args.NewState));
                    break;
                case ConnectionState.Disconnected:
                    this._signalBus.Fire(new DisconnectedSignal(args.OldState, args.NewState));
                    break;
                case ConnectionState.Reconnecting:
                    this._signalBus.Fire(new ReconnectingSignal(args.OldState, args.NewState));
                    break;

            }
        }

        private void OnConnectionClosed()
        {

        }

        #endregion

        #region Hub Interaction

        private void WireReceiveMethods()
        {
            //Startup
            HubProxy.On<IEnumerable<SourceDto>>("ReceivedSources", sources => this.ReceivedSources(sources));
            HubProxy.On<IEnumerable<SourceInstanceDto>>("ReceivedSourceInstances", sourceInstances => this.ReceivedSourceInstances(sourceInstances));
            HubProxy.On<IEnumerable<DigitalWallDto>>("ReceivedDigitalWalls", digitalWalls => this.ReceivedDigitalWalls(digitalWalls));
            HubProxy.On<IEnumerable<SpaceSessionDto>>("ReceivedSpaceSessions", spaceSessions => this.ReceivedSpaceSessions(spaceSessions));

            //Non-Startup
            HubProxy.On<SourceDto>("SourceAdded", source => this.SourceAdded(source));
            HubProxy.On<int>("SourceRemoved", sourceId => this.SourceRemoved(sourceId));

            HubProxy.On<SourceInstanceDto>("SourceInstanceAdded", sourceInstance => this.SourceInstanceAdded(sourceInstance));
            HubProxy.On<int, int>("SourceInstanceRemoved", (sourceInstanceId, wallId) => this.SourceInstanceRemoved(sourceInstanceId, wallId));
            HubProxy.On<SourceInstanceDto>("SourceInstanceModified", sourceInstanceId => this.SourceInstanceModified(sourceInstanceId));


            HubProxy.On<DigitalWallDto>("DigitalWallAdded", digitalWall => this.DigitalWallAdded(digitalWall));
            HubProxy.On<int>("DigitalWallRemoved", digitalWallId => this.DigitalWallRemoved(digitalWallId));

            HubProxy.On<SpaceSessionDto>("SpaceSessionAdded", spaceSession => this.SpaceSessionAdded(spaceSession));
            HubProxy.On<int>("SpaceSessionRemoved", spaceSessionId => this.SpaceSessionRemoved(spaceSessionId));
        }

        #region Send

        public void RequestStartupData()
        {
            this.RequestSources();
            this.RequestSourceInstances();
            this.RequestDigitalWalls();
            this.RequestSpaceSessions();
        }

        public void RequestSources()
        {
            HubProxy.Invoke("RequestSources");
        }

        public void RequestSourceInstances()
        {
            HubProxy.Invoke("RequestSourceInstances");
        }

        public void RequestDigitalWalls()
        {
            HubProxy.Invoke("RequestDigitalWalls");
        }

        public void RequestSpaceSessions()
        {
            HubProxy.Invoke("RequestSpaceSessions");
        }

        public async Task<int> AddSource(UcSource source)
        {
            int id = await HubProxy.Invoke<int>("AddSource", SourceMapper.GetDtoSourceFromUcSource(source));
            return id;
        }

        public async Task<int> RemoveSource(int sourceId)
        {
            int id = await HubProxy.Invoke<int>("RemoveSource", sourceId);
            return id;
        }

        public async Task<int> AddSourceInstance(UcSourceInstance sourceInstance)
        {
            int id = await HubProxy.Invoke<int>("AddSourceInstance", SourceInstanceMapper.GetDtoSourceInstanceFromUcSourceInstance(sourceInstance));
            return id;
        }

        public async Task<int> RemoveSourceInstance(int sourceInstanceId)
        {
            int id = await HubProxy.Invoke<int>("RemoveSourceInstance", sourceInstanceId);
            return id;
        }

        public async Task<int> ModifySourceInstance(UcSourceInstance sourceInstance)
        {
            var sourceInstanceDto = SourceInstanceMapper.GetDtoSourceInstanceFromUcSourceInstance(sourceInstance);
            int id = await HubProxy.Invoke<int>("ModifySourceInstance", sourceInstanceDto);
            return id;
        }

        public async Task<int> AddDigitalWall(UcDigitalWall digitalWall)
        {
            int id = await HubProxy.Invoke<int>("AddDigitalWall", DigitalWallMapper.GetDtoDigitalWallFromUcDigitalWall(digitalWall));
            return id;
        }

        public async Task<int> RemoveDigitalWall(int digitalWallId)
        {
            int id = await HubProxy.Invoke<int>("RemoveDigitalWall", digitalWallId);
            return id;
        }

        public async Task<int> AddSpaceSession(UcSpaceSession spaceSession, int digitalWallId)
        {
            SpaceSessionDto newSession = SpaceSessionMapper.GetDtoSpaceSessionFromUcSpaceSession(spaceSession);
            newSession.DigitalWallId = digitalWallId;
            int id = await HubProxy.Invoke<int>("AddSpaceSession", newSession);
            return id;
        }

        public async Task<int> RemoveSpaceSession(int spaceSessionId)
        {
            int id = await HubProxy.Invoke<int>("RemoveSpaceSession", spaceSessionId);
            return id;
        }


        #endregion

        #region Receive

        //Startup 

        private void ReceivedSources(IEnumerable<SourceDto> sources)
        {
            this._startupCache.Add(StartupCacheKey.Sources, sources);
            this.TryProcessStartupCache();
        }

        private void ReceivedSourceInstances(IEnumerable<SourceInstanceDto> sourceInstances)
        {
            this._startupCache.Add(StartupCacheKey.SourceInstances, sourceInstances);
            this.TryProcessStartupCache();
        }

        private void ReceivedDigitalWalls(IEnumerable<DigitalWallDto> digitalWalls)
        {
            this._startupCache.Add(StartupCacheKey.DigitalWalls, digitalWalls);
            this.TryProcessStartupCache();
        }

        private void ReceivedSpaceSessions(IEnumerable<SpaceSessionDto> spaceSessions)
        {
            this._startupCache.Add(StartupCacheKey.SpaceSessions, spaceSessions);
            this.TryProcessStartupCache();
        }

        //Non-Startup
        private void SourceAdded(SourceDto source)
        {
            this._profile.Sources.Add(SourceMapper.GetUcSourceFromDtoSource(source));
        }

        private void SourceRemoved(int sourceId)
        {

            foreach (var source in this._profile.Sources.ToList())
                if (source.SourceId == sourceId)
                    this._profile.Sources.Remove(source);
        }

        private void SourceInstanceAdded(SourceInstanceDto sourceInstance)
        {
            var matchingSource = this._profile.Sources.FirstOrDefault(s => s.SourceId == sourceInstance.SourceId);
            var newSourceInstance = SourceInstanceMapper.GetUcSourceInstanceFromUcSourceAndDtoSourceInstance(matchingSource, sourceInstance, this._commonValues.PixelToUnityUnitScale, this._unityMainThreadDispatcher);
            var matchingWall = this._profile.DigitalWalls.FirstOrDefault(w => w.WallId == newSourceInstance.WallId);
            matchingWall.SourceInstances.Add(newSourceInstance);
        }

        private void SourceInstanceRemoved(int sourceInstanceId, int associatedWallId)
        {
            var matchingWall = this._profile.DigitalWalls.FirstOrDefault(w => w.WallId == associatedWallId);
            var matchingSourceInstance = matchingWall?.SourceInstances.FirstOrDefault(s => s.SourceInstanceId == sourceInstanceId);
            matchingWall?.SourceInstances.Remove(matchingSourceInstance);
        }

        private void SourceInstanceModified(SourceInstanceDto sourceInstance)
        {
            var matchingWall = this._profile.DigitalWalls.FirstOrDefault(w => w.WallId == sourceInstance.WallId);
            var matchingSourceInstance = matchingWall?.SourceInstances.FirstOrDefault(s => s.SourceInstanceId == sourceInstance.Id);

            //Necessary?
            //var toCopy = SourceInstanceMapper.GetUcSourceInstanceFromUcSourceAndDtoSourceInstance((UcSource)matchingSourceInstance, sourceInstance);
            this._unityMainThreadDispatcher.Enqueue(() =>
            {
                matchingSourceInstance.X = sourceInstance.X;
                matchingSourceInstance.Y = sourceInstance.Y;
                matchingSourceInstance.Width = sourceInstance.Width;
                matchingSourceInstance.Height = sourceInstance.Height;
                matchingSourceInstance.WallId = sourceInstance.WallId;
            });
        }

        private void DigitalWallAdded(DigitalWallDto digitalWall)
        {
            this._profile.DigitalWalls.Add(DigitalWallMapper.GetUcDigitalWallFromDtoDigitalWallAndSourceInstances(digitalWall, new ObservableCollection<UcSourceInstance>(), this._commonValues.PixelToUnityUnitScale, this._commonValues.WallCenter, this._unityMainThreadDispatcher));
        }

        private void DigitalWallRemoved(int digitalWallId)
        {
            var matchingWall = this._profile.DigitalWalls.FirstOrDefault(w => w.WallId == digitalWallId);
            this._profile.DigitalWalls.Remove(matchingWall);
        }

        private void SpaceSessionAdded(SpaceSessionDto spaceSession)
        {
            var matchingDigitalWall = this._profile.DigitalWalls.FirstOrDefault(w => w.WallId == spaceSession.DigitalWallId);
            this._profile.SpaceSessions.Add(SpaceSessionMapper.GetUcSpaceSessionFromUcDigitalWallAndDtoSpaceSession(matchingDigitalWall, spaceSession));
        }

        private void SpaceSessionRemoved(int spaceSessionId)
        {
            var matchingSpaceSession = this._profile.SpaceSessions.FirstOrDefault(s => s.SessionId == spaceSessionId);
            this._profile.SpaceSessions.Remove(matchingSpaceSession);
        }

        #endregion

        #endregion

        public void TryProcessStartupCache()
        {
            //If we've already processed the startup cache, we don't need to.
            if (this._startupCacheProcessed)
                return;

            //If the startup cache does not contain all the necessary data, we can't process it yet.
            if ((this._startupCache.ContainsKey(StartupCacheKey.Sources) &&
               this._startupCache.ContainsKey(StartupCacheKey.SourceInstances) &&
               this._startupCache.ContainsKey(StartupCacheKey.DigitalWalls) &&
               this._startupCache.ContainsKey(StartupCacheKey.SpaceSessions)) == false)
                return;

            this._unityMainThreadDispatcher.Enqueue(() => 
            {
                this._profile.Sources = SourceMapper.GetUcSourceListFromDtoSourceList((IEnumerable<SourceDto>)this._startupCache[StartupCacheKey.Sources]);
                var sourceInstances = SourceInstanceMapper.GetUcSourceInstanceListFromUcSourceListAndDtoSourceInstanceList(this._profile.Sources, (IEnumerable<SourceInstanceDto>)this._startupCache[StartupCacheKey.SourceInstances], this._commonValues.PixelToUnityUnitScale, this._unityMainThreadDispatcher);
                this._profile.DigitalWalls = DigitalWallMapper.GetUcDigitalWallListFromDtoDigitalWallList(sourceInstances, (IEnumerable<DigitalWallDto>)this._startupCache[StartupCacheKey.DigitalWalls], this._commonValues.PixelToUnityUnitScale, this._commonValues.WallCenter, this._unityMainThreadDispatcher);
                this._profile.SpaceSessions = SpaceSessionMapper.GetUcSpaceSessionListFromUcDigitalWallListAndDtoSpaceSessionList(this._profile.DigitalWalls, (IEnumerable<SpaceSessionDto>)this._startupCache[StartupCacheKey.SpaceSessions]);

                //If we have any digital walls, render the first.
                UcDigitalWall firstWall = this._profile.DigitalWalls.FirstOrDefault();
                if(firstWall != null)
                {
                    firstWall.Render();
                    this._profile.SelectedWall = firstWall;
                }

            });
            

            this._startupCacheProcessed = true;
        }

        public void Dispose()
        {
            Connection?.Stop();
            Connection?.Dispose();
        }

        #endregion

    }

    enum StartupCacheKey
    {
        Sources = 0,
        SourceInstances = 1,
        DigitalWalls = 2,
        SpaceSessions = 3
    }
}
