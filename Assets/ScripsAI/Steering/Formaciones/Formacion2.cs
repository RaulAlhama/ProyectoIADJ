using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formacion2 : FormationPattern
{

    public override DriftOffset getSlotLocation(int slotNumber){
        
        Vector3 v;
        switch(slotNumber) 
        {
        case 0:
            v = new Vector3(2,0,-2);
            return new DriftOffset(v,0.0f);
         case 1:
            v = new Vector3(-2,0,-2);
            return new DriftOffset(v,0.0f);
         case 2:
            v = new Vector3(2,0,2);
            return new DriftOffset(v,180.0f);
        case 3:
            v = new Vector3(-2,0,2);
            return new DriftOffset(v,45.0f);
         case 4:
            v = new Vector3(-1,0,-3);
            return new DriftOffset(v,-45.0f);
         default:
            v = Vector3.zero;
            return new DriftOffset(v,0.0f);
        }

        
    
    }

    public override bool supportsSlots(int slotCount){
        return slotCount<=5;
    }

}
