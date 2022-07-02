using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipControl : MonoBehaviour
{
    public Transform middleReference;

    [SerializeField]
    private BaseComponent middleComponent;

    private bool dampenerOn = false;
    private float maxVelocity = 5.0f;

    public List<EngineComponent> shipEngines;
    public List<TurretComponent> shipTurrets;

    public List<BaseComponent> componentList;

    [SerializeField]
    private float thrusterImpulseTime = 1.5f;
    [SerializeField]
    private float maxThrusterImpulseTime = 1.5f;
    [SerializeField]
    private float thrusterDelayTime  = 3.0f;
    [SerializeField]
    private float maxThrusterDelayTime = 3.0f;

    // Use this for initialization
    void Start()
    {
        componentList = new List<BaseComponent>();
        shipEngines = new List<EngineComponent>();
        shipTurrets = new List<TurretComponent>();

        getAllComponents();
        getEngines();
        getTurrets();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        middleReference.transform.position = middleComponent.transform.position;
        middleReference.transform.rotation = middleComponent.transform.rotation;
    }

    void FixedUpdate()
    {
        ClampVelocity();
        InertiaDampining();
    }

    private void getInput()
    {
        foreach (EngineComponent engine in shipEngines)
        {
            if (Input.GetKey(engine.primeCode))
            {
                engine.runThrusters(engine.GetComponent<Rigidbody2D>(), 0);
            }

            //alt fire for left/right thrusting, only if time left in thruster time
            if (Input.GetKey(engine.altCode) && thrusterImpulseTime > 0)
            {
                engine.runThrusters(engine.GetComponent<Rigidbody2D>(), 10);

                thrusterImpulseTime -= Time.deltaTime;
            }
        }

        //alt fire is out of time and recharging
        if (thrusterImpulseTime <= 0)
        {
            thrusterDelayTime -= Time.deltaTime;

            if (thrusterDelayTime <= 0) //timer is out, reset the impulse time
            {
                thrusterImpulseTime = maxThrusterImpulseTime;
                thrusterDelayTime = maxThrusterDelayTime;
            }
        }
    

        foreach (TurretComponent turret in shipTurrets)
        {
            if (Input.GetKey(turret.code))
            {
                turret.turretTimer -= Time.deltaTime;

                if (turret.turretTimer <= 0)
                {
                    turret.Fire();
                    turret.turretTimer = turret.turretTimerDefault;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            dampenerOn = true;
        }
        else
        {
            dampenerOn = false;
        }
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
    private void InertiaDampining()
    {
        if (dampenerOn) //Spacebar slowdown
        {
            foreach (BaseComponent comp in componentList)
            {
                Vector2 oppositeVel = comp.GetComponent<Rigidbody2D>().velocity * -1;
                comp.GetComponent<Rigidbody2D>().velocity += (oppositeVel / 25);
                float oppositeAng = comp.GetComponent<Rigidbody2D>().angularVelocity * -1;
                comp.GetComponent<Rigidbody2D>().angularVelocity += (oppositeAng / 25);
            }
        } 
        else //Natural dampening
        {
            foreach (BaseComponent comp in componentList)
            {
                Vector2 oppositeVel = comp.GetComponent<Rigidbody2D>().velocity * -1;
                comp.GetComponent<Rigidbody2D>().velocity += (oppositeVel / 100);
                float oppositeAng = comp.GetComponent<Rigidbody2D>().angularVelocity * -1;
                comp.GetComponent<Rigidbody2D>().angularVelocity += (oppositeAng / 100);
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
            turretArray[i].weight = 0.0000001f;
        }
    }
}
