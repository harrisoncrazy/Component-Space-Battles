using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class StoredPrefab
{
    public GameObject prefab;
    public string prefabID;
}

public class shipGenerate : MonoBehaviour
{
    public List<StoredPrefab> partPrefabs;

    [SerializeField] private TextAsset jsonObj;

    //HAVE TO USE A CONTAINER IN ORDER TO CORRECTLY PARSE A JSON FILE
    public infoShipListContainer myShip = new infoShipListContainer();

    public void Start()
    {
        //parsing JSON to string, data path
        string jsonText = File.ReadAllText(Application.dataPath + "/Ship.json");
        //String with JSON included, container with the class inside
        JsonUtility.FromJsonOverwrite(jsonText, myShip);

        GenerateShip();
    }

    public void GenerateShip()
    {
        GameObject emptyObj = new GameObject();
        emptyObj.name = "shipBase";

        for (int i = 0; i < myShip.shipInfo.Count; i++)
        {
            for (int x = 0; x < partPrefabs.Count; x++)
            {
                if (myShip.shipInfo[i].part_ID == partPrefabs[x].prefabID)
                {
                    GameObject shipPart = Instantiate(partPrefabs[x].prefab, transform.position, transform.rotation) as GameObject;
                    shipPart.transform.SetParent(emptyObj.transform);
                }
            }
        }

        for (int i = 0; i < myShip.shipInfo.Count; i++)
        {
            
        }
    }
}
