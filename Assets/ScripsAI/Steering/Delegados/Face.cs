using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{

  void Start()
    {
        this.nameSteering = "Face";
        
    }

    public override Steering GetSteering(AgentNPC agent)
    {

        Vector3 newDirection = target.Position - agent.Position;

        //Bodi.PositionToAngle(newDirection) -> Target.Orientation
        explTargetRotation = Bodi.PositionToAngle(newDirection) - agent.Orientation;
        Debug.Log("Face.cs: " + "Custom Rotation: " + explTargetRotation);
        this.isExplicitTarget = true;

        return base.GetSteering(agent);

    }
}