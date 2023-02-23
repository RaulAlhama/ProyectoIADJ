using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : SteeringBehaviour
{
    private float timeToTarget = 0.1f;
    // Declara las variables que necesites para este SteeringBehaviour
    void Start()
    {
        this.nameSteering = "Leave";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        Vector3 newDirection = agent.Position - target.Position; //Dirección contraria a Arrive
        float distance = newDirection.magnitude;

        
        if (distance > 10.0f)
        {
            steer.linear = Vector3.zero;
            agent.Velocity = Vector3.zero;
        }
        if (distance < agent.RadioInterior)
        {
            agent.Speed = agent.MaxSpeed;
            //Debug.Log(distance + " > " + agent.RadioExterior + " ,Speed = " + agent.MaxSpeed);
        }
        else
        {
            agent.Speed = agent.MaxSpeed * distance/agent.RadioExterior;
            Debug.Log(agent.RadioInterior + " < " + distance + " > " + agent.RadioExterior + " , Speed = " + agent.MaxSpeed * distance/agent.RadioInterior);
        }
        
        agent.Velocity = newDirection.normalized;
        agent.Velocity *= agent.Speed;

        steer.linear = agent.Velocity - target.Velocity;
        steer.linear /= timeToTarget;
        //agent.transform.rotation = new Quaternion(0,90,0,1);

        if (steer.linear.magnitude > agent.MaxAcceleration)
            steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        
        
        // Retornamos el resultado final.
        steer.angular = 0;
        return steer;
    }
}
