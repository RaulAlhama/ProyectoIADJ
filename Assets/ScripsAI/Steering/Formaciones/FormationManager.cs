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
    public Agent[] agentes;
    public AgentNPC movimiento;
    //public DriftOffset driftOffset;


    void Start(){

        for (int i=0;i<agentes.Length;i++){
            SlotAssignment slotAssignment = new SlotAssignment(agentes[i], i);
            slotAssignments.Add(slotAssignment);
        }

        updateSlots();
    }


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

    public void updateSlots(){

        Vector3 liderPos = lider.Position;

        for (int i=0;i<slotAssignments.Count;i++){

            DriftOffset relativeLoc = pattern.getSlotLocation(slotAssignments[i].slotNumber);       // Obtiene la posicion relativa en el patron dependiendo de su identificador
            Debug.Log("(" + i + ") relativeLoc: " + relativeLoc.position + ", liderPos: " + lider.Position);

            GameObject gtarget = new GameObject("target");               // Creamos el target
            Agent target = gtarget.AddComponent<Agent>() as Agent;

            GameObject garrive = new GameObject("target");               // Creamos el arrive
            Arrive arrive = garrive.AddComponent<Arrive>() as Arrive;
 

            slotAssignments[i].character.Position = relativeLoc.position + liderPos;

        }
    }


}
