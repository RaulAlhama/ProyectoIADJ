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

    private float targetSpeed;
    private Vector3 targetVelocity;

    public override Steering GetSteering(AgentNPC agent)
    {
        // Creamos el steering.
        Steering steer = new Steering();

        // Calculamos la dirección deseada restando la posición del target a la del agente.
        Vector3 newDirection = target.Position - agent.Position;

        // Obtenemos la distancia que debe recorrer calculando el módulo de la dirección.
        float distance = newDirection.magnitude;

        // Si la distancia es menor al radio interior del personaje, lo paramos.
        if (distance < agent.RadioInterior)
        {
            if(/*agent.getLLegada() && */ distance < agent.RadioInterior){
                    agent.Velocity = Vector3.zero; //wallAvoidDance
            }

            agent.setStatus(Agent.STOPPED);  //Para en seco
            return steer;
        }

        // Si la distancia es menor al radio exterior del personaje, establecemos la velocidad del agente a su máximo.
        if (distance > target.RadioExterior)
        {
            targetSpeed = agent.MaxSpeed;
             agent.setStatus(Agent.MOVING);
        }

        // Si la distancia está entre radio exterior del personaje y el radio interior, reducimos la velocidad.
        else
        {
            targetSpeed = agent.MaxSpeed * distance/agent.RadioExterior;
        }
        
        // Obtenemos el vector velocidad mediante la dirección y la velocidad obtenida.
        if(agent.inFormacion && agent.liderFollowing){
            agent.Velocity = newDirection.normalized;
            agent.Velocity *= targetSpeed;
            steer.linear = agent.Velocity - target.Velocity;
            steer.linear /= timeToTarget;
        } else{
            targetVelocity = newDirection * targetSpeed;
            steer.linear = targetVelocity - agent.Velocity;
            steer.linear /= timeToTarget;
        }
       

            
        

        // Si la aceleración obtenida a partir de la velocidad calculada es mayor que la aceleración máxima, la establecemos al máximo.
        if (steer.linear.magnitude > agent.MaxAcceleration)
            steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        
        
        // Establecemos el steering angular a cero y devolvemos el steering.
        steer.angular = 0;
        return steer;
    }
}