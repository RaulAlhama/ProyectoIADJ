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
    public float tiempo;
    private float targetSpeed;
    private Vector3 targetVelocity;

    // Start is called before the first frame update
    void Start()
    {
        this.nameSteering = "Wander";
        wander = new GameObject("Wander");
        target = wander.AddComponent<Agent>() as Agent;
        firstTime = true;
        tiempo = 0.0f;

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
            agent.Velocity = Vector3.zero; //Detenemos al agente cuando llega al target
            //[Formaciones] restablecemos el tiempo para que espere 20 unidades.
            if(tiempo < 0)
                tiempo = 20.0f;
        }
        Steering steer;
        firstTime = false;
        //[Formaciones] Esperamos un tiempo antes de aplicar el movimiento
        if(agent.getStatus() == Agent.ENFORMACION && !(tiempo < 0)){
            tiempo -= Time.deltaTime;
            steer = new Steering(); // Devolvemos steering vacio
        } else{
            steer = base.GetSteering(agent); //Una vez pasado el tiempo aplicamos movimiento angular
        }
        //[Formaciones] Si el agente está parado, cambiamos al estado de formación
        if(steer.linear.magnitude == 0 && agent.inFormacion){
            agent.setStatus(Agent.ENFORMACION);
        }
        //Comprobamos que haya terminado de rotar para aplicar el movimiento lineal
        if ((steer.angular == 0 && !agent.inFormacion) || (steer.angular == 0 && tiempo < 0)){
            agent.setStatus(Agent.MOVING);
            //Aplicamos el movimiento lineal
            Vector3 newDirection = target.Position - agent.Position;
            float distance = newDirection.magnitude;
 
            if (distance < agent.RadioInterior)
            {   
                return steer;
            }
            if (distance > agent.RadioExterior)
            {
                targetSpeed = agent.MaxSpeed;
                //Debug.Log(distance + " > " + agent.RadioExterior + " ,Speed = " + agent.MaxSpeed);
            }
            else
            {
                targetSpeed = agent.MaxSpeed * distance/agent.RadioExterior;
                //Debug.Log(agent.RadioInterior + " < " + distance + " > " + agent.RadioExterior + " , Speed = " + agent.MaxSpeed * distance/agent.RadioInterior);
            }
            
            targetVelocity = newDirection * targetSpeed;
            steer.linear = targetVelocity - agent.Velocity;
            steer.linear /= timeToTarget;

            if (steer.linear.magnitude > agent.MaxAcceleration)
                steer.linear = steer.linear.normalized * agent.MaxAcceleration;
            
            }
       
        
        return steer;
        
    }
}
