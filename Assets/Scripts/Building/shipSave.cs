using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

/*XXXXXXXXXXX START OF WRITING SHIP XXXXXXXXXXX*/
[Serializable]
public class partShipInfo
{
    public string part_ID;
    public string part_Name;
    public bool isFlippedX;
    public bool isFlippedY;
    public List<PartConnectionPoints> partConnections;
}

[Serializable]
public class PartConnectionPoints
{
    public string type;
    public float xPos;
    public float yPos;
    public string connectedTo;
}

[Serializable]
public class infoShipListContainer
{
    public List<partShipInfo> shipInfo;
}
/*XXXXXXXXXXX END OF WRITING SHIP XXXXXXXXXXX*/

public class partTracker
{
    public string partName;
    public int numOfType;
}

public class shipSave : MonoBehaviour
{
    [SerializeField] private TextAsset jsonObj;
    //HAVE TO USE A CONTAINER IN ORDER TO CORRECTLY PARSE A JSON FILE
    public infoShipListContainer myPartList = new infoShipListContainer();

    public void Start()
    {
        myPartList.shipInfo = new List<partShipInfo>();
    }

    public void SaveShip()
    {
        GameObject shipContainer = GameObject.Find("partGroup");

        

        List<partTracker> parts = new List<partTracker>();

        //iterating through all child objects
        for (int x = 0; x < shipContainer.transform.childCount; x++)
        {
            //temp part containers
            floatingPart currentPart = shipContainer.transform.GetChild(x).GetComponent<floatingPart>();
            partShipInfo tempContainer = new partShipInfo();

            //getting part ID
            tempContainer.part_ID = currentPart.partID;
            tempContainer.part_Name = currentPart.name;

            //Getting part connections
            List<PartConnectionPoints> connectPoints = new List<PartConnectionPoints>();
            InstallSlot[] slotsInPart = currentPart.transform.GetComponentsInChildren<InstallSlot>();
            
            for (int y = 0; y < slotsInPart.Length; y++)
            {
                PartConnectionPoints connectionP = new PartConnectionPoints();
                if (slotsInPart[y].connectedComponent != null)
                {
                    connectionP.connectedTo = slotsInPart[y].connectedComponent.name;
                    connectionP.xPos = slotsInPart[y].transform.localPosition.x;
                    connectionP.yPos = slotsInPart[y].transform.localPosition.y;
                    connectPoints.Add(connectionP);
                }
            }

            //is the part flipped?
            if (currentPart.isFlippedX)
            {
                tempContainer.isFlippedX = true;
            }

            if (currentPart.isFlippedY)
            {
                tempContainer.isFlippedY = true;
            }

            tempContainer.partConnections = connectPoints;
            myPartList.shipInfo.Add(tempContainer);
        }

        string JsonText = JsonUtility.ToJson(myPartList);
        File.WriteAllText(Application.dataPath + "/Ship.json", JsonText);
    }
}
