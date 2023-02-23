using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;

public class AgentPlayer : Agent
{
    public Agent target;
    public bool selected = false;
    //private ObstacleAvoidance obstacleAvoidance;
    public PathFollowing pathFollowing;
    private float distance;
    private Vector3 relativeTarget;

    public Path camino;

    // Update is called once per frame
    public virtual void Start(){

         pathFollowing = new PathFollowing(this,camino); //pathFollowing
         relativeTarget = pathFollowing.getSiguienteObjetivo(); //pathFollowing
         Orientation = transform.eulerAngles.y;
    }
    public virtual void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.
           
            if(target != null){
                
               /* if(obstacleAvoidance== null){

                    distance = (target.Position - Position).magnitude;
                    obstacleAvoidance = new ObstacleAvoidance(target, this);
                   
               }*/
                //Vector3 Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                //relativeTarget = obstacleAvoidance.getNuevoObjetivo(); //wallAvoidDance
                //relativeTarget = relativeTarget - Position;
                //relativeTarget = relativeTarget.normalized;
                //relativeTarget *= MaxAcceleration;

                relativeTarget = pathFollowing.getSiguienteObjetivo(); //pathFollowing
                Acceleration = relativeTarget - Position;
                Acceleration = Acceleration.normalized;
                Acceleration *= MaxAcceleration;
                
                Position += Velocity * Time.deltaTime;
                Velocity += Acceleration * Time.deltaTime;
                //Velocity += relativeTarget - Position;
                distance = (relativeTarget - Position).magnitude; //pathFollowing
                //distance = (target.Position - Position).magnitude; //wallAvoidDance
                
            
               if (distance < RadioExterior){

                    //Velocity = Vector3.zero; //wallAvoidDance
                    relativeTarget = pathFollowing.getSiguienteObjetivo(); //pathFollowing

                }else 
                if(Velocity.magnitude > MaxSpeed){

                    Velocity = Velocity.normalized;
                    Velocity *= MaxSpeed;
                }
                    
                //Velocity *= MaxSpeed;  // DESCOMENTA !!
                
                //Vector3 translation = Velocity * Time.deltaTime;
                //Position += translation;
                //transform.Translate(translation, Space.World);

                // Para el jugador usamos el SteeringBehaviour (LookAt)
                // que ya lleva implementado Unity.
                // Notar que al jugador le aplicamos un movimiento no-acelerado.
                transform.LookAt(transform.position + Velocity);
                
                Orientation = transform.rotation.eulerAngles.y; // DESCOMENTA !!
               

            }

        
    }

    void OnMouseDown(){
        
        if(!selected){

            selected = true;
        }else{

            selected = false;
        }
    }


}