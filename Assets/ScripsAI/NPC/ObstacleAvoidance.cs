using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance
{
    private Vector3 target;
    private AgentPlayer player;

    public float avoidDistance = 11f;
    private float lookAhead = 5f;
    private float lookAheadSmall = 3f;

    private Vector3 targetRelativo;
    //private bool bandera = true;
    private Vector3 muroNormal;
    public ObstacleAvoidance(Vector3 target, AgentPlayer player){
            
        this.player = player;
        this.target = target;
    }
    public Vector3 getNuevoObjetivo(){
        
        Vector3 from = player.Position; // Origen de la línea
        Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

        from = from + elevation;

        Vector3 direction = player.transform.TransformDirection(Vector3.forward);
        Vector3 directionLeft = new Vector3 (Mathf.Cos(player.AnguloExterior * Mathf.Deg2Rad) * direction.x + Mathf.Sin(player.AnguloExterior * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin(player.AnguloExterior * Mathf.Deg2Rad) * direction.x + Mathf.Cos(player.AnguloExterior* Mathf.Deg2Rad) * direction.z); 
        Vector3 directionRight= new Vector3 (Mathf.Cos(-player.AnguloExterior * Mathf.Deg2Rad) * direction.x + Mathf.Sin(-player.AnguloExterior* Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin(-player.AnguloExterior * Mathf.Deg2Rad) * direction.x + Mathf.Cos(-player.AnguloExterior * Mathf.Deg2Rad) * direction.z); 

        Debug.DrawRay(from, direction * lookAhead);
        Debug.DrawRay(from, directionLeft * lookAheadSmall);
        Debug.DrawRay(from, directionRight * lookAheadSmall);

        int mask = 1 << 6;

        RaycastHit hitLeft;
        if (Physics.Raycast(from, directionLeft, out hitLeft, lookAheadSmall, mask)){
           
            Vector3 newTargetPosition = hitLeft.point + hitLeft.normal * avoidDistance;
            return new Vector3(newTargetPosition.x,0,newTargetPosition.z);
        }
        
        RaycastHit hitRight;
        if (Physics.Raycast(from, directionRight, out hitRight,lookAheadSmall, mask)){
            
            Vector3 newTargetPosition = hitRight.point + hitRight.normal * avoidDistance;
            return new Vector3(newTargetPosition.x,0,newTargetPosition.z);
        }
        
        RaycastHit hitFront;
        if (Physics.Raycast(from, direction,out hitFront ,lookAhead, mask)){
            
            Vector3 newTargetPosition = hitFront.point + hitFront.normal * avoidDistance;
            return new Vector3(newTargetPosition.x,0,newTargetPosition.z);
        }else{
            Debug.Log("Target Postion: " + target);
            return target;
        }
    }
}
