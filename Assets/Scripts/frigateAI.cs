﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frigateAI : MonoBehaviour {

    [SerializeField]
    private GameObject middleReference;

    private bool dampenerOn = false;
    private float maxVelocity = 5.0f;

    public List<EngineComponent> shipEngines;
    public List<TurretComponent> shipTurrets;

    public List<BaseComponent> componentList;

    private Transform playerPosition;

    private float angleDiff;

    // Use this for initialization
    void Start()
    {
        getAllComponents();
        getEngines();
        getTurrets();

        dampenerOn = true;
    }

    void FixedUpdate()
    {
        getPlayerPosition();

        TurnTowardsPosition(playerPosition.GetComponent<shipControl>().middleReference.transform);
        AccelerationTowardsPoint(playerPosition.GetComponent<shipControl>().middleReference.transform.position);
    }

    private void TurnTowardsPosition(Transform trans)
    {
        Vector3 dir = trans.position - middleReference.transform.position;
        dir = trans.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float currentAngle = middleReference.transform.eulerAngles.z;
        currentAngle -= 270;

        if (currentAngle < -180)
        {
            currentAngle += 360;
        }

        //Debug.Log(angle + ", " + currentAngle);

        float differece = currentAngle - angle;
        angleDiff = differece;
        //Debug.Log(differece);

        if (differece < 0)
        {
            fireTurnLeftThrusters();
        }
        else if (differece > 0)
        {
            fireTurnRightThrusters();
        }

        if (differece > 45 || differece < -45)
        {
            InertiaDampiningAngularVelocity();
        }
    }

    private void AccelerationTowardsPoint(Vector3 position)
    {
        if (Vector3.Distance(middleReference.transform.position, position) >= 5.0f)
        {
            foreach (EngineComponent engine in shipEngines)
            {
                if (engine.tag == "mainEngine")
                {
                    if (!(angleDiff > 45) || !(angleDiff < -45))
                    {
                        engine.runThrusters(engine.GetComponent<Rigidbody2D>());
                    }
                    else
                    {
                        InertiaDampiningVelocity();
                    }
                }
            }
        }
    }

    private void fireTurnLeftThrusters()
    {
        foreach (EngineComponent engine in shipEngines)
        {
            if (engine.tag == "thrusterLeft")
            {
                engine.runThrusters(engine.GetComponent<Rigidbody2D>());
            }
        }
       // Debug.Log("left");
    }

    private void fireTurnRightThrusters()
    {
        foreach (EngineComponent engine in shipEngines)
        {
            if (engine.tag == "thrusterRight")
            {
                engine.runThrusters(engine.GetComponent<Rigidbody2D>());
            }
        }
       //Debug.Log("right");
    }

    private void getPlayerPosition()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void ClampVelocity()
    {
        foreach (BaseComponent comp in componentList)
        {
            float x = Mathf.Clamp(comp.GetComponent<Rigidbody2D>().velocity.x, -maxVelocity, maxVelocity);
            float y = Mathf.Clamp(comp.GetComponent<Rigidbody2D>().velocity.y, -maxVelocity, maxVelocity);

            comp.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
        }
    }

    //inertia dampening
    private void InertiaDampiningAngularVelocity()
    {
        if (dampenerOn)
        {
            foreach (BaseComponent comp in componentList)
            {
                float oppositeAng = comp.GetComponent<Rigidbody2D>().angularVelocity * -1;
                comp.GetComponent<Rigidbody2D>().angularVelocity += (oppositeAng / 25);
            }
        }
    }

    private void InertiaDampiningVelocity()
    {
        if (dampenerOn)
        {
            foreach (BaseComponent comp in componentList)
            {
                Vector2 oppositeVel = comp.GetComponent<Rigidbody2D>().velocity * -1;
                comp.GetComponent<Rigidbody2D>().velocity += (oppositeVel / 55);
            }
        }
    }

    private void getAllComponents()
    {
        BaseComponent[] components = transform.GetComponentsInChildren<BaseComponent>();

        for (int i = 0; i < components.Length; i++)
        {
            components[i].shipController = this.gameObject;
            components[i].getSlots();
            componentList.Add(components[i]);
        }
    }

    private void getEngines()
    {
        EngineComponent[] engineArray = transform.GetComponentsInChildren<EngineComponent>();

        for (int i = 0; i < engineArray.Length; i++)
        {
            shipEngines.Add(engineArray[i]);
        }
    }

    private void getTurrets()
    {
        TurretComponent[] turretArray = transform.GetComponentsInChildren<TurretComponent>();

        for (int i = 0; i < turretArray.Length; i++)
        {
            shipTurrets.Add(turretArray[i]);
        }
    }
}
