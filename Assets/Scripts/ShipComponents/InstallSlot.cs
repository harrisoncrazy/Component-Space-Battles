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

    public BuildMaster buildMaster;

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
        if (isSelected == false && buildMaster.selectedSlot == null && connectedComponent == null)
        {
            isSelected = true;
            this.GetComponent<LineRenderer>().enabled = true;
            buildMaster.selectedSlot = this;
        }// if there is a selected part
        else if (isSelected == false && buildMaster.selectedSlot != null && connectedComponent == null)
        {
            //setting connected components
            this.connectedComponent = buildMaster.selectedSlot.transform.parent.gameObject;
            buildMaster.selectedSlot.GetComponent<InstallSlot>().connectedComponent = transform.parent.gameObject;

            //Detecting and grouping parts dynamically
            //if not already attached to a part 
            if (transform.parent.transform.parent == null && buildMaster.selectedSlot.transform.parent.transform.parent == null)
            {
                //Adding to empty gameobject to manage moving together
                GameObject emptyObj = new GameObject();
                emptyObj.name = "partGroup";
                emptyObj.transform.position = this.transform.position;
                transform.parent.transform.parent = emptyObj.transform;
                buildMaster.selectedSlot.transform.parent.transform.parent = emptyObj.transform;
            }
            else if (transform.parent.transform.parent != null && buildMaster.selectedSlot.transform.parent.transform.parent != null)//if both objects have more than 2 paired
            {
                GameObject objectContainer = this.transform.parent.transform.parent.gameObject;
                //iterating through list of objects, moving them to other container
                for (int i = objectContainer.transform.childCount-1; i >= 0; i--)
                {
                    Transform child = objectContainer.transform.GetChild(i);
                    child.transform.position = buildMaster.selectedSlot.transform.parent.transform.parent.transform.position;
                    child.SetParent(buildMaster.selectedSlot.transform.parent.transform.parent, true);
                }
                Destroy(objectContainer);
            }
            else if (transform.parent.transform.parent != null) //if connecting more than 2 parts together
            {
                buildMaster.selectedSlot.transform.parent.transform.position = this.transform.parent.transform.parent.transform.position;
                buildMaster.selectedSlot.transform.parent.transform.parent = this.transform.parent.transform.parent;
            }
            else if (buildMaster.selectedSlot.transform.parent.transform.parent != null)
            {
                this.transform.parent.transform.position = buildMaster.selectedSlot.transform.parent.transform.parent.transform.position;
                this.transform.parent.transform.parent = buildMaster.selectedSlot.transform.parent.transform.parent;
            }

            //positioning all parts
            floatingPart[] allParts = FindObjectsOfType<floatingPart>();
            foreach (floatingPart part in allParts)
            {
                part.getSlots();
            }

            //resetting line renderer and stored object
            buildMode = false;
            isSelected = false;
            this.GetComponent<LineRenderer>().enabled = false;
            buildMaster.selectedSlot.GetComponent<InstallSlot>().buildMode = false;
            buildMaster.selectedSlot = null;
        }
    }

    public void Start()
    {
        if (buildMode)
        {
            this.GetComponent<LineRenderer>().positionCount = 2;
            buildMaster = GameObject.Find("BuildMaster").GetComponent<BuildMaster>();
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
    }
}
