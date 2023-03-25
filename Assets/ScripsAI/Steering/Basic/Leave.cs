using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : SteeringBehaviour
{
    private float timeToTarget = 0.1f;
    void Start()
    {
        this.nameSteering = "Leave";
    }


    public override Steering GetSteering(AgentNPC agent)
    {

        // Creamos el steering.
        Steering steer = new Steering();

        // Calculamos la dirección deseada restando la posición del target a la del agente.
        Vector3 newDirection = agent.Position - target.Position;

        // Obtenemos la distancia que debe recorrer calculando el módulo de la dirección.
        float distance = newDirection.magnitude;

        // Si la distancia es mayor a un determinado valor, detenemos al agente.
        if (distance > 10.0f)
        {
            steer.linear = Vector3.zero;
            agent.Velocity = Vector3.zero;
        }

        // Si la distancia es menor que el radio interior del personaje, establecemos su velocidad al máximo.
        if (distance < agent.RadioInterior)
        {
            agent.Speed = agent.MaxSpeed;

        }

        // Si la distancia está entre ambos, reducimos la velocidad.
        else
        {
            agent.Speed = agent.MaxSpeed * distance/agent.RadioExterior;
        }
        
        // Obtenemos el vector velocidad mediante la dirección y la velocidad obtenida.
        agent.Velocity = newDirection.normalized;
        agent.Velocity *= agent.Speed;

        steer.linear = agent.Velocity - target.Velocity;
        steer.linear /= timeToTarget;

        // Si la aceleración obtenida a partir de la velocidad calculada es mayor que la aceleración máxima, la establecemos al máximo.
        if (steer.linear.magnitude > agent.MaxAcceleration)
            steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        
        // Establecemos el steering angular a cero y devolvemos el steering.
        steer.angular = 0;
        return steer;
    }
}
