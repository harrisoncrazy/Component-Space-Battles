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

    public infoListContainer myPartList = new infoListContainer();
    public List<imgReference> partImages;

    [SerializeField] private GameObject uiDisplayPref;

    public void Start()
    {
        string jsonText = File.ReadAllText(Application.dataPath + "/Test.json");
        JsonUtility.FromJsonOverwrite(jsonText, myPartList);

        GenerateUI();
    }

    private void GenerateUI()
    {
        int offset = 0;
        for (int i = 0; i < myPartList.partInfoList.Count; i++)
        {
            GameObject prefab = Instantiate(uiDisplayPref, transform.position, transform.rotation) as GameObject;
            prefab.transform.SetParent(GameObject.Find("ObjectContainer").transform, false);
            prefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 - offset);
            offset += 160;

            for (int z = 0; z < partImages.Count; z++)
            {
                if (myPartList.partInfoList[i].imgName == partImages[z].refName)
                {
                    prefab.GetComponent<partUIElement>().mainImage.sprite = partImages[z].partSprite;
                    prefab.GetComponent<partUIElement>().mainText.text = myPartList.partInfoList[i].displayName;
                }
            }
        }
    }
}
