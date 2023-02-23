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

        Steering steer = base.GetSteering(agent);
        //Aplicamos el movimiento lineal
        Vector3 newDirection = target.Position - agent.Position;
        float distance = newDirection.magnitude;

        
        if (distance < agent.RadioInterior)
        {
            agent.Velocity = Vector3.zero;  //Para en seco
            return steer;
        }
        if (distance > agent.RadioExterior)
        {
            agent.Speed = agent.MaxSpeed;
            //Debug.Log(distance + " > " + agent.RadioExterior + " ,Speed = " + agent.MaxSpeed);
        }
        else
        {
            agent.Speed = agent.MaxSpeed * distance/agent.RadioExterior;
            Debug.Log(agent.RadioInterior + " < " + distance + " > " + agent.RadioExterior + " , Speed = " + agent.MaxSpeed * distance/agent.RadioInterior);
        }
        
        agent.Velocity = newDirection.normalized;
        agent.Velocity *= agent.Speed;

        steer.linear = agent.Velocity - target.Velocity;
        steer.linear /= timeToTarget;
        //agent.transform.rotation = new Quaternion(0,90,0,1);

        if (steer.linear.magnitude > agent.MaxAcceleration)
            steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        

        return steer;
        
    }
}
