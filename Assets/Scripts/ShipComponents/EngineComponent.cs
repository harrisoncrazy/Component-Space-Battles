using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineComponent : BaseComponent {

    public ThrusterComponent[] thrustPoints;
    public float totalThrustStrength = 2;

    public KeyCode code;

    public EngineComponent() {}

	void Start()
    {
        base.Start();
        getThrustPoints();
    }
    
    public void getThrustPoints()
    {
        thrustPoints = this.transform.GetComponentsInChildren<ThrusterComponent>();
        
        foreach (ThrusterComponent component in thrustPoints)
        {
            component.thrustStrength = totalThrustStrength / thrustPoints.Length;
        }
    }

    public void runThrusters(Rigidbody2D rb)
    {
        foreach (ThrusterComponent thruster in thrustPoints)
        {
            thruster.addThrust(rb);
        }
    }
}
