using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{

    // Declara las variables que necesites para este SteeringBehaviour

    
    void Start()
    {
        this.nameSteering = "Seek";
    }


    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();
        Vector3 newDirection;
        if(isExplicitTarget){
            newDirection = explTargetDirection;
        }
        else{
            newDirection = target.Position - agent.Position;
        }
        

        // Calcula el steering.
        // Asignamos a steer.linear el vector obtenido normalizado
        steer.linear = newDirection.normalized * agent.MaxAcceleration;
        // Asignamos a steer.angular el escalar cero.
        steer.angular = 0;

        /*float distancia =  newDirection.magnitude;

         if( distancia < target.RadioInterior){
            steer.linear = Vector3.zero;
            agent.Velocity = Vector3.zero;
        }*/
        // Retornamos el resultado final.
        return steer;

        
    }
}