using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    private Agent explicitTarget;
    private GameObject pursue;

  void Start()
    {
        this.nameSteering = "Pursue";
        pursue = new GameObject("Pursue");
        Agent invisible = pursue.AddComponent<Agent>() as Agent;
        explicitTarget = target;
        this.target = invisible;
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