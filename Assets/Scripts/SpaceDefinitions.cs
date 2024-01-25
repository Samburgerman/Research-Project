using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDefinitions : MonoBehaviour
{
    private List<Space> spaces = new();
    [SerializeField] private GameObject spacePrefab;
    [SerializeField] private List<Material> materials;//good, bad, then netural
    [SerializeField] private List<int> moneys;//good, bad, then netural

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

    public GameObject GetSpacePrefab() { return spacePrefab; }

    public int GetSpaceCount() { return spaces.Count; }
}
public class Space : Object
{
    private int positionIndex;
    private int money;
    private Material material;

    public Space(int positionIndex,int money,GameObject gameObject,Material material)
    {
        this.positionIndex=positionIndex;
        this.money=money;
        this.material=material;
        this.SpaceGameObject=gameObject;
    }

    public int GetMoney() { return money; }

    public int GetPositionIndex() { return positionIndex; }

    public Material GetMaterial() { return material; }

    private GameObject SpaceGameObject { get; set; }

    public override string ToString()
    {
        string s0 = "money:"+money;
        string s1 = " material:"+material.name;
        string s2 = " SpaceGameObject:"+SpaceGameObject.name;
        return s0+s1+s2;
    }
}

public class ProximityValues : Object
{
    private Piece piece;
    private Space from;
    private SpaceDefinitions spaceDefinitions;
    private List<int> proximityValues;

    public ProximityValues(SpaceDefinitions spaceDefinitions,Piece piece)
    {
        this.piece=piece;
        from=piece.GetSpaceOn();
        this.spaceDefinitions=spaceDefinitions;
        proximityValues=CalculateProximityValues();
    }

    public List<int> GetProximityValues() { return proximityValues; }

    private List<int> CalculateProximityValues()
    {
        List<int> proximityInts = new();
        for(int i = 1; i<=spaceDefinitions.GetSpaceCount(); i++)
        {
            int position = i+from.GetPositionIndex();
            while(position>=spaceDefinitions.GetSpaceCount())
            { position-=spaceDefinitions.GetSpaceCount(); }
            Space to = spaceDefinitions.GetSpaceFromIndex(position);
            int distance = CalculateProximityValue(from,to);
            proximityInts.Add(distance);
        }
        return proximityInts;
    }

    private int CalculateProximityValue(Space from,Space to)
    {
        int distance = to.GetPositionIndex()-from.GetPositionIndex();
        while(distance<=0)
        { distance+=spaceDefinitions.GetSpaceCount(); }
        //Debug.Log("from: "+from+" to: "+to+" distance: "+distance);
        return distance;
    }

    public override string ToString()
    {
        string proximityValuesInts = "";
        foreach(int i in proximityValues)
            proximityValuesInts+=i.ToString()+" ";
        string msg = "Proximity values:"+proximityValuesInts+" Space from: "+from;
        return msg;
    }
}