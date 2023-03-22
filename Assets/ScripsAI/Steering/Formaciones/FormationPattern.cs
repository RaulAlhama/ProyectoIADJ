using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationPattern : MonoBehaviour
{

    public abstract DriftOffset getSlotLocation(int slotNumber);

    public abstract bool supportsSlots(int slotCount);
    
}
