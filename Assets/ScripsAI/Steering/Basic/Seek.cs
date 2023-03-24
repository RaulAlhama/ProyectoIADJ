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

        // Creamos el steering.
        Steering steer = new Steering();

        Vector3 newDirection;
        if(isExplicitTarget){
            newDirection = explTargetDirection;
        }

        // En caso contrario, calculamos la dirección deseada restando la posición del target a la del agente.
        else{
            newDirection = target.Position - agent.Position;
        }
        
        // Obtenemos el vector velocidad mediante la dirección y aceleración máxima del agente.
        steer.linear = newDirection.normalized * agent.MaxAcceleration;

        // Establecemos el steering angular a cero y devolvemos el steering.
        steer.angular = 0;
        return steer;

        
    }
}