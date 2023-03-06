using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlotAssignment
{
    public Agent character;
    public int slotNumber;

    public SlotAssignment(Agent character, int slotNumber)
    {
        this.character = character;
        this.slotNumber = slotNumber;
    }
}


[System.Serializable]
public struct DriftOffset
{

    public Vector3 position;
    public float orientation;

    public DriftOffset(Vector3 position, float orientation){
        this.position = position;
        this.orientation = orientation;
    }
    
}

public class FormationManager : MonoBehaviour
{

    public FormationPattern pattern;
    public List<SlotAssignment> slotAssignments;
    public Agent lider;
    //public DriftOffset driftOffset;

    public void updateSlotAssignments(){

        for (int i=0;i<slotAssignments.Count;i++){
            slotAssignments[i] = new SlotAssignment(slotAssignments[i].character, i);
        }

        //driftOffset = pattern.getDriftOffset(slotAssignments);
    }

    public bool addCharacter(Agent agent){

        int occupiedSlots = slotAssignments.Count;

        if (pattern.supportsSlots(occupiedSlots + 1)){

            SlotAssignment slotAssignment = new SlotAssignment(agent, occupiedSlots);
            slotAssignments.Add(slotAssignment);

            updateSlotAssignments();

            return true;
        }

        return false;
    }

    public void removeCharacter(Agent agent){

        for (int i=0;i<slotAssignments.Count;i++){

            if (slotAssignments[i].character==agent){

                slotAssignments.Remove(slotAssignments[i]);
                updateSlotAssignments();
            }
        }        
    }

    /*public void updateSlots(){

        for (int i=0;i<slotAssignments.Count;i++){

            DriftOffset anchor =  getAnchorPoint(slotAssignments[i].character); // Esto es la posicion respecto al lider pero deberia ser respecto al centor de masas

            Vector3 orientationMatrix = Bodi.AngleToPosition(anchor.orientation); // La matriz pone como se calcula en la diapositiva
            
            DriftOffset relativeLoc = pattern.getSlotLocation(slotAssignments[i].slotNumber);

            DriftOffset location = new DriftOffset();
            location.position = relativeLoc.position * orientationMatrix + anchor.position;
            location.oriention = anchor.oriention + relativeLoc.oriention;

            //location.position -= driftOffset.position;
            //location.orientation -= driftOffset.orientation;

            slotAssignments[i].character.setTarget(location);

        }
    }*/

    public DriftOffset getAnchorPoint(Agent agent){
       
        Vector3 position = lider.Position - agent.Position;
        position += Bodi.AngleToPosition(lider.Orientation);
        DriftOffset anchor = new DriftOffset(position,lider.Orientation);
        return anchor;

    }

}
