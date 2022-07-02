using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterComponent : MonoBehaviour
{

    public float thrustStrength;
    public bool rightThrust;

    public void addThrust(Rigidbody2D rb, float forceEdit)
    {
        //editing the force for an alt impulse
        float thrustStrNew = thrustStrength + forceEdit;
        Vector2 force = transform.up * thrustStrNew / 10;

        rb.AddForce(force);
    }
}