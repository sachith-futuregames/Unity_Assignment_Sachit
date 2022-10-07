using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum EGameMode
{
    SinglePlayer,
    M_Beginner,
    M_Advanced
};

public enum EPlayerIndex
{
    Player_One,
    Player_Two,
    Player_Three,
    Player_Four
};

public enum ETeamIndex
{
    Team_One,
    Team_Two,
    Team_Three,
    Team_Four
};

public enum EPlayerControlled
{
    Player,
    AI
};

public enum EWeaponName
{
    Bomb,
    AirStrike
}

[System.Serializable]
public struct FLevelData
{
    public List<FTeamData> TeamData;
};

[System.Serializable]
public struct FTeamData
{
    public string TeamName;
    public Player_Controller[] Players;
    public EPlayerControlled PlayerControlType;
    public int PlayerToPlay;
    public bool bHasPlayers;
};

[System.Serializable]
public struct FModeLevelPair
{
    public EGameMode Key;
    public LevelManager Value;
    public GameObject PlayerPreFab;
}
public class GlobalGameManager : MonoBehaviour
{
    //Variables that bring in choices of Gameplay
    public List<FModeLevelPair> LevelDataObjects;
    //Variables that show the current States of the gameplay
    public EGameMode GameMode;
    public LevelManager CurrentLevelData;
    public GameObject CharacterPrefab;
    public int ActiveTeamIndex = 0;
    public int ActivePlayerIndex= 0;
    //DataTables
    public List<WeaponsManager> WeaponList;
    public List<Mesh> TeamMeshes;

    //SubManagers
    public ActiveCameraManager cameraManager;


    public void Start()
    {
        PopulateGame();
        NextPlayer();
        SetActiveStatus(true);
        cameraManager.SetActiveTPPCamera(CurrentLevelData.Teams[ActiveTeamIndex].Players[ActivePlayerIndex].TPPCameraTarget.transform);
    }
    //Sets the game mode and the Level Data Based on the users Choice
    public void SetGameMode(EGameMode InGameMode)
    {
        GameMode = InGameMode;
        CurrentLevelData = LevelDataObjects[(int)InGameMode].Value;
        CharacterPrefab = LevelDataObjects[(int)InGameMode].PlayerPreFab;
        
    }

    public void PopulateGame()
    {
        CurrentLevelData.PopulateTeams();
        GameObject PlayerLayer = GameObject.Find("Players");
        for(int i = 0; i < CurrentLevelData.TeamCount; ++i)
        {
            for(int j = 0; j < CurrentLevelData.PlayerCount; ++j)
            {
                GameObject Player = Instantiate(CharacterPrefab, new Vector3(i * 10, 50, j * 10), Quaternion.identity);
                Player.transform.SetParent(PlayerLayer.transform);
                Player.GetComponent<Player_Controller>().SetPlayerMesh(TeamMeshes[i]);
                Player.GetComponent<Player_Controller>().TeamID = i;
                CurrentLevelData.Teams[i].Players[j] = Player.GetComponent<Player_Controller>();
                CurrentLevelData.Teams[i].Players[j].GlobalManager = this;
                
                
            }
        }
    }

    public void SetActiveStatus(bool InStatus)
    {
        CurrentLevelData.Teams[ActiveTeamIndex].Players[ActivePlayerIndex].bIsActive = InStatus;
        
    }

    public void NextTurn()
    {

        SetActiveStatus(false);
        bool _check = CheckPlayers(ActiveTeamIndex);
        NextTeam();
        SetActiveStatus(true);
        cameraManager.SetActiveTPPCamera(CurrentLevelData.Teams[ActiveTeamIndex].Players[ActivePlayerIndex].TPPCameraTarget.transform);
    }

    public void NextTeam()
    {
        ++ActiveTeamIndex;
        ActiveTeamIndex %= CurrentLevelData.TeamCount;
        if (CheckPlayers(ActiveTeamIndex))
        {
            NextPlayer();
        }
        else
        {
            NextTeam();
        }
        if(!CheckTeams())
        {
            EndGame();
            SceneManager.LoadScene("GameEnd");
        }


       
        
       
    }

    public void NextPlayer()
    {
        ActivePlayerIndex = CurrentLevelData.Teams[ActiveTeamIndex].PlayerToPlay++;
        CurrentLevelData.Teams[ActiveTeamIndex].PlayerToPlay %= CurrentLevelData.Teams[ActiveTeamIndex].Players.Length;
        if(!CurrentLevelData.Teams[ActiveTeamIndex].Players[ActivePlayerIndex].bIsAlive)
        {
            NextPlayer();
        }
    }

    public bool CheckTeams()
    {
        int AliveTeams = CurrentLevelData.TeamCount;
        foreach(var Team in CurrentLevelData.Teams)
        {
            AliveTeams -= Team.bHasPlayers ? 0 : 1;
        }
        return AliveTeams > 1;
    }

    public bool CheckPlayers(int TeamIndex)
    {
        bool CheckFlag = false;
        foreach(var Player in CurrentLevelData.Teams[TeamIndex].Players)
        {
            if (Player.bIsAlive)
            {
                CheckFlag = true;
                break;
            }
        }
        CurrentLevelData.Teams[TeamIndex].bHasPlayers = CheckFlag;
        return CheckFlag;
    }

    public void TestFunction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            NextTurn();
        }

    }

    public void EndGame()
    {
        for (int i = 0; i < CurrentLevelData.TeamCount; ++i)
        {
            for (int j = 0; j < CurrentLevelData.PlayerCount; ++j)
            {
                CurrentLevelData.Teams[i].Players[j].playerInputActions.Disable();


            }
        }
    }


}
