using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "levelData", menuName = "Custom Managers/Level Manager", order = 1)]
public class LevelManager : ScriptableObject
{
    public EGameMode DifficultyLevel;
    public int TeamCount;
    public int PlayerCount;
    public int PlayerControllerCount;
    public int MapWidth;
    public int MapLength;
    public FTeamData[] Teams;

    //Random Values that have no major impact on Gameplay
    private readonly string[] TeamNames = { "Coach Man", "The Boxers", "Wolf Gang", "High on Victory", "The Dementors", "The Flow Zone", "Always Benched", "Miracle Makers", "Turf Warnocks", "Restless Rockets", "Red Head Gang", "Royal Army", "Gryffindors", "Mah_Gnomies", "Traitor Joes", "Greek Gods", "Broken Bones", "Tactical Attacks", "Noob Power", "Sulking Hulks" };


    private void SetNames(int index, string name)
    {
        Teams[index].TeamName = name;
    }
    //Populate the Teams Data in the LevelDataActor 
    public void PopulateTeams()
    {
        Array.Clear(Teams, 0, Teams.Length);
        Teams = new FTeamData[TeamCount];
        for(int i = 0; i < TeamCount; ++i)
        {
            Teams[i] = new FTeamData();
            SetNames(i, TeamNames[UnityEngine.Random.Range(0, TeamNames.Length)]);
            Teams[i].PlayerControlType = i < PlayerControllerCount ? EPlayerControlled.Player : EPlayerControlled.AI;
            Teams[i].Players = new Player_Controller[PlayerCount];
            Teams[i].PlayerToPlay = 0;
            Teams[i].bHasPlayers = true;
            
        }
    }

    

    
}

