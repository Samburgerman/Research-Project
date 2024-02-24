using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static SpaceDefinitions;

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
        int n = spaceDefinitions.GetBaseSpaceCount();
        List<int> proximityInts = new() { -1,-1,-1 };
        int fromIndex = from.GetSpaceTypeIndex();
        for(int i=0; i<3;i++)
        {
            int toIndex = n+i;
            int diff = (toIndex-fromIndex)%n;
            if(diff==0)
                diff+=n;
            proximityInts[toIndex-n]=diff;
        }
        CleanProximityInts(proximityInts);
        return proximityInts;
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