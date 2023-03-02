using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : Flee
{
    private Agent explicitTarget;
    private GameObject evade;

  void Start()
    {
        this.nameSteering = "Evade";
    }

    [SerializeField] [Range(0.0f, 10.0f)] private float maxPrediction;
    

    public override Steering GetSteering(AgentNPC agent)
    {

        // Vamos a  crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 newDirection = explicitTarget.Position - agent.Position;
        float distance = newDirection.magnitude;

        // Velocidad actual
        var speed = agent.Velocity.magnitude;

        // Si la velocidad actual es pequeña, aplicamos la prediccion
        float predictedSpeed;
        if(speed <= distance/maxPrediction){
            predictedSpeed = maxPrediction;
        } else{
            predictedSpeed = distance / speed;
        }


        //Dirección predicha
        Debug.Log("ANTES: " + explicitTarget.Position + explicitTarget.Velocity + predictedSpeed);
        this.target.Position = explicitTarget.Position;
        this.target.Position += explicitTarget.Velocity * predictedSpeed;
        Debug.Log("DESPUES: " + explicitTarget.Position);

        return base.GetSteering(agent);
    }
}