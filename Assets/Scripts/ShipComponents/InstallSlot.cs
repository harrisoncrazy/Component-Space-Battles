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

    public BaseComponent connectedComponent;

    private FixedJoint2D joint;

    public void InstallJoint()
    {
        joint = this.transform.parent.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = connectedComponent.GetComponent<Rigidbody2D>();
        connectedComponent.transform.position = this.transform.position;
    }
}
