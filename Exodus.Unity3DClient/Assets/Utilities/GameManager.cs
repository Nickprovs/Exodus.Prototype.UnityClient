using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Utilities
{
    public class GameManager : IGameManager
    {
        private SignalBus _signalBus;
        private IUcSignalrProxy _signalrProxy;
        private IUnityMainThreadDispatcher _unityMainThreadDispatcher;

        public void GameManagerStateChanged()
        {
            Debug.Log("GameManagerStateChanged was called");
        }

        public void LoadMainScene()
        {
            this._unityMainThreadDispatcher.Enqueue(()=>
            {
                SceneManager.LoadScene("MainScene");
            });
        }

        [Inject]
        public void Setup(SignalBus signalBus, IUcSignalrProxy signalrProxy, IUnityMainThreadDispatcher unityMainThreadDispatcher)
        {
            this._signalBus = signalBus;
            this._signalrProxy = signalrProxy;
            this._unityMainThreadDispatcher = unityMainThreadDispatcher;
        }

    }
}
