using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class MovementLogic
{
    private static List<List<float>> probabilities;
    private static float probabilityForBoosted = 1.0f;
    public static float probabilityForDeboosted = (1.0f-probabilityForBoosted)/2;

    private static void InitializeProbabilities()
    {
        //order is good, bad, neutral, then fair
        List<float> probabilitiesForPositive = GetProbabilitiesForPositive();
        List<float> probabilitiesForNegative = GetProbabilitiesForNegative();
        List<float> probabilitiesForNeutralized = GetProbabilitiesForNeutralized();
        List<float> probabilitiesForFair = GetProbabilitiesForFair();
        probabilities=new List<List<float>>
        {
            probabilitiesForPositive, probabilitiesForNegative, probabilitiesForNeutralized, probabilitiesForFair
        };
        CleanProbabilities(probabilities);
    }

    private static void CleanProbabilities(List<List<float>> probs)
    {
        foreach(List<float> probList in probs)
        {
            float sum = 0.0f;
            foreach(float prob in probList)
                sum+=prob;
            if(sum!=1.0f)
                throw new Exception("the probabilities for this list: "+probList+" is too big/small: "+sum);
        }
    }

    private static List<float> GetProbabilitiesForPositive() => new()
        {
            probabilityForBoosted,
            probabilityForDeboosted,
            probabilityForDeboosted
        };

    private static List<float> GetProbabilitiesForNegative() => new()
        {
            probabilityForDeboosted,
            probabilityForBoosted,
            probabilityForDeboosted
        };

    private static List<float> GetProbabilitiesForNeutralized() => new()
        {
            probabilityForDeboosted,
            probabilityForDeboosted,
            probabilityForBoosted
        };

    private static List<float> GetProbabilitiesForFair() => new()
        {1.0f/3,1.0f/3,1.0f/3/*these need to stay in decimals not percents*/};

    public static List<float> GetProbabilitiesForCondition(PlayerData.ExperimentalCondition experimentalCondition)
    {
        InitializeProbabilities();
        List<float> odds = experimentalCondition switch
        {
            PlayerData.ExperimentalCondition.Positive => probabilities[0],
            PlayerData.ExperimentalCondition.Negative => probabilities[1],
            PlayerData.ExperimentalCondition.Neutralized => probabilities[2],
            PlayerData.ExperimentalCondition.Fair => probabilities[3],
            _ => throw new Exception("Expeimental condition is undefined: "+experimentalCondition),
        };
        return odds;
    }

    public static int DecideMovement(Piece piece)
    {
        SpaceDefinitions spaceDefinitions = piece.GetSpaceDefinitions();
        ProximityValues proximityValues = new(spaceDefinitions,piece);
        List<float> odds = GetProbabilitiesForCondition(piece.GetPlayerData().GetExperimentalCondition());
        string oddsString = "";
        foreach(float odd in odds)
            oddsString+=odd+" ";
        //odds are correct for positive
        float randomValue = UnityEngine.Random.Range(0f,1f);
        int indexOfTypeOfSpace = -1;
        float culminativeOdds = 0.0f;
        for(int i = 0; i<probabilities[i].Count&&indexOfTypeOfSpace==-1; i++)
        {
            culminativeOdds+=odds[i];
            if(randomValue<culminativeOdds)
                indexOfTypeOfSpace=i;
        }
        //the indexOfTypeOfSpace is being decided correctly
        int rollNumber = DecideRollFromProximityValues(proximityValues,indexOfTypeOfSpace);
        Debug.Log("rollNumber: "+rollNumber+" proximityValues: "+proximityValues);
        return rollNumber;
    }

    private static int DecideRollFromProximityValues(ProximityValues proximityValues,int indexOfTypeOfSpace)
    {
        int lowRoll = proximityValues.GetProximityValues()[indexOfTypeOfSpace];
        //there are 2 same ty[e of spaces within the roll 1-6
        int newRoll = lowRoll;
        if(DecideToAdd3())
            newRoll+=3;
        return newRoll;
    }

    private static bool DecideToAdd3() =>
        //50-50 chance
        UnityEngine.Random.Range(0.0f,1.0f)>=0.5f;
}