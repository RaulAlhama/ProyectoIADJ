using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathFollowing : Agent
{
    // Start is called before the first frame update
    //public Agent target;
    public PathFollowing pathFollowing;
    private float distance;
    private Vector3 relativeTarget;

    public Path camino;

    // Update is called once per frame
    public virtual void Start(){

         pathFollowing = new PathFollowing(this,camino); //pathFollowing
         relativeTarget = pathFollowing.getSiguienteObjetivo(); //pathFollowing: obtenemos la posición del nodo (el más cercano si es el primero)
    }
    public virtual void Update()
    {
        Acceleration = relativeTarget - Position;
        Acceleration = Acceleration.normalized;
        Acceleration *= MaxAcceleration;
        
        Position += Velocity * Time.deltaTime;
        Velocity += Acceleration * Time.deltaTime;
        distance = (relativeTarget - Position).magnitude; //pathFollowing
    
        if (distance < RadioExterior){

             relativeTarget = pathFollowing.getSiguienteObjetivo(); //pathFollowing

         }else if(Velocity.magnitude > MaxSpeed){

            Velocity = Velocity.normalized;
            Velocity *= MaxSpeed;
         }
         transform.LookAt(transform.position + Velocity);
         
         Orientation = transform.rotation.eulerAngles.y; // DESCOMENTA !!
        
    }
}
