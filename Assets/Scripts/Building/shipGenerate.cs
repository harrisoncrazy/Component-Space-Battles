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
                    shipPart.name = myShip.shipInfo[i].part_Name;

                    if (myShip.shipInfo[i].isFlippedX == true)
                    {
                        shipPart.transform.localScale = new Vector3(shipPart.transform.localScale.x * -1, shipPart.transform.localScale.y);
                    }
                    if (myShip.shipInfo[i].isFlippedY == true)
                    {
                        shipPart.transform.localScale = new Vector3(shipPart.transform.localScale.x, shipPart.transform.localScale.y * -1);
                    }
                }
            }
        }

        for (int i = 0; i < myShip.shipInfo.Count; i++)
        {
            GameObject currentPart = GameObject.Find(myShip.shipInfo[i].part_Name);

            for (int x = 0; x < myShip.shipInfo[i].partConnections.Count; x++)
            {
                GameObject slot = currentPart.transform.GetChild(x).gameObject;
                Vector3 pos = new Vector3(myShip.shipInfo[i].partConnections[x].xPos, myShip.shipInfo[i].partConnections[x].yPos);
                slot.transform.localPosition = pos;

                slot.GetComponent<InstallSlot>().connectedComponent = GameObject.Find(myShip.shipInfo[i].partConnections[x].connectedTo);
            }
        }

        emptyObj.AddComponent<shipControl>();
    }
}
