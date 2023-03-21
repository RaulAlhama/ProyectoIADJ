using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public List<Agent> selectedNPCs; //Lista de los agentes
    public List<FormationManager> formaciones;
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    private GameObject obj;

    private GameObject objRombo;
    private Formacion1 rombo;
    private GameObject objDos;
    private Formacion2 dos;
    private GameObject puntero = null; // puntero a instanciar

    public FormationManager getFormacion(Agent agent){
        foreach (FormationManager formacion in formaciones){
            foreach (SlotAssignment slot in formacion.slotAssignments){
                if (slot.character == agent)
                {
                    return formacion;
                }
            }

            if (formacion.lider == agent)
                return formacion;
        }
        return null;
    }


    public void comprobarFormacion(){
        foreach (FormationManager formacion in formaciones){
            foreach (SlotAssignment slot in formacion.slotAssignments){
                
                if (slot.character.getTarget() != null){
                    
                    Vector3 newDirection =  slot.character.getTarget().Position - slot.character.Position;
                    float distance = newDirection.magnitude;
                    if (distance < slot.character.RadioInterior)
                    {
                        slot.character.formar = true;
                    }
                } 
                    
            }

        }
    }



    void Update()
    {

        // Comprobamos si los agentes tienen que volver a formar
        comprobarFormacion();

        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   
            
            bool romperFormacion = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){

                targetPosition = hit.point; //Nos quedamos con la posfción donde hemos hecho click
                targetPosition.y = 0;
                npcVirtualPrefab.Position = targetPosition; //Asignamos esa posición al target Virtual 
                punteroPrefab.transform.position = targetPosition; //Se la asignamos también al puntero
                puntero = Instantiate(punteroPrefab);// creamos el puntero
                Destroy(puntero, 0.5f);
                // Mueve los NPC's seleccionados al destino
                foreach (Agent agent in selectedNPCs)
                {   

                    // Si un agente de los seleccionados no está en formación, se debe romper la formación
                    if (agent.inFormacion == false){
                        romperFormacion = true;
                    }

                    else{
                        agent.formar = false;
                    }

                    agent.setTarget(npcVirtualPrefab);

                }

                // Comprobamos si hay que romper la formación
                if (romperFormacion){

                    foreach (Agent agent in selectedNPCs)
                    {
                        // Se cambia el estado del agente
                        agent.inFormacion = false;
                        // Si el personaje está en una formación, esta se destruye
                        if (getFormacion(agent) != null){
                            Destroy(getFormacion(agent).pattern.gameObject);
                            Destroy(getFormacion(agent).gameObject);
                        }
                    }
                    
                    

                }

                romperFormacion = false;

            }



        }


        //Si pulsamos en un personaje mientras pulsamos control, se añadirá a la lista de personajes seleccionados
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                if (npcObject.CompareTag("NPC"))
                {
                    Agent selectAgent;
                    if(npcObject.GetComponent<AgentPlayer>() != null)
                        selectAgent = npcObject.GetComponent<AgentPlayer>();
                    else
                        selectAgent = npcObject.GetComponent<AgentNPC>();
            
                    if (selectedNPCs.Contains(selectAgent))
                    {
                        selectAgent.quitarMarcador();
                        selectedNPCs.Remove(selectAgent);
                    }
                    else
                    {
                        selectAgent.activarMarcador();
                        selectedNPCs.Add(selectAgent);
                    }
                }
                
            }
        }
        //Si pulsamos en un personaje y no tenemos pulsado CONTROL la lista solo contendrá a este personaje
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                if (npcObject.CompareTag("NPC"))
                {
                    Agent selectAgent;
                    if(npcObject.GetComponent<AgentPlayer>() != null)
                        selectAgent = npcObject.GetComponent<AgentPlayer>();
                    else
                        selectAgent = npcObject.GetComponent<AgentNPC>();
                    if (selectedNPCs.Count > 0){ //Si hay varios personajes seleccionados, los deseleccionamos 
                        foreach (Agent otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();
                        }
                        selectedNPCs.Clear();
                    }
                    
                    selectedNPCs.Add(selectAgent);
                    selectAgent.activarMarcador();
                } 
                else{
                    foreach (Agent otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();
                        }
                    selectedNPCs.Clear();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            if (GameObject.Find("Formacion_" + lider) == null)
            {
                objForm = new GameObject("Formacion_" + lider);
                formacion = objForm.AddComponent<FormationManager>();

                formacion.lider = lider;
                lider.inFormacion = true;
                lider.formar = true;

                formacion.slotAssignments = new List<SlotAssignment>();

                objRombo = new GameObject("Rombo_" + lider);
                rombo = objRombo.AddComponent<Formacion1>();
                formacion.pattern = rombo;
                for (int i=1;i<selectedNPCs.Count;i++)
                {
                    if (selectedNPCs[i] != lider){
                        
                        // Si el agente ya estaba en una formación, se elimina de dicha formación
                        if (getFormacion(selectedNPCs[i]) != null)
                            getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);

                        // Se añade el agente a la formación
                        formacion.addCharacter(selectedNPCs[i]);
                        selectedNPCs[i].inFormacion = true;
                        selectedNPCs[i].formar = true;
                    }
                }
                //selectedNPCs.Clear();
                //selectedNPCs.Add(lider);

                //}

                formaciones.Add(formacion);
            
            }

            /*
            else {
                Destroy(GameObject.Find("Formacion_" + lider).gameObject);
                Destroy(GameObject.Find("Rombo_" + lider).gameObject);
            }*/

        }

        if (Input.GetKeyDown(KeyCode.Q)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            if (GameObject.Find("Formacion_" + lider) == null)
            {
                objForm = new GameObject("Formacion_" + lider);
                formacion = objForm.AddComponent<FormationManager>();

                formacion.lider = lider;
                lider.inFormacion = true;
                lider.formar = true;

                formacion.slotAssignments = new List<SlotAssignment>();

                objDos = new GameObject("Rombo_" + lider);
                dos = objDos.AddComponent<Formacion2>();
                formacion.pattern = dos;
                for (int i=1;i<selectedNPCs.Count;i++)
                {
                    if (selectedNPCs[i] != lider){
                        
                        // Si el agente ya estaba en una formación, se elimina de dicha formación
                        if (getFormacion(selectedNPCs[i]) != null)
                            getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);

                        // Se añade el agente a la formación
                        formacion.addCharacter(selectedNPCs[i]);
                        selectedNPCs[i].inFormacion = true;
                        selectedNPCs[i].formar = true;
                    }
                }
                //selectedNPCs.Clear();
                //selectedNPCs.Add(lider);

                //}

                formaciones.Add(formacion);
            
            }

            /*
            else {
                Destroy(GameObject.Find("Formacion_" + lider).gameObject);
                Destroy(GameObject.Find("Rombo_" + lider).gameObject);
            }*/

        }
 
    }
}