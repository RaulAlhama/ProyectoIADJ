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

        // Creamos el steering.
        Steering steer = new Steering();

        // Almacenamos en maxAcceleration la aceleración máxima del personaje.
        float maxAcceleration = agent.MaxAcceleration;

        // Calculamos la velocidad deseada restando la velocidad del target a la del agente.
        steer.linear = target.Velocity - agent.Velocity;
        steer.linear /= timeToTarget;

        // Si la aceleración obtenida a partir de la velocidad calculada es mayor que la aceleración máxima, la establecemos al máximo.
        if(steer.linear.magnitude > maxAcceleration){
            steer.linear = steer.linear.normalized * maxAcceleration;
        }

        // Establecemos el steering angular a cero y devolvemos el steering.
        steer.angular = 0;
        return steer;
    }
}