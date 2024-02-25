using System.Collections.Generic;

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
        int fromIndex = SpaceDefinitions.ConvertSpaceTypeToIndex(from.SpaceType);
        for(int i = 0; i<3; i++)
        {
            int toIndex = i;
            if(toIndex<=fromIndex)
                toIndex+=3;
            proximityInts[i]=toIndex-fromIndex;
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