using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public List<Agent> selectedNPCs; //Lista de los agentes
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    private GameObject obj;

    private GameObject objForm;
    private FormationManager formacion;
    private bool enFormacion = false;
    private GameObject objRombo;
    private RomboFormation rombo;
    private GameObject puntero = null; // puntero a instanciar


    void Update()
    {
        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   

           
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
                    if(agent.inFormacion == true){
                        agent.inFormacion = false;
                        formacion.removeCharacter((AgentNPC) agent);


                    }
                    agent.setTarget(npcVirtualPrefab);

                }

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

        if (Input.GetKey(KeyCode.R)){

            if (!enFormacion){
                
                enFormacion = true;
                Agent lider = selectedNPCs[0];

                objForm = new GameObject("Formacion_" + lider);
                formacion = objForm.AddComponent<FormationManager>();

                formacion.lider = (AgentNPC) lider;
                formacion.agentes = new AgentNPC[3];
                formacion.slotAssignments = new List<SlotAssignment>();

                objRombo = new GameObject("Rombo_" + lider);
                rombo = objRombo.AddComponent<RomboFormation>();
                formacion.pattern = rombo;
                for (int i=1;i<selectedNPCs.Count;i++)
                {
                    if (selectedNPCs[i] != lider){
                        formacion.addCharacter((AgentNPC) selectedNPCs[i]);
                        selectedNPCs[i].quitarMarcador();
                        selectedNPCs[i].inFormacion = true;
                    }
                }
                selectedNPCs.Clear();
                selectedNPCs.Add(lider);

            }

        }
 
    }
}