using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour {

    public GameObject shipController;

    public bool isBuildMode = false;

    public float weight;
    [Range(0, 75.0f)]
    public float armor;
    public float health;

    public InstallSlot[] slots;

    public void Start()
    {
        getSlots();
        this.GetComponent<Rigidbody2D>().mass = weight;
    }

    public BaseComponent()
    {
        //Total weight of the component, gets sent to rigidbody
        weight = 1;
        //Reduces damage to health, max of 75 which gives only 25% of damage
        armor = 10;
        //Total health of the component, if this equals zero, ur shit is dead son
        health = 100;
    }

    public void getSlots()
    {
        //getting all children install slot objects
        slots = this.transform.GetComponentsInChildren<InstallSlot>();

        foreach (InstallSlot slot in slots)
        {
            if (!(slot.installType == "thrusterSlot"))
            {
                slot.InstallJoint();
                
                if (slot.installType == "turretSlot" && slot.connectedComponent != null)
                {
                    slot.connectedComponent.GetComponent<TurretComponent>().slotPos = this.transform;
                }
            }
        }

        //delaying thruster install for proper positioning
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

    public void takeDamage(float damageAmount, string dmgType)
    {
        float actualDamage = damageAmount;

        if (armor != 0)
        {
            //getting the percentage value of the armor
            float armPercent = armor / 100;

            //get the amount of damage that gets subtracted
            float reducedAmount = damageAmount * armPercent;
            actualDamage = actualDamage - reducedAmount;

            //reducing armor amount
            armor -= actualDamage;

            /*
            Debug.Log("Armor:" + armPercent);
            Debug.Log("Armor:" + armor);
            Debug.Log("reducedAmount:" + reducedAmount);
            Debug.Log("aDmg:" + actualDamage);*/
        }

        health -= actualDamage;

        if (health <= 0)
        {
            destroyComponent();
        }
    }

    public void destroyComponent()
    {
        foreach (FixedJoint2D joint in this.GetComponents<FixedJoint2D>())
        {
            //disconnecting all connected components
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
