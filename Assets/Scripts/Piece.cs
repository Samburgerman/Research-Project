using UnityEngine;
using System;

public class Piece : MonoBehaviour
{
    private PlayerData playerData;
    private SpaceDefinitions spaceDefinitions;

    public void InitializePieceFields(PlayerData playerData,PieceComponentRefrences refrences)
    {
        this.playerData=playerData;
        spaceDefinitions=refrences.spaceDefinitions;
    }

    public PlayerData GetPlayerData() { return playerData; }

    public Space GetSpaceOn()
    {
        int spaceNumber = GetPlayerData().spaceNumber;
        Space space = spaceDefinitions.GetSpaceFromIndex(spaceNumber);
        return space;
    }

    public void AdjustSpaces(int toAdd) { playerData.AdjustSpace(toAdd,GameManager.TotalSpaces); }

    public void AdjustMoney(int toAdd) { playerData.AdjustMoney(toAdd); }

    public SpaceDefinitions GetSpaceDefinitions() { return spaceDefinitions; }
}

[System.Serializable]
public struct PlayerData
{
    public int playerIndex;
    public int money;
    public int spaceNumber;
    public ExperimentalCondition experimentalCondition;

    public enum ExperimentalCondition
    {
        Positive,
        Negative,
        Neutralized,
        Fair
    }

    public static ExperimentalCondition GetExperimentalConditionFromIndex(int i)
    {
        switch(i)
        {
            case 0:
                return ExperimentalCondition.Positive;
            case 1:
                return ExperimentalCondition.Positive;
            case 2:
                return ExperimentalCondition.Positive;
            case 3:
                return ExperimentalCondition.Positive;
            default:
                throw new Exception("Out of bounds: "+i);
        }
    }

    public PlayerData(int playerIndex,
                      int spaceNumeber,
                      int money,
                      ExperimentalCondition experimentalCondition)
    {
        this.playerIndex=playerIndex;
        this.money=money;
        spaceNumber=spaceNumeber;
        this.experimentalCondition=experimentalCondition;
    }

    public void AdjustSpace(int toAdd,int totalSpaces)
    {
        spaceNumber+=toAdd;
        //we need to handle when a space number is too high
        //ex space 15 should be written as 3 if there are 12 spaces
        while(spaceNumber>=totalSpaces)
            spaceNumber-=totalSpaces;
    }

    public void AdjustMoney(int toAdd) { money+=toAdd; }

    public ExperimentalCondition GetExperimentalCondition() { return experimentalCondition; }

    public override string ToString()
    {
        string s1 = "Player Index: "+playerIndex;
        string s2 = " Money: "+money;
        string s3 = " Space Number: "+spaceNumber;
        string s4 = " Experimental Condition: "+experimentalCondition;
        return s1+s2+s3+s4;
    }
}

public struct PieceComponentRefrences
{
    public SpaceDefinitions spaceDefinitions;
}