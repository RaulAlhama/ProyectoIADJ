using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Flee";
    }

    public override Steering GetSteering(AgentNPC agent)
    {
        Steering steer = new Steering();
        Vector3 newDirection = agent.Position - target.Position;
         float distancia =  newDirection.magnitude;
         if( distancia < 10f){
            steer.linear = newDirection.normalized * agent.MaxAcceleration;
         }else{
            steer.linear = Vector3.zero;
            agent.Velocity = Vector3.zero;
         }
       
            
        
    

        // Calcula el steering.
        

        // Mirar en la dirección
        
        steer.angular = 0;

       
        // Retornamos el resultado final.
        return steer;
    }

    
}