using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFormation : FormationPattern
{

    public override DriftOffset getSlotLocation(int slotNumber){
        
        Vector3 v;
        switch(slotNumber) 
        {
        case 0:
            v = new Vector3(4,0,-4);
            break;
        case 1:
            v = new Vector3(-4,0,-4);
            break;
        case 2:
            v = new Vector3(8,0,-0);
            break;
        case 3:
            v = new Vector3(-8,0,0);
            break;
         default:
            v = Vector3.zero;
            break;
        }

        return new DriftOffset(v,0.0f);
    
    }

    public override bool supportsSlots(int slotCount){
        return slotCount<=4;
    }

}
