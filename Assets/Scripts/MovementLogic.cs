using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class MovementLogic
{
    private static List<List<float>> probabilities;
    private static float probabilityForBoosted = 0.5f;
    public static float probabilityForDeboosted = (1.0f-probabilityForBoosted)/2;

    private static void InitializeProbabilities()
    {
        float probabilityForDeboosted = (1-probabilityForBoosted)/2;
        //order is good, bad, then neutral
        List<float> probabilitiesForPositive = GetProbabilitiesForPositive();
        List<float> probabilitiesForNegative = GetProbabilitiesForNegative();
        List<float> probabilitiesForNeutralized = GetProbabilitiesForNeutralized();
        List<float> probabilitiesForFair = GetProbabilitiesForFair();
        probabilities=new List<List<float>>
        {
            probabilitiesForPositive, probabilitiesForNegative, probabilitiesForNeutralized, probabilitiesForFair
        };
    }

    private static List<float> GetProbabilitiesForPositive()
    {
        return new()
        {
            probabilityForBoosted,
            probabilityForDeboosted,
            probabilityForDeboosted
        };
    }

    private static List<float> GetProbabilitiesForNegative()
    {
        return new()
        {
            probabilityForDeboosted,
            probabilityForBoosted,
            probabilityForDeboosted
        };
    }

    private static List<float> GetProbabilitiesForNeutralized()
    {
        return new()
        {
            probabilityForDeboosted,
            probabilityForDeboosted,
            probabilityForBoosted
        };
    }

    private static List<float> GetProbabilitiesForFair()
    {
        return new()
        {
            1.0f/3,1.0f/3,1.0f/3//these need to stay in decimals not percents
        };
    }

    public static List<float> GetProbabilitiesForCondition(PlayerData.ExperimentalCondition experimentalCondition)
    {
        InitializeProbabilities();
        List<float> odds;
        switch(experimentalCondition)
        {
            case PlayerData.ExperimentalCondition.Positive:
                odds=probabilities[0];
                break;
            case PlayerData.ExperimentalCondition.Negative:
                odds=probabilities[1];
                break;
            case PlayerData.ExperimentalCondition.Neutralized:
                odds=probabilities[2];
                break;
            case PlayerData.ExperimentalCondition.Fair:
                odds=probabilities[3];
                break;
            default:
                throw new Exception("Expeimental condition is undefined: "+experimentalCondition);
        }
        return odds;
    }

    public static int DecideMovement(Piece piece)
    {
        SpaceDefinitions spaceDefinitions = piece.GetSpaceDefinitions();
        ProximityValues proximityValues = new(spaceDefinitions,piece);
        //ProximityValues are being returned as -1,0,0 no matter what; this is where the problem is
        List<float> odds = GetProbabilitiesForCondition(piece.GetPlayerData().GetExperimentalCondition());
        //odds are being returned correctly
        float randomValue = UnityEngine.Random.Range(0f,1f);
        //random value is correct
        int indexOfTypeOfSpace = -1;
        float culminativeOdds = 0.0f;
        for(int i = 0; i<probabilities[i].Count&&indexOfTypeOfSpace==-1; i++)
        {
            culminativeOdds+=odds[i];
            if(randomValue<culminativeOdds)
            {
                indexOfTypeOfSpace=i;
                //Debug.Log("At this point, the loop should terminate. i="+i);
            }
        }
        /*Debug.Log("Culminative odds: "+culminativeOdds+
            " randomValue: "+randomValue+
            " indexOfTypeOfSpace: "+indexOfTypeOfSpace+
            " proximityValues: "+proximityValues.ToString());*/
        int roll = proximityValues.GetProximityValues()[indexOfTypeOfSpace];
        return roll;
    }
}