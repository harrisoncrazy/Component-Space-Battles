﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class partUIElement : MonoBehaviour
{
    public Image mainImage;
    public Text mainText;
    public string partID;
    public bool keycodeReq;

    [SerializeField] private GameObject floatingPartPref;
    [SerializeField] private GameObject installSlotPref;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Instanciating floating part
        GameObject prefab = Instantiate(floatingPartPref, transform.position, transform.rotation) as GameObject;
        prefab.transform.SetParent(null);
        prefab.transform.localPosition = new Vector3(0, 0);
        prefab.GetComponent<floatingPart>().mainSprite.sprite = this.mainImage.sprite;
        prefab.GetComponent<floatingPart>().partID = partID;
        prefab.GetComponent<floatingPart>().keycodeReq = keycodeReq;
        prefab.AddComponent<PolygonCollider2D>().isTrigger = true;
        prefab.name = partID + "_" + Random.Range(0.0f, 420.0f).ToString("F2");

        //Instanciating part installslot indicators
        List<ConnectionPoints> connectionPoints = GameObject.Find("BuildMaster").GetComponent<BuildMaster>().returnInstallSlots(partID);

        for (int i = 0; i < connectionPoints.Count; i++)
        {
            GameObject partPoint = Instantiate(installSlotPref, transform.position, transform.rotation) as GameObject;
            partPoint.transform.SetParent(prefab.transform);
            partPoint.GetComponent<SpriteRenderer>().sortingOrder = 5;
            partPoint.transform.localPosition = new Vector2(connectionPoints[i].xPos, connectionPoints[i].yPos);
            partPoint.GetComponent<InstallSlot>().installType = connectionPoints[i].type;
        }
    }
}
