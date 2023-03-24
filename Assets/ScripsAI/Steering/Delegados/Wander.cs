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
    private bool firstTime;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Wander";
        wander = new GameObject("Wander");
        target = wander.AddComponent<Agent>() as Agent;
        firstTime = true;
        time = 0.1f;

    }

    // Update is called once per frame
    public override Steering GetSteering(AgentNPC agent)
    {

        if ((this.target.Position - agent.Position).magnitude < agent.RadioInterior || firstTime){
            Random rnd = new Random();
            wanderOrientation += (float)rnd.NextDouble() * wanderRate;
       
            this.target.Orientation =  wanderOrientation + agent.Orientation;
            this.target.Position = agent.Position + wanderOffset * Bodi.AngleToPosition(agent.Orientation);
            this.target.Position += wanderRadius * Bodi.AngleToPosition(this.target.Orientation);
            agent.Velocity = new Steering().linear;
             
        }

        Steering steer = base.GetSteering(agent);
        firstTime = false;
        time -= Time.deltaTime;
            
        if (agent.Rotation == 0 && time <= 0){
            time = 0.1f;
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
            }
            else
            {
                agent.Speed = agent.MaxSpeed * distance/agent.RadioExterior;
            }
            
            agent.Velocity = newDirection.normalized;
            agent.Velocity *= agent.Speed;

            steer.linear = agent.Velocity - target.Velocity;
            steer.linear /= timeToTarget;

            if (steer.linear.magnitude > agent.MaxAcceleration)
                steer.linear = steer.linear.normalized * agent.MaxAcceleration;
            }
       
        

        return steer;
        
    }
}
