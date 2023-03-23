using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovimientoMouse : MonoBehaviour
{
    public List<Agent> selectedNPCs; //Lista de los agentes
    public List<FormationManager> formaciones;
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    private GameObject obj;

    private GameObject objUno;
    private Formacion1 uno;
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

        foreach (FormationManager formacion in formaciones.ToList()){

            if (formacion.slotAssignments.Count == 0){
                formaciones.Remove(formacion);
                Destroy(formacion.pattern.gameObject);   
                Destroy(formacion.gameObject);
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

        if (Input.GetKeyDown(KeyCode.Alpha1)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            if (getFormacion(lider) != null)
                getFormacion(lider).removeCharacter(lider);

            string nombre = "";
            for (int i=1;i<selectedNPCs.Count;i++)
                nombre = nombre + "_" + selectedNPCs[i]; 

            objForm = new GameObject("Formacion1_" + lider + nombre);
            formacion = objForm.AddComponent<FormationManager>();
            formacion.lider = lider;
            lider.inFormacion = true;

            formacion.slotAssignments = new List<SlotAssignment>();

            objUno = new GameObject("Uno_" + lider + nombre);
            uno = objUno.AddComponent<Formacion1>();
            formacion.pattern = uno;

            for (int i=1;i<selectedNPCs.Count;i++)
            {
                if (selectedNPCs[i] != lider){
                    // Si el agente ya estaba en una formación, se elimina de dicha formación
                    if (getFormacion(selectedNPCs[i]) != null)
                    {   
                        getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);
                    }

                    // Se añade el agente a la formación
                    formacion.addCharacter(selectedNPCs[i]);
                    selectedNPCs[i].inFormacion = true;
                    
                }
            }

            formaciones.Add(formacion);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            if (getFormacion(lider) != null)
                getFormacion(lider).removeCharacter(lider);

            string nombre = "";
            for (int i=1;i<selectedNPCs.Count;i++)
                nombre = nombre + "_" + selectedNPCs[i]; 

            objForm = new GameObject("Formacion2_" + lider + nombre);
            formacion = objForm.AddComponent<FormationManager>();
            formacion.lider = lider;
            lider.inFormacion = true;

            formacion.slotAssignments = new List<SlotAssignment>();

            objDos = new GameObject("Dos_" + lider + nombre);
            dos = objDos.AddComponent<Formacion2>();
            formacion.pattern = dos;

            for (int i=1;i<selectedNPCs.Count;i++)
            {
                if (selectedNPCs[i] != lider){
                    // Si el agente ya estaba en una formación, se elimina de dicha formación
                    if (getFormacion(selectedNPCs[i]) != null)
                    {   
                        getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);
                    }

                    // Se añade el agente a la formación
                    formacion.addCharacter(selectedNPCs[i]);
                    selectedNPCs[i].inFormacion = true;
                    
                }
            }

            formaciones.Add(formacion);

        }
 
    }
}