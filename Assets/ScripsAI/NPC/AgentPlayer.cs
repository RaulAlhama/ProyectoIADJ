using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;

public class AgentPlayer : Agent
{
    public Agent target;
    public bool selected = false;
    private ObstacleAvoidance obstacleAvoidance;
    private float distance;
    private Vector3 relativeTarget;

    public virtual void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.
           
        if(target != null){
            
            if(obstacleAvoidance== null){

                distance = (target.Position - Position).magnitude;
                obstacleAvoidance = new ObstacleAvoidance(target, this);
               
        }
            //Vector3 Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            relativeTarget = obstacleAvoidance.getNuevoObjetivo(); //wallAvoidDance
            Acceleration = relativeTarget - Position;
            Acceleration = Acceleration.normalized;
            Acceleration *= MaxAcceleration;
            
            Position += Velocity * Time.deltaTime;
            Velocity += Acceleration * Time.deltaTime;
            distance = (target.Position - Position).magnitude; //wallAvoidDance
            
            if (distance < RadioInterior){
                
                Velocity = Vector3.zero; //wallAvoidDance
            }else if(Velocity.magnitude > MaxSpeed){
                
                Velocity = Velocity.normalized;
                Velocity *= MaxSpeed;
            }
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