using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    private float startingAngularPosition = 0;
    private List<GameObject> spaceGameObjects = new();

    private Vector3 GetPositionOfSpace(int spaceNumber,int spacesTotal,float radius)
    {
        float distanceBetweenSpaces = 2*Mathf.PI/spacesTotal;
        if(spaceNumber>=spacesTotal)
        {
            Debug.LogError("The space number of this space is greater than spaces total. spaceNumber: "
                +spaceNumber+" spacesTotal: "+spacesTotal);
            return Vector3.zero;
        }
        float angularPosition = startingAngularPosition+distanceBetweenSpaces*spaceNumber;
        //z=r*sin(theta) the conversion between angular and tangential quantitites
        float x = Mathf.Cos(angularPosition);
        float z = Mathf.Sin(angularPosition);
        x*=radius;
        z*=radius;
        return new Vector3(x,0,z);
    }

    public void GenerateBoard(int numSpaces,float radius)
    {
        spaceDefinitions.GenerateSpacesList();
        for(int spaceNumber = 0; spaceNumber<numSpaces; spaceNumber++)
        {
            int typesOfSpaces = spaceDefinitions.GetSpaceCount();
            int spaceIndex = spaceNumber%typesOfSpaces;
            GameObject gameObject = InstansiateSpace(GetPositionOfSpace(spaceNumber,numSpaces,radius),spaceIndex);
            spaceGameObjects.Add(gameObject);
        }
    }

    private GameObject InstansiateSpace(Vector3 position,int spaceIndex)
    {
        Space space = spaceDefinitions.GetSpaceFromIndex(spaceIndex);
        GameObject spacePrefab = spaceDefinitions.GetSpacePrefab();
        Material material = space.GetMaterial();
        List<Material> materials = new();
        materials.Add(material);
        spacePrefab.GetComponent<MeshRenderer>().SetMaterials(materials);
        //change color
        GameObject gameObject = GameObject.Instantiate(spacePrefab);
        gameObject.transform.SetParent(transform,true);
        Vector3 vector3 = new(position.x,transform.position.y,position.z);
        gameObject.transform.position=vector3;
        return gameObject;
    }

    public List<GameObject> GetSpaceGameObjects()
    { return spaceGameObjects; }

    public Vector3 GetSpaceTransformPosition(int spaceNumber)
    {
        return spaceGameObjects[spaceNumber].transform.position;
    }
}