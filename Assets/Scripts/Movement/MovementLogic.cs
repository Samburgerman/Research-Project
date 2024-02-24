using System.Collections.Generic;
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
        float randomValue = Random.Range(0f,1f);
        int indexOfTypeOfSpaceToLandOn = -1;
        float culminativeOdds = 0.0f;
        MovementProbabilities.GetProbabilitiesForCondition(experimentalCondition);
        for(int i = 0; i<odds.Count&&indexOfTypeOfSpaceToLandOn==-1; i++)
        {
            culminativeOdds+=odds[i];
            if(randomValue<culminativeOdds)
                indexOfTypeOfSpaceToLandOn=i;
        }
        int rollNumber = DecideRollFromProximityValuesAndSpaceIndex(proximityValues,indexOfTypeOfSpaceToLandOn);
        Debug.Log("rollNumber: "+rollNumber+" proximityValues: "+proximityValues);
        return rollNumber;
    }

    private static int DecideRollFromProximityValuesAndSpaceIndex(ProximityValues proximityValues,int indexOfTypeOfSpaceToLandOn)
    {
        int lowRoll = proximityValues.GetProximityValues()[indexOfTypeOfSpaceToLandOn];
        //there are 2 same type of spaces within the roll 1-6, so randomly choose between them
        int newRoll = lowRoll;
        if(DecideToAdd3())
            newRoll+=3;
        return newRoll;
    }

    private static bool DecideToAdd3() =>
        //50-50 chance
        Random.Range(0.0f,1.0f)>=0.5f;
}