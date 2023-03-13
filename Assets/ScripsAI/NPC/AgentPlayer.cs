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

    public GameObject indicadorPrefab = null;
    private GameObject indicador = null;
    private bool targetCambiado = false;





    public void Update()
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
                if(indicador == null){
                    setStatus(NOSELECTED);
                }
                esperaNuevaTarget();
                break;
            }

        }
        
    }


    public override void activarMarcador(){
        indicador = Instantiate(indicadorPrefab, transform);
        indicador.transform.localPosition = Vector3.up * 4;
        if (status != MOVING)
            setStatus(SELECTED);
    }

    public override void quitarMarcador(){
        if (indicador != null){
            Destroy(indicador);
        }
        
    }

    private void setStatus(int value){

        status = value;
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

}