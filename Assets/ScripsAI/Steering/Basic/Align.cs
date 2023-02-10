using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    private float timeToTarget = 0.1f;
    public float targetRotation;
    
    void Start()
    {
        this.nameSteering = "Align";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        float rotation = target.Orientation - agent.Orientation;

        rotation = Bodi.MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        
        if (rotationSize < agent.AnguloInterior){
            Debug.Log(rotationSize + " < " + agent.AnguloInterior);
            steer.angular = 0;
            agent.Rotation = 0;
            return steer;
        }

        if (rotationSize > agent.AnguloExterior){
            Debug.Log(rotationSize + " > " + agent.AnguloExterior + " ,Rotation = " + agent.MaxRotation);
            targetRotation = agent.MaxRotation;
        }

        else{
            Debug.Log(agent.AnguloInterior + " < " + rotationSize + " > " + agent.AnguloExterior + " , Rotation = " + agent.MaxRotation * rotationSize/agent.AnguloExterior);
            targetRotation = agent.MaxRotation * rotationSize/agent.AnguloExterior;
        }      


        targetRotation *= rotation/rotationSize;
        steer.angular = targetRotation - agent.Rotation;
        steer.angular /= timeToTarget;
  
        
        float angularAcceleration = Mathf.Abs(steer.angular);
        if (angularAcceleration > agent.MaxAngularAcc)
        {
            steer.angular /= angularAcceleration;
            steer.angular *= agent.MaxAngularAcc;
        }
        
        // Retornamos el resultado final.
        steer.linear = Vector3.zero;
        return steer;
        
    }
}