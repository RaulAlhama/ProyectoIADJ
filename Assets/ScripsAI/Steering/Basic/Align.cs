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

        // Creamos el steering.
        Steering steer = new Steering();

        // Variable para almacenar la rotación que falta para alinear.
        float rotation;
        
        if(isExplicitTarget){
            rotation = this.explTargetRotation;
        }

        // Calculamos la rotación faltante restando la orientación del target a la del agente.
        else{
            rotation = target.Orientation - agent.Orientation;
        }
        rotation = Bodi.MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        // Si la rotación es menor al ángulo de visión interior del personaje, lo paramos.
        if (rotationSize < agent.AnguloInterior){
            steer.angular = 0;
            agent.Rotation = 0;
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