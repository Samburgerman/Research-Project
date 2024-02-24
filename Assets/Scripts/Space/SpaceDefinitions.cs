using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Space;

public class SpaceDefinitions : MonoBehaviour
{
    private List<Space> baseSpaces = new();
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
            Space toAdd = new(moneys[i],spacePrefab,materials[i]);
            baseSpaces.Add(toAdd);
        }
    }

    public Space GetSpaceFromIndex(int spaceNumber)
    {
        int spaceTypeIndex = spaceNumber%baseSpaces.Count;
        Space space = baseSpaces[spaceTypeIndex];
        return space;
    }

    public GameObject GetSpacePrefab() => spacePrefab;

    public int GetBaseSpaceCount() => baseSpaces.Count;

    public static SpaceType ConvertIndexToSpaceType(int i)
    {
        return i switch
        {
            0 => SpaceType.Good,
            1 => SpaceType.Bad,
            2 => SpaceType.Neutral,
            _ => throw new System.IndexOutOfRangeException(),
        };
    }

    public static int ConvertSpaceTypeToIndex(SpaceType spaceType)
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