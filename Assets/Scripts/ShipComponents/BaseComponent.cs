using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour {

    public GameObject shipController;

    public float weight;
    public float armor;
    public float health;
    public float shields;
    
    public InstallSlot[] slots;

    public void Start()
    {
        //getSlots();
    }

    public BaseComponent()
    {
        weight = 1;
        armor = 100;
        health = 100;
        shields = 0;
    }

    public void getSlots()
    {
        slots = this.transform.GetComponentsInChildren<InstallSlot>();

        foreach (InstallSlot slot in slots)
        {
            if (!(slot.installType == "thrusterSlot"))
            {
                slot.InstallJoint();
            }

            if (slot.installType == "turretSlot")
            {
                slot.connectedComponent.GetComponent<TurretComponent>().slotPos = this.transform;
            }
        }

        StartCoroutine(delayedThruster());
    }

    public IEnumerator delayedThruster()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (InstallSlot slot in slots)
        {
            if (slot.installType == "thrusterSlot")
            {
                slot.InstallJoint();
            }
        }
    }

    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            foreach(FixedJoint2D joint in this.GetComponents<FixedJoint2D>())
            {
                if (joint.connectedBody != null)
                {
                    Rigidbody2D rbRef = joint.connectedBody;
                    joint.connectedBody = null;

                    Vector3 direction = rbRef.transform.position - this.transform.position;
                    direction = direction.normalized;

                    rbRef.AddForce(direction * 15.0f, ForceMode2D.Force);
                }
            }

            Destroy(this.gameObject);
        }
    }
}
