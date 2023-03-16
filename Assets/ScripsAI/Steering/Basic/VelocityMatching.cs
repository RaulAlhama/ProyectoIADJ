using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    private float timeToTarget = 0.1f;
    
    void Start()
    {
        this.nameSteering = "VelocityMatching";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();
        float maxAcceleration = agent.MaxAcceleration;
        // Calcula el steering.

        steer.linear = target.Velocity - agent.Velocity;
        steer.linear /= timeToTarget;

        if(steer.linear.magnitude > maxAcceleration){
            steer.linear = steer.linear.normalized * maxAcceleration;
        }

        steer.angular = 0;
        return steer;
    }
}