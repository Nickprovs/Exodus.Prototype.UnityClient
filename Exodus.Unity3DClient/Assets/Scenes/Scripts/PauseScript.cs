using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityStandardAssets.Characters.FirstPerson;
using Assets.Types;
using UnityEngine.UI;
using System.Linq;

public class PauseScript : MonoBehaviour
{
    #region Fields

    private IUnityMainThreadDispatcher _dispatcher;

    private IUcProfile _profile;

    public Transform canvas;

    public Transform player;

    public Transform menu;

    public Dropdown wallDropdown;

    public GameObject buttonPrefab;

    #endregion

    [Inject]
    public void Setup(IUnityMainThreadDispatcher unityMainThreadDispatcher, IUcProfile profile)
    {
        this._dispatcher = unityMainThreadDispatcher;
        this._profile = profile;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.wallDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(this.wallDropdown);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Pause();
        }
    }

    void Pause()
    {
        if (canvas.gameObject.activeInHierarchy == false)
        {
            this.RefreshButtons();
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            player.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            player.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    void RefreshButtons()
    {
        this._dispatcher.Enqueue(() =>
        {
            RemovePauseOptions();
            AddPauseOptions();
        });
    }

    void AddPauseOptions()
    {
        if (this._profile.DigitalWalls != null)
        {
            List<Dropdown.OptionData> newOptions = new List<Dropdown.OptionData>();
            foreach (UcWall wall in this._profile.DigitalWalls)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                optionData.text = wall.Name;
                newOptions.Add(optionData);
            }
            this.wallDropdown.AddOptions(newOptions);
        }
    }

    void RemovePauseOptions()
    {
        this.wallDropdown.ClearOptions();
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {

        var selectedWallName = change.options[change.value].text;
        var selectedWall = this._profile.DigitalWalls.FirstOrDefault(w => w.Name == selectedWallName);

        if (selectedWall != null)
        {
            //Unrender the last selected wall if not null
            this._profile.SelectedWall?.Unrender();

            //Set the new selected wall
            this._profile.SelectedWall = selectedWall;

            //Render the new selected wall
            this._profile.SelectedWall.Render();
        }

    }
}
