using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallSlot : MonoBehaviour {

    public string installType;
    /*Possible types:
    //mainBody
    //turretSlot
    //thrusterSlot
    */

    public GameObject connectedComponent;

    private FixedJoint2D joint;

    public bool buildMode = false;
    public bool isSelected = false;

    public void InstallJoint()
    {
        if (connectedComponent != null)
        {
            joint = this.transform.parent.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = connectedComponent.GetComponent<Rigidbody2D>();
            connectedComponent.transform.position = this.transform.position;
        }
    }

    public void OnMouseDown()
    {
        //if this part isnt selected, and no other part is selected
        if (isSelected == false && GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot == null)
        {
            isSelected = true;
            this.GetComponent<LineRenderer>().enabled = true;
            GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot = this;
        }// if there is a selected part
        else if (isSelected == false && GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot != null)
        {
            //setting connected components
            this.connectedComponent = GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot.transform.parent.gameObject;
            GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot.GetComponent<InstallSlot>().connectedComponent = transform.parent.gameObject;

            //installing joints on this and other object
            InstallJoint();
            GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot.InstallJoint();

            //resetting line renderer and stored object
            buildMode = false;
            GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot.GetComponent<InstallSlot>().buildMode = false;
            GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot = null;
            
        }
    }

    public void Start()
    {
        if (buildMode)
        {
            this.GetComponent<LineRenderer>().positionCount = 2;
        }
    }

    public void Update()
    {
        if (isSelected)
        {
            //Getting mouse position
            Vector3 pos = Input.mousePosition;
            pos.z = transform.position.z - Camera.main.transform.position.z;

            //setting line points
            this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            this.GetComponent<LineRenderer>().SetPosition(1, Camera.main.ScreenToWorldPoint(pos));

            if (Input.GetMouseButtonDown(1))
            {
                isSelected = false;
                this.GetComponent<LineRenderer>().enabled = false;
                GameObject.Find("BuildMaster").GetComponent<BuildMaster>().selectedSlot = null;
            }
        }

        if (buildMode == false)
        {
            isSelected = false;
            this.GetComponent<LineRenderer>().enabled = false;
        }
    }
}
