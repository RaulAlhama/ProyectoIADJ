using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;



public class AgentPlayer : Agent
{
    public Agent target;
    public float ori;
    public bool selected = false;

    // Update is called once per frame
    public virtual void Update()
    {
        // Mientras que no definas las propiedades en Bodi esto seguirá dando error.

        if(selected){
            if(target != null){
            
            //Vector3 Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 Velocity = target.Position - Position;
            float distance = Velocity.magnitude;
            Velocity = Velocity.normalized;

            

        
            if (distance < RadioInterior)
        {
            Velocity = Vector3.zero;
        }else if(Velocity.magnitude > MaxSpeed){
                float dirX = Velocity.x / Mathf.Abs(Velocity.x);
                float dirZ = Velocity.z / Mathf.Abs(Velocity.z);
                Velocity = new Vector3(dirX * MaxSpeed,0,dirZ * MaxSpeed);
            }
                
            //Velocity *= MaxSpeed;  // DESCOMENTA !!
            
            Vector3 translation = Velocity * Time.deltaTime;
            transform.Translate(translation, Space.World);

            // Para el jugador usamos el SteeringBehaviour (LookAt)
            // que ya lleva implementado Unity.
            // Notar que al jugador le aplicamos un movimiento no-acelerado.
            transform.LookAt(transform.position + Velocity);
            
            Orientation = transform.rotation.eulerAngles.y; // DESCOMENTA !!
            ori = Orientation;

            }
            

        }
        
    }

    void OnMouseDown(){
        
        Debug.Log("collider");
        
        if(!selected){

            selected = true;
        }else{

            selected = false;
        }
    }


}
