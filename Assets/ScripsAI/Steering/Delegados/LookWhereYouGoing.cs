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
        Vector3 predictedPosition = target.Velocity - agent.Position;
        //Con PositionToAngle obtenemos la posición del tarjet predicho
        //Obtenemos la rotación
        explTargetRotation = Bodi.PositionToAngle(predictedPosition) - agent.Orientation;
        Debug.Log("LookWhere.cs: " + "Custom Rotation: " + explTargetRotation);
        this.isExplicitTarget = true;

        return base.GetSteering(agent);
    }
}
