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
    private bool firstTime;

    void Start(){
        firstTime = true;
        lider.isLider = true;
    }

    void Update(){

        updateSlots();
    }

    // Método para inicializar la lista de SlotAssignments
    public void updateSlotAssignments(){

        for (int i=0;i<slotAssignments.Count;i++){
            slotAssignments[i] = new SlotAssignment(slotAssignments[i].character, i);
        }

    }

    // Método para añadir un nuevo agente a la formación
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

    // Método para eliminar un agente de la formación
    public void removeCharacter(Agent agent){

        for (int i=0;i<slotAssignments.Count;i++){

            if (slotAssignments[i].character==agent){

                slotAssignments.Remove(slotAssignments[i]);
                updateSlotAssignments();
            }
        }

    }

    // Método para actualizar la posición y orientación de los agentes de la formación
    public void updateSlots(){

        for (int i=0;i<slotAssignments.Count;i++){
            
            if (lider.getStatus() != Agent.MOVING){

                DriftOffset relativeLoc = pattern.getSlotLocation(slotAssignments[i].slotNumber);       // Obtiene la posicion relativa en el patron dependiendo de su identificador

                if (!firstTime){
                    Destroy(GameObject.Find("target_" + slotAssignments[i].character));
                    slotAssignments[i].character.GetComponent<Arrive>().target.Position = Bodi.VectorRotate(relativeLoc.position,lider.Orientation) + lider.Position;
                    slotAssignments[i].character.GetComponent<Align>().target.Orientation = relativeLoc.orientation + lider.Orientation;
                }

                else {
                    GameObject gtarget = new GameObject("target_" + slotAssignments[i].character);               // Creamos el target
                    Agent targetf = gtarget.AddComponent<Agent>() as Agent;
                    targetf.Position = relativeLoc.position + lider.Position;
                    targetf.Orientation = relativeLoc.orientation + lider.Orientation;

                    slotAssignments[i].character.setTarget(targetf);
                }

            }

            else {
                
                slotAssignments[i].character.setTarget(lider);
                
            }


        }
        firstTime = false;
    }


}
