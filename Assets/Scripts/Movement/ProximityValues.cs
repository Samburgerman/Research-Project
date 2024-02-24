using System.Collections.Generic;
using UnityEngine;

public class ProximityValues : object
{
    private Space from;
    private SpaceDefinitions spaceDefinitions;
    private List<int> proximityValues;

    public ProximityValues(SpaceDefinitions spaceDefinitions,Piece piece)
    {
        from=piece.GetSpaceOn();
        this.spaceDefinitions=spaceDefinitions;
        proximityValues=CalculateProximityValues();
    }

    public List<int> GetProximityValues() => proximityValues;

    private List<int> CalculateProximityValues()
    {
        List<int> proximityInts = new() { -1,-1,-1 };
        int fromIndex = from.GetPositionIndex();
        int spaceIndex = fromIndex;
        for(int i = 0; i<3; i++)
            AddProximityValue(spaceIndex+i,proximityInts,spaceDefinitions);
        CleanProximityInts(proximityInts);
        return proximityInts;
    }

    private static void AddProximityValue(int spaceIndex, List<int> proximityInts,SpaceDefinitions spaceDefinitions)
    {
        spaceIndex=IncrementSpaceNum(spaceIndex);
        Space examining = spaceDefinitions.GetSpaceFromIndex(spaceIndex);
        int indexOfSpaceType = SpaceDefinitions.ConvertSpaceTypeToInt(examining.SpaceType);
        spaceIndex=AdjustSpaceIndexTo13Range(spaceIndex);
        proximityInts[indexOfSpaceType]=spaceIndex;
    }

    private static int AdjustSpaceIndexTo13Range(int spaceIndex)
    {
        spaceIndex=ReduceToThreshold(spaceIndex,3);
        if(spaceIndex==0)
            spaceIndex+=3;
        return spaceIndex;
    }

    private static int IncrementSpaceNum(int fromSpaceIndex)
    {
        fromSpaceIndex++;
        fromSpaceIndex=ReduceSpaceIndex(fromSpaceIndex);
        return fromSpaceIndex;
    }

    private static int ReduceSpaceIndex(int spaceIndex)
    {
        return ReduceToThreshold(spaceIndex,GameManager.NumSpaces);
    }

    private static int ReduceToThreshold(int current,int count)
    {
        while(current>=count)
        {
            current-=count;
        }
        return current;
    }

    private static void CleanProximityInts(List<int> proximityInts)
    {
        string msg = "";
        int i = 0;
        foreach(int prox in proximityInts)
        {
            if(prox>=4||prox<=-1)
                msg+="Invalid proximity int of "+prox+" at positon i="+i+". ";
            i++;
        }
        if(msg.Length>=1)
        {
            string proxIntsString = "";
            foreach(int prox in proximityInts)
            { proxIntsString+=prox+" "; }
            throw new System.Exception("Proximity ints of "+proxIntsString+" are invalid: "+msg);
        }
    }

    public override string ToString()
    {
        string proximityValuesInts = "";
        foreach(int i in proximityValues)
            proximityValuesInts+=i.ToString()+" ";
        string msg = "Proximity values: "+proximityValuesInts+" Space from: "+from;
        return msg;
    }
}