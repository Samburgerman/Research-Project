using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Space;
using static SpaceDefinitions;

public class SpaceDefinitions : MonoBehaviour
{
    private List<Space> spaces = new();
    [SerializeField] private GameObject spacePrefab;
    [SerializeField] private List<Material> materials;//good, bad, then netural
    [SerializeField] private List<int> moneys;//good, bad, then netural

    public enum SpaceType
    {
        Good, Bad, Neutral
    }

    public void GenerateSpacesList()
    {
        for(int i = 0; i<moneys.Count; i++)
        {
            Space toAdd = new(i,moneys[i],spacePrefab,materials[i]);
            spaces.Add(toAdd);
        }
    }

    public Space GetSpaceFromIndex(int spaceNumber)
    {
        int spaceTypeIndex = spaceNumber%spaces.Count;
        Space space = spaces[spaceTypeIndex];
        return space;
    }

    public GameObject GetSpacePrefab() => spacePrefab;

    public int GetSpaceCount() => spaces.Count;

    public static SpaceType ConvertIntToSpaceType(int i)
    {
        return i switch
        {
            0 => SpaceType.Good,
            1 => SpaceType.Bad,
            2 => SpaceType.Neutral,
            _ => throw new System.IndexOutOfRangeException(),
        };
    }

    public static int ConvertSpaceTypeToInt(SpaceType spaceType)
    {
        return spaceType switch
        {
            SpaceType.Good => 0,
            SpaceType.Bad => 1,
            SpaceType.Neutral => 2,
            _ => throw new System.Exception("No such spaceType: "+spaceType+" can be converted."),
        };
    }
}
public class Space : Object
{
    private int positionIndex;
    private int money;
    private Material material;
    public SpaceType SpaceType { get; private set; }

    public Space(int positionIndex,int money,GameObject gameObject,Material material)
    {
        this.positionIndex=positionIndex;
        this.money=money;
        this.material=material;
        SpaceGameObject=gameObject;
        SpaceType=ConvertIntToSpaceType(positionIndex%3);
    }

    public int GetMoney() => money;

    public int GetPositionIndex() => positionIndex;

    public Material GetMaterial() => material;

    private GameObject SpaceGameObject { get; set; }

    public override string ToString()
    {
        string s0 = "money: "+money;
        string s1 = " material: "+material.name;
        string s2 = " SpaceType: "+SpaceType;
        string s3 = " SpaceGameObject: "+SpaceGameObject.name;
        return s0+s1+s2+s3;
    }
}

public class ProximityValues : Object
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
        List<int> proximityInts = new()
        {
            -1,
            -1,
            -1
        };
        int fromIndex = from.GetPositionIndex();
        int spaceIndex = fromIndex;
        for(int i=0;i<3;i++)
        {
            spaceIndex=IncrementSpaceNum(spaceIndex);
            Space examining=spaceDefinitions.GetSpaceFromIndex(spaceIndex);
            int indexOfSpaceType = SpaceDefinitions.ConvertSpaceTypeToInt(examining.SpaceType);
            proximityInts[indexOfSpaceType]=spaceIndex;
        }
        CleanProximityInts(proximityInts);
        return proximityInts;
    }

    private static int IncrementSpaceNum(int fromSpaceIndex)
    {
        fromSpaceIndex++;
        while(fromSpaceIndex>=GameManager.TotalSpaces)
            fromSpaceIndex-=GameManager.TotalSpaces;
        return fromSpaceIndex;
    }

    private static void CleanProximityInts(List<int> proximityInts)
    {
        string msg = "";
        int i=0;
        foreach(int prox in proximityInts)
        {
            if(prox>=4||prox<=-1)
                msg+="Invalid proximity int of "+prox+" at positon i="+i+". ";
            i++;
        }
        if(msg.Length>=1)
            throw new System.Exception(msg);
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