using System.Collections.Generic;
using UnityEngine;
using static SpaceDefinitions;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    [SerializeField] private float spaceRadius = 5;
    [SerializeField] Vector3 spaceScale = Vector3.one;
    private static readonly float startingAngularPosition = 0;
    private static float GetDistanceBetweenSpaces(int spacesTotal) => 2*Mathf.PI/spacesTotal;

    public List<GameObject> SpaceGameObjects { get; private set; } = new();

    public void GenerateBoard(int numSpaces)
    {
        spaceDefinitions.GenerateSpacesList();
        for(int i = 0; i<numSpaces; i++)
        {
            int typesOfSpaces = spaceDefinitions.GetBaseSpaceCount();
            int spaceIndex = i%typesOfSpaces;
            GameObject spaceGameObject = InstansiateSpace(GetPositionOfSpace(i,numSpaces,spaceRadius),spaceIndex);
            Resize(spaceGameObject,spaceScale);
            SpaceGameObjects.Add(spaceGameObject);
        }
    }

    private Vector3 GetPositionOfSpace(int spaceNumber,int spacesTotal,float radius)
    {
        float distanceBetweenSpaces = GetDistanceBetweenSpaces(spacesTotal);
        if(spaceNumber>=spacesTotal)
        {
            Debug.LogError("The space number of this space is greater than spaces total. spaceNumber: "
                +spaceNumber+" spacesTotal: "+spacesTotal);
            return Vector3.zero;
        }
        float angularPosition = startingAngularPosition+distanceBetweenSpaces*spaceNumber;
        //z=r*sin(theta) the conversion between angular and tangential quantitites
        return CalculatePositionFromAngularPosition(radius,angularPosition);
    }

    private static Vector3 CalculatePositionFromAngularPosition(float radius,float angularPosition)
    {
        float x = Mathf.Cos(angularPosition);
        float z = Mathf.Sin(angularPosition);
        x*=radius;
        z*=radius;
        Vector3 result = new(x,0,z);
        return result;
    }

    private static void Resize(GameObject gameObject,Vector3 scale)
    {
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x*=scale.x;
        localScale.y*=scale.y;
        localScale.z*=scale.z;
        gameObject.transform.localScale=localScale;
    }

    private GameObject InstansiateSpace(Vector3 position,int spaceIndex)
    {
        GameObject spacePrefab = DefineSpacePrefab(spaceIndex);
        GameObject spaceGameObject = Instantiate(spacePrefab);
        SetSpaceGameObjectPosition(position,spaceGameObject);
        return spaceGameObject;
    }

    private void SetSpaceGameObjectPosition(Vector3 position,GameObject spaceGameObject)
    {
        spaceGameObject.transform.SetParent(transform,true);
        Vector3 vector3 = new(position.x,transform.position.y,position.z);
        spaceGameObject.transform.position=vector3;
    }

    private GameObject DefineSpacePrefab(int spaceIndex)
    {
        Space space = spaceDefinitions.GetSpaceFromIndex(spaceIndex);
        GameObject spacePrefab = spaceDefinitions.GetSpacePrefab();
        SetSpaceMaterial(space,spacePrefab);
        return spacePrefab;
    }

    private static void SetSpaceMaterial(Space space,GameObject spaceGameObject)
    {
        Material material = space.GetMaterial();
        List<Material> materials = new() { material };
        spaceGameObject.GetComponent<MeshRenderer>().SetMaterials(materials);
    }

    public Vector3 GetSpaceTransformPosition(int spaceNumber) => SpaceGameObjects[spaceNumber].transform.position;

    //public SpaceType GetSpaceTypeFromPositionIndex(int i)
    //{
    //    int indexOfTypeOfSpace = i%spaceDefinitions.GetBaseSpaceCount();
    //    return ConvertIndexToSpaceType(indexOfTypeOfSpace);
    //}
}