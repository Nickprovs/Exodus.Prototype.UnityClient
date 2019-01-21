using Assets.Interfaces;
using Assets.Scenes.Scripts.Signals;
using Assets.Utilities;
using UnityEngine;
using Zenject;

public class DefaultInstaller : MonoInstaller<DefaultInstaller>
{
    public override void InstallBindings()
    {
        //Signals
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ConnectedSignal>();
        Container.DeclareSignal<DisconnectedSignal>();
        Container.DeclareSignal<ReconnectingSignal>();
        Container.DeclareSignal<ConnectingSignal>();


        //Utilities
        Container.Bind<IGameManager>().To<GameManager>().AsSingle();
        Container.Bind<IUcSignalrProxy>().To<UcSignalrProxy>().AsSingle();
        Container.Bind<IUcProfile>().To<UcProfile>().AsSingle();
        Container.Bind<ICommonValues>().To<CommonValues>().AsSingle();
        Container.Bind<IUnityMainThreadDispatcher>().To<UnityMainThreadDispatcher>().FromNewComponentOnNewGameObject().AsSingle();
    }
}