using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    private float startingAngularPosition = 0;
    public List<GameObject> SpaceGameObjects { get; private set; } = new();

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

    private static float GetDistanceBetweenSpaces(int spacesTotal)
    {return 2*Mathf.PI/spacesTotal;}

    public void GenerateBoard(int numSpaces,float radius)
    {
        spaceDefinitions.GenerateSpacesList();
        for(int spaceNumber = 0; spaceNumber<numSpaces; spaceNumber++)
        {
            int typesOfSpaces = spaceDefinitions.GetSpaceCount();
            int spaceIndex = spaceNumber%typesOfSpaces;
            GameObject gameObject = InstansiateSpace(GetPositionOfSpace(spaceNumber,numSpaces,radius),spaceIndex);
            SpaceGameObjects.Add(gameObject);
        }
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
        List<Material> materials = new(){material};
        spaceGameObject.GetComponent<MeshRenderer>().SetMaterials(materials);
    }

    public List<GameObject> GetSpaceGameObjects()
    { return SpaceGameObjects; }

    public Vector3 GetSpaceTransformPosition(int spaceNumber)
    { return SpaceGameObjects[spaceNumber].transform.position; }
}