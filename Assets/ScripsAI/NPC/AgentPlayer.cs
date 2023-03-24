using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;

public class AgentPlayer : Agent
{
    public Agent target;
    private ObstacleAvoidance obstacleAvoidance;
    private float distance;
    private Vector3 relativeTarget;

    public GameObject indicadorPrefab;
    private bool targetCambiado = false;

    private float tiempo = 0;
    private bool objetivo = false;



    public void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.
        switch (status){

            case NOSELECTED:{
                if(select){
                    select= false;
                }
                indicadorPrefab.SetActive(false);
                break;
            }
            case SELECTED:{

                if(!select){
                    select = true;
                }
                indicadorPrefab.SetActive(true);
                esperaTarget();
                break;
            }
            case MOVING:{
                tiempo = 0;
                aplicaMovimiento();
                break;
            }
            case STOPPED:{
                
                tiempo += Time.deltaTime;
                esperaNuevaTarget();
                break;
            }

        }
        
    }
    void OnMouseDown(){
        
        if(!select){

            select = true;
            indicadorPrefab.SetActive(true);
            if(status == STOPPED || status == NOSELECTED)
                setStatus(SELECTED);
        }else{
            if(status == SELECTED || status == STOPPED)
                setStatus(NOSELECTED);
            select = false;
            indicadorPrefab.SetActive(false);
        }
    }
    public override void setTarget(Agent virtualTargetPrefab ){
            //virtualTarget = Instantiate(virtualTargetPrefab);
        targetCambiado = true;
        target = virtualTargetPrefab;
        obstacleAvoidance = new ObstacleAvoidance(target.Position, this);
    }
        
        

    private void aplicaMovimiento(){

        
            //Vector3 Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            relativeTarget = obstacleAvoidance.getNuevoObjetivo(); //wallAvoidDance
            /*Acceleration = relativeTarget - Position;
            Acceleration = Acceleration.normalized;
            Acceleration *= MaxAcceleration;*/
            
            Velocity += relativeTarget - Position;
            Position += Velocity * Time.deltaTime;
            //Velocity += Acceleration * Time.deltaTime;
            distance = (target.Position - Position).magnitude; //wallAvoidDance
            if (distance < RadioExterior){
                if(objetivo && distance < RadioInterior)
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

        if(target != null && targetCambiado == true){
            targetCambiado = false;
            if(obstacleAvoidance== null){

                distance = (target.Position - Position).magnitude;

               
            }
            setStatus(MOVING);
        }
    }
    private void esperaNuevaTarget(){

        if(target.Position != Position && targetCambiado == true){
            targetCambiado = false;
            setStatus(MOVING);
        }
    }
    public void setLLegada(bool valor){

        objetivo = valor;
    }
    public bool getLLegada(){

        return objetivo;
    }

}