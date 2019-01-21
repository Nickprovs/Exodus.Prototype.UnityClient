using Assets.Interfaces;
using Assets.Scenes.Scripts.Signals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuScript : MonoBehaviour
{
    public TMP_InputField _inputField;
    private SignalBus _signalBus;
    private IUcSignalrProxy _signalrProxy;
    private IGameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ConnectButtonClicked()
    {
        string ipAddress = this._inputField.text;
        bool ipIsValid = await this.IsValidIp(ipAddress);
        if (ipIsValid)
            this._signalrProxy.ConnectAsync(ipAddress, "9999");
        else
            this._inputField.text = "";
    }

    [Inject]
    public void Setup(SignalBus signalBus, IUcSignalrProxy signalrProxy, IGameManager gameManager)
    {
        this._signalBus = signalBus;
        this._signalrProxy = signalrProxy;
        this._gameManager = gameManager;
        this._signalBus.Subscribe<ConnectedSignal>(this.OnConnected);

    }
    private void OnConnected(ConnectedSignal args)
    {
        this._gameManager.LoadMainScene();
    }

    private async Task<bool> IsValidIp(string ipAddress)
    {
        IPHostEntry host;
        IPAddress validIp;

        try
        {
            //If the ip is avalid
            if (IPAddress.TryParse(ipAddress, out validIp))
            {
                //If the host is responsive
                host = await Dns.GetHostEntryAsync(ipAddress);
                validIp = host.AddressList.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);

                //If we have any invalid network info... return false
                if (validIp == null || host == null)
                    return false;
                //Otherwise return true
                else
                    return true;
            }
        }
        catch (SocketException)
        {
            Debug.LogError("Could not connect to host");
        }

        //If we couldn't make contact and we resulted in an exception, also return false;
        return false;
    }

}
