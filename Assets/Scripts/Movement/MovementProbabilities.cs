using System.Collections.Generic;
using UnityEngine;

internal static class MovementProbabilities
{
    private static readonly float probabilityForBoosted = 1.0f;//todo change to 0.5-0.7 when prox value are correct
    private static readonly float probabilityForDeboosted = (1.0f-probabilityForBoosted)/2;
    private static List<List<float>> probabilities;

    public static List<float> GetProbabilitiesForCondition(PlayerData.ExperimentalCondition experimentalCondition)
    {
        InitializeProbabilities();
        List<float> odds = experimentalCondition switch
        {
            PlayerData.ExperimentalCondition.Positive => probabilities[0],
            PlayerData.ExperimentalCondition.Negative => probabilities[1],
            PlayerData.ExperimentalCondition.Neutralized => probabilities[2],
            PlayerData.ExperimentalCondition.Fair => probabilities[3],
            _ => throw new System.Exception("Expeimental condition is undefined: "+experimentalCondition),
        };
        return odds;
    }

    private static void CleanProbabilities(List<List<float>> probs)
    {
        foreach(List<float> probList in probs)
        {
            float sum = 0.0f;
            foreach(float prob in probList)
                sum+=prob;
            if(sum!=1.0f)
            {
                string probListString = "";
                foreach(float prob in probList)
                    probListString += prob+" ";
                Debug.Log("probabilityForBoosted: "+probabilityForBoosted);
                Debug.Log("probabilityForDeBoosted: "+probabilityForDeboosted);
                throw new System.Exception("The probabilities for this list: "+probListString+" are too big/small: "+sum);
            }
        }
    }

    private static List<float> GetProbabilitiesForFair() => new()
        {1.0f/3,1.0f/3,1.0f/3/*these need to stay in decimals not percents*/};

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

    private static List<float> GetProbabilitiesForPositive() => new()
        {
            probabilityForBoosted,
            probabilityForDeboosted,
            probabilityForDeboosted
        };

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
}