using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static PlayerData;

public class Piece : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerData playerData;
    private SpaceDefinitions spaceDefinitions;

    public void InitializePiece(
        GameManager gameManager,
        SpaceDefinitions spaceDefinitions,
        int playerIndex,
        ExperimentalCondition experimentalCondition,
        int startPosition,
        int startMoney)
    {
        this.gameManager=gameManager;
        this.spaceDefinitions=spaceDefinitions;
        playerData=new PlayerData(gameManager,playerIndex,startMoney,startPosition,experimentalCondition);
    }

    public PlayerData GetPlayerData() { return playerData; }

    public Space GetSpaceOn()
    {
        int spaceNumber = GetPlayerData().GetSpaceNumber();
        Space space = spaceDefinitions.GetSpaceFromIndex(spaceNumber);
        return space;
    }

    public void AdjustSpaces(int toAdd) { playerData.AdjustSpace(toAdd); }

    public void AdjustMoney(int toAdd) { playerData.AdjustMoney(toAdd); }

    public SpaceDefinitions GetSpaceDefinitions() { return spaceDefinitions; }
}

[System.Serializable]
public struct PlayerData
{
    GameManager gameManager;
    private int playerIndex;
    private int money;
    private int spaceNumber;
    ExperimentalCondition experimentalCondition;

    public enum ExperimentalCondition
    {
        Positive,
        Negative,
        Neutralized,
        Fair
    }

    public PlayerData(GameManager gameManager,
                      int playerIndex,
                      int spaceNumeber,
                      int money,
                      ExperimentalCondition experimentalCondition)
    {
        this.gameManager = gameManager;
        this.playerIndex=playerIndex;
        this.money=money;
        this.spaceNumber=spaceNumeber;
        this.experimentalCondition=experimentalCondition;
    }

    public int GetPlayerIndex() { return playerIndex; }

    public int GetMoney() { return money; }

    public int GetSpaceNumber() { /*Debug.Log("Im on the space #: "+spaceNumber);*/ return spaceNumber; }

    public void AdjustMoney(int toAdd) { money+=toAdd; }

    public void AdjustSpace(int toAdd)
    {
        spaceNumber+=toAdd;
        //we need to handle when a space number is too high
        //ex 15 should be written as 3 if there are 12 spaces
        while(spaceNumber>=gameManager.GetTotalSpaces())
        {
            spaceNumber-=gameManager.GetTotalSpaces();
        }
    }

    public ExperimentalCondition GetExperimentalCondition() { return experimentalCondition; }

    public PlayerData Clone()
    {
        PlayerData playerData = new PlayerData(playerIndex,
                                               spaceNumber,
                                               money,
                                               experimentalCondition);
        return playerData;
    }

    public override string ToString()
    {
        string s1 = "Player Index: "+playerIndex;
        string s2 = " Money: "+money;
        string s3 = " Space Number: "+spaceNumber;
        string s4 = " Experimental Condition: "+experimentalCondition;
        return s1+s2+s3+s4;
    }
}