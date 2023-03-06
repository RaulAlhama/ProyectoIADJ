using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationPattern : MonoBehaviour
{

    public int numberOfSlots;

    public abstract DriftOffset getSlotLocation(int slotNumber);

    public abstract bool supportsSlots(int slotCount);
    
}
