using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Wander : Face
{

    private float wanderOffset=5f;             // Desplazamiento hacia delante del wander
    public float wanderRadius =15f;            // Radio del desplazamiento
    public float wanderRate=20f;               // Valor de la maxima orientacion que puede cambiar el wander
    private float wanderOrientation=0f;
    private GameObject wander;

    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Wander";
        wander = new GameObject("Wander");
        target = wander.AddComponent<Agent>() as Agent;

    }

    // Update is called once per frame
    public override Steering GetSteering(AgentNPC agent)
    {

        if (((this.target.Position - agent.Position).magnitude < agent.RadioInterior)){

             Random rnd = new Random();
            wanderOrientation += ((float) rnd.NextDouble()) * wanderRate;
       
            this.target.Orientation =  wanderOrientation + agent.Orientation;
            this.target.Position = agent.Position + wanderOffset * Bodi.AngleToPosition(agent.Orientation);
            this.target.Position += wanderRadius * Bodi.AngleToPosition(this.target.Orientation);
             
        }

        Steering steering = base.GetSteering(agent);
        //Aplicamos el movimiento lineal
        Vector3 newDirection = target.Position - agent.Position;
        steering.linear = newDirection.normalized * agent.MaxAcceleration;

        return steering;
        
    }
}
