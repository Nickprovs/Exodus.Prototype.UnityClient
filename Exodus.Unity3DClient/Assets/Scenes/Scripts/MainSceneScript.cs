using Assets.Interfaces;
using UnityEngine;
using Zenject;

public class MainSceneScript : MonoBehaviour
{
    private IUcSignalrProxy _signalrProxy;
    private IGameManager _gameManager;


    [Inject]
    public void Setup(IGameManager gameManager, IUcSignalrProxy signalrProxy)
    {
        this._gameManager = gameManager;
        this._signalrProxy = signalrProxy;
    }

    // Start is called before the first frame update
    void Start()
    {
        this._gameManager.GameManagerStateChanged();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
