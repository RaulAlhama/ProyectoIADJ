using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYouGoing : Align
{
    void Start()
    {
        this.nameSteering = "LookWhereYouGoing";
    }

    public override Steering GetSteering(AgentNPC agent) {
        if (target.Velocity.magnitude == 0){
            Debug.Log("LookWhere.cs: NO hay velocidad");
            Steering steer = new Steering();
            return steer;
        }
        //Predecimos la posición del target
        Vector3 predictedPosition = this.target.Position + target.Velocity; //Posición relativa del target
        Vector3 newDirection = predictedPosition - agent.Position; //Dirección donde se encuentra el target relativo
        //Con PositionToAngle obtenemos la posición del tarjet predicho
        //Obtenemos la rotación
        explTargetRotation = Bodi.PositionToAngle(newDirection) - agent.Orientation;
        this.isExplicitTarget = true;
        //Debug.Log("LookWhere.cs: " + "Custom Rotation: " + explTargetRotation);
        

        return base.GetSteering(agent);
    }
}
