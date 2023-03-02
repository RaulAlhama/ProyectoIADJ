using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    [SerializeField] [Range(0.0f, 20.0f)] private float maxPrediction;

  void Start()
    {
        this.nameSteering = "Pursue";
    }


    public override Steering GetSteering(AgentNPC agent)
    {

        // Vamos a  crear un nuevo target en la posicion donde estaria nuestro target
        Vector3 newDirection = target.Position - agent.Position;
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
        var predictedTargetPosition = this.target.Position + target.Velocity * predictedSpeed; //obtenemos la supuesta posición donde se econtrará el target
        isExplicitTarget = true;
        explTargetDirection = predictedTargetPosition - agent.Position;

        return base.GetSteering(agent);
    }
}