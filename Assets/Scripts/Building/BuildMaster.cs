using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class partInfo
{
    public string imgName;
    public int numConnections;
    public string displayName;

    public List<ConnectionPoints> partConnections;
}

[Serializable]
public class ConnectionPoints
{
    public string type;
    public float xPos;
    public float yPos;
}

[Serializable]
public class infoListContainer
{
    public List<partInfo> partInfoList;
}

[Serializable]
public class imgReference
{
    public string refName;
    public Sprite partSprite;
}


public class BuildMaster : MonoBehaviour
{
    [SerializeField] private TextAsset jsonObj;

    //HAVE TO USE A CONTAINER IN ORDER TO CORRECTLY PARSE A JSON FILE
    public infoListContainer myPartList = new infoListContainer();
    public List<imgReference> partImages;

    [SerializeField] private GameObject uiDisplayPref;

    public void Start()
    {
        //parsing JSON to string, data path
        string jsonText = File.ReadAllText(Application.dataPath + "/Test.json");
        //String with JSON included, container with the class inside
        JsonUtility.FromJsonOverwrite(jsonText, myPartList);

        GenerateUI();
    }

    private void GenerateUI()
    {
        int offset = 0;
        for (int i = 0; i < myPartList.partInfoList.Count; i++)
        {
            //instanciating ui element, parenting it to the container
            GameObject prefab = Instantiate(uiDisplayPref, transform.position, transform.rotation) as GameObject;
            prefab.transform.SetParent(GameObject.Find("ObjectContainer").transform, false);
            prefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 - offset);
            offset += 160;

            float scaleY = 180;

            for (int z = 0; z < partImages.Count; z++)
            {
                if (myPartList.partInfoList[i].imgName == partImages[z].refName)
                {
                    //Setting the sprite and text on the ui element
                    prefab.GetComponent<partUIElement>().mainImage.sprite = partImages[z].partSprite;
                    prefab.GetComponent<partUIElement>().mainText.text = myPartList.partInfoList[i].displayName;
                    prefab.GetComponent<partUIElement>().partID = myPartList.partInfoList[i].imgName;
                }

                //Scaling the viewport/scroll area
                scaleY += 160;
                Vector2 newScale = new Vector2(GameObject.Find("ObjectContainer").GetComponent<RectTransform>().sizeDelta.x, scaleY);
                GameObject.Find("ObjectContainer").GetComponent<RectTransform>().sizeDelta = newScale;
            }

            //Scaling the viewport/scroll area (undoing single offset, as it goes over by 1 object) HACK SOLUTION, maybe find a way to fix later
            scaleY -= 140; //160 - 20 for the small gap at the bottom
            Vector2 newScale2 = new Vector2(GameObject.Find("ObjectContainer").GetComponent<RectTransform>().sizeDelta.x, scaleY);
            GameObject.Find("ObjectContainer").GetComponent<RectTransform>().sizeDelta = newScale2;
        }
    }

    public List<ConnectionPoints> returnInstallSlots (string ID)
    {
        List<ConnectionPoints> connectionPoints = new List<ConnectionPoints>();

        for (int i = 0; i < myPartList.partInfoList.Count; i++)
        {
            if (myPartList.partInfoList[i].imgName == ID)
            {
                connectionPoints = myPartList.partInfoList[i].partConnections;
            }
        }

        return connectionPoints;
    }
}
