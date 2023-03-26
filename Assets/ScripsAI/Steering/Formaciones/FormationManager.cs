using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlotAssignment
{
    public Agent character;
    public int slotNumber;
    public Agent target;
    

    public SlotAssignment(Agent character, int slotNumber)
    {
        this.character = character;
        this.slotNumber = slotNumber;
        this.target = new Agent();
    }
    public SlotAssignment(Agent character, int slotNumber, Agent trg)
    {
        this.character = character;
        this.slotNumber = slotNumber;
        this.target = trg;
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
    private bool liderFollowing = true;

    void Start(){
        firstTime = true;
        lider.isLider = true;
    }

    void Update(){

        
        updateSlots();
    }
    public void setComportamiento(bool value){

        liderFollowing = value;
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

                //slotAssignments[i].character.Velocity = Vector3.zero;
                DriftOffset relativeLoc = pattern.getSlotLocation(slotAssignments[i].slotNumber);       // Obtiene la posicion relativa en el patron dependiendo de su identificador

                if (!firstTime){
                    
                    if(liderFollowing){

                        if(slotAssignments[i].character is AgentNPC){
                            
                            AgentNPC peon = (AgentNPC)slotAssignments[i].character;
                            peon.virtualTarget.Position = Bodi.VectorRotate(relativeLoc.position,lider.Orientation) + lider.Position; //Arrive
                            peon.virtualTarget.Orientation = relativeLoc.orientation + lider.Orientation; //Align
                            peon.Velocity = Vector3.zero;
                            //peon.Rotation = 0; 
                        
                        }
                    }else{

                        Destroy(GameObject.Find("target_" + slotAssignments[i].character));

                        GameObject gtarget = new GameObject("target_" + slotAssignments[i].character);  
                        Agent targetf = gtarget.AddComponent<Agent>() as Agent;
                        targetf.Position = Bodi.VectorRotate(relativeLoc.position,lider.Orientation) + lider.Position;
                        targetf.Orientation = relativeLoc.orientation + lider.Orientation;
                        slotAssignments[i] = new SlotAssignment(slotAssignments[i].character, i, targetf);
                    }
                    
                        
                }  else {
                    
                    if(liderFollowing){

                        GameObject gtarget = new GameObject("target_" + slotAssignments[i].character);               // Creamos el target
                        Agent targetf = gtarget.AddComponent<Agent>() as Agent;
                        targetf.Position = relativeLoc.position + lider.Position;
                        targetf.Orientation = relativeLoc.orientation + lider.Orientation;
                        slotAssignments[i].character.setTarget(targetf);
                        Destroy(GameObject.Find("target_" + slotAssignments[i].character));
                    }else{

                        GameObject gtarget = new GameObject("target_" + slotAssignments[i].character);               // Creamos el target
                        Agent targetf = gtarget.AddComponent<Agent>() as Agent;
                        targetf.Position = relativeLoc.position + lider.Position;
                        targetf.Orientation = relativeLoc.orientation + lider.Orientation;

                        slotAssignments[i] = new SlotAssignment(slotAssignments[i].character, i, targetf);
                    }
                    
                }

            }

            else {
                
                if(liderFollowing)
                    slotAssignments[i].character.setTarget(lider);
                
            }


        }
        firstTime = false;
    }


}
