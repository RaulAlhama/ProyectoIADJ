using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{
    private float timeToTarget = 0.1f;
    // Declara las variables que necesites para este SteeringBehaviour
    void Start()
    {
        this.nameSteering = "Arrive";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();
        Vector3 newDirection = target.Position - agent.Position;
        float distance = newDirection.magnitude;

        
        if (distance <= 0)
        {
            agent.Velocity = Vector3.zero;  //Para en seco
            return steer;
        }
        if (distance > agent.RadioExterior)
        {
            agent.Speed = agent.MaxSpeed;
            //Debug.Log(distance + " > " + agent.RadioExterior + " ,Speed = " + agent.MaxSpeed);
        }
        else
        {
            agent.Speed = agent.MaxSpeed * distance/agent.RadioExterior;
            //Debug.Log(agent.RadioInterior + " < " + distance + " > " + agent.RadioExterior + " , Speed = " + agent.MaxSpeed * distance/agent.RadioInterior);
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
