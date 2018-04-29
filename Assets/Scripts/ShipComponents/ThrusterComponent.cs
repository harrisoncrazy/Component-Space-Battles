using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterComponent : MonoBehaviour {

    public float thrustStrength;

    public void addThrust(Rigidbody2D rb)
    {
        Vector3 force = transform.up * thrustStrength / 10;
        Vector3 forcePosition = transform.position;

        rb.AddForceAtPosition(force, forcePosition);
    }
}
