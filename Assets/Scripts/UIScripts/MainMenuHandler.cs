using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public enum EScreens
{
    StartScreen,
    ModeSelection,
    GameSelection
};

public class MainMenuHandler : MonoBehaviour
{
    //Variables to Handle Window Switching
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject GameSelection;
    public LevelManager CurrentLevelData;
    public TextMeshProUGUI TeamCountLabel;
    public TextMeshProUGUI PlayerCountLabel;

    

    public void Start()
    {
        SelectScreen(0);
        CurrentLevelData.TeamCount = 2;
        CurrentLevelData.PlayerControllerCount = 2;
        CurrentLevelData.PlayerCount = 1;

    }
    public void SelectScreen(int InScreen)
    {
        if(StartScreen)
        {
            StartScreen.SetActive(EScreens.StartScreen == (EScreens)InScreen);
        }
        
        if(GameSelection)
        {
            GameSelection.SetActive(EScreens.GameSelection == (EScreens)InScreen);
        }
        
    }


    public void SetTeamCount(Slider InSlider)
    {
        CurrentLevelData.TeamCount = (int)InSlider.value;
        CurrentLevelData.PlayerControllerCount = (int)InSlider.value;
        TeamCountLabel.text = ((int)InSlider.value).ToString();
    }

    public void SetPlayerCount(Slider InSlider)
    {
        CurrentLevelData.PlayerCount = (int)InSlider.value;
        PlayerCountLabel.text = ((int)InSlider.value).ToString();
    }

    public void OpenScene(string InName)
    {
        SceneManager.LoadScene(InName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
