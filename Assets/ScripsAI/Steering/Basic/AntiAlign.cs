using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiAlign : SteeringBehaviour
{
 private float timeToTarget = 0.1f;
    public float targetRotation;
    
    void Start()
    {
        this.nameSteering = "AntiAlign";
    }


    public override Steering GetSteering(AgentNPC agent)
    {

        // Creamos el steering.
        Steering steer = new Steering();

        // Variable para almacenar la rotación que falta para alinear.
        // Calculamos la rotación faltante sumando 180 a la orientación del target y restándole la del agente.
        float rotation = (target.Orientation + 180.0f) - agent.Orientation;

        rotation = Bodi.MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        // Si la rotación es menor al ángulo de visión interior del personaje, lo paramos.
        if (rotationSize < agent.AnguloInterior){
            return steer;
        }

        // Si la rotación es mayor al ángulo de visión exterior del personaje, establecemos la velocidad de rotación del agente al máximo.
        if (rotationSize > agent.AnguloExterior){
            targetRotation = agent.MaxRotation;
        }

        // Si la rotación está entre ángulo de visión exterior del personaje y el ángulo interior, reducimos la velocidad de rotación.
        else{
            targetRotation = agent.MaxRotation * rotationSize/agent.AnguloExterior;
        }      

        // Calculamos el steering angular del agente mediante la rotación obtenida.
        targetRotation *= rotation/rotationSize;
        steer.angular = targetRotation - agent.Rotation;
        steer.angular /= timeToTarget;
  
        // Si la aceleración obtenida a partir de la rotación calculada es mayor que la aceleración angular máxima, la establecemos al máximo.
        float angularAcceleration = Mathf.Abs(steer.angular);
        if (angularAcceleration > agent.MaxAngularAcc)
        {
            steer.angular /= angularAcceleration;
            steer.angular *= agent.MaxAngularAcc;
        }
        
        // Establecemos el steering linear a cero y devolvemos el steering.
        steer.linear = Vector3.zero;
        return steer;
    }
}
