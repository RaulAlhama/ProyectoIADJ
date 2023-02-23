using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour
    protected float timeToTarget = 0.1f;
    private float targetRotation;
    
    void Start()
    {
        this.nameSteering = "Align";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();

        // Calcula el steering.
        float rotation;
        
        if(useCustom){
             rotation = this.customRotation;
        }
        else{
            rotation = target.Orientation - agent.Orientation;
        }
        rotation = Bodi.MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < agent.AnguloInterior){
            steer.angular = 0;
            agent.Rotation = 0;
            return steer;
        }

        if (rotationSize > agent.AnguloExterior){
            targetRotation = agent.MaxRotation;
        }

        else{
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
        
        steer.linear = Vector3.zero;
        return steer;
        
    }
}