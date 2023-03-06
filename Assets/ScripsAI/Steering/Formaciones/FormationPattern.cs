using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationPattern : MonoBehaviour
{

    public int numberOfSlots;

    public virtual DriftOffset getSlotLocation(int slotNumber){
        return new DriftOffset(Vector3.zero,0.0f);
    }

    public virtual bool supportsSlots(int slotCount){
        return true;
    }
}
