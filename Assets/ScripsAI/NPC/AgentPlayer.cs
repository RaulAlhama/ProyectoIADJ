using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;

public class AgentPlayer : Agent
{
    public Agent target;
    public bool select = false;
    private ObstacleAvoidance obstacleAvoidance;
    private float distance;
    private Vector3 relativeTarget;
    public const int NOSELECTED = 0;
    public const int SELECTED = 1;
    public const int MOVING = 2;
    public const int STOPPED = 3;
    public int status = NOSELECTED;




    public virtual void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.
        switch (status){

            case NOSELECTED:{

                break;
            }
            case SELECTED:{

                esperaTarget();
                break;
            }
            case MOVING:{

                aplicaMovimiento();
                break;
            }
            case STOPPED:{
                esperaNuevaTarget();
                break;
            }

        }
        
    }

    void OnMouseDown(){
        
        if(!select){

            select = true;
            if(status == STOPPED || status == NOSELECTED)
                setStatus(SELECTED);
        }else{

            if(status == SELECTED || status == STOPPED)
                setStatus(NOSELECTED);
            select = false;
        }
    }
    private void setStatus(int value){

        status = value;
    }
    private void aplicaMovimiento(){

        
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
                target.Position = Position;
                setStatus(STOPPED);

            }else if(Velocity.magnitude > MaxSpeed){
                
                Velocity = Velocity.normalized;
                Velocity *= MaxSpeed;
                setStatus(MOVING);
            }
            transform.LookAt(transform.position + Velocity);
            
            Orientation = transform.rotation.eulerAngles.y; // DESCOMENTA !!
    }
    private void esperaTarget(){

        if(target != null){

            if(obstacleAvoidance== null){

                distance = (target.Position - Position).magnitude;
                obstacleAvoidance = new ObstacleAvoidance(target, this);

               
            }
            setStatus(MOVING);
        }
    }
    private void esperaNuevaTarget(){

        if(target.Position != Position){

            setStatus(MOVING);
        }
    }

}