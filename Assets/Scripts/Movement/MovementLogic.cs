using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental;
using UnityEngine;
using static PlayerData;

public static class MovementLogic
{
    public static int DecideRollNumber(Piece piece)
    {
        SpaceDefinitions spaceDefinitions = piece.GetSpaceDefinitions();
        ProximityValues proximityValues = new(spaceDefinitions,piece);
        ExperimentalCondition experimentalCondition = piece.GetPlayerData().GetExperimentalCondition();
        List<float> odds = MovementProbabilities.GetProbabilitiesForCondition(experimentalCondition);
        float randomValue = UnityEngine.Random.Range(0f,1f);
        int indexOfTypeOfSpace = -1;
        float culminativeOdds = 0.0f;
        MovementProbabilities.GetProbabilitiesForCondition(experimentalCondition);
        for(int i = 0; i<odds.Count&&indexOfTypeOfSpace==-1; i++)
        {
            culminativeOdds+=odds[i];
            if(randomValue<culminativeOdds)
                indexOfTypeOfSpace=i;
        }
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