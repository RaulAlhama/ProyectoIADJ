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
        
        // Creamos el steering.
        Steering steer = new Steering();

        // Calculamos la dirección deseada restando la posición del agente a la del target.
        Vector3 newDirection = agent.Position - target.Position;

        // Obtenemos la distancia que debe recorrer calculando el módulo de la dirección.
        float distancia =  newDirection.magnitude;
        
        // Si la distancia es menor que un determinado valor, desplazamos el agente en la dirección calculada en máxima aceleración.
        if(distancia < 10f){
            steer.linear = newDirection.normalized * agent.MaxAcceleration;
        }
        
        // Si no, establecemos tanto el steering linear como la velocidad a cero.
        else{
            steer.linear = Vector3.zero;
            agent.Velocity = Vector3.zero;
        }
        
        // Establecemos el steering angular a cero y devolvemos el steering.
        steer.angular = 0;
        return steer;
    }

    
}