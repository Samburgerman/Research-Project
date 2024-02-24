using UnityEngine;
using static SpaceDefinitions;

public class Space : object
{
    private int baseSpaceIndex;
    private int money;
    private Material material;
    public SpaceType SpaceType { get; private set; }

    public Space(int baseSpaceIndex, int money,GameObject gameObject,Material material)
    {
        this.baseSpaceIndex= baseSpaceIndex;
        this.money=money;
        this.material=material;
        SpaceGameObject=gameObject;
        SpaceType=ConvertIndexToSpaceType(baseSpaceIndex%3);

    }

    public int GetMoney() => money;

    public Material GetMaterial() => material;

    private GameObject SpaceGameObject { get; set; }

    public override string ToString()
    {
        string s = " SpaceType: "+SpaceType;
        return s;
        //string s0 = "money: "+money;
        //string s1 = " material: "+material.name;
        //string s2 = " SpaceType: "+SpaceType;
        //string s3 = " SpaceGameObject: "+SpaceGameObject.name;
        //return s0+s1+s2+s3;
    }
}