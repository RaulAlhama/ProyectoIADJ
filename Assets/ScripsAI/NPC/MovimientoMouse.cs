using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public List<Agent> selectedNPCs; //Lista de los agentes
    private Vector3 targetPosition; // posición donde haremos click
    private Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    private GameObject obj;

    private GameObject puntero = null; // puntero a instanciar

    void Start()
    {
        obj = new GameObject("targetMouse");
        npcVirtualPrefab = obj.AddComponent<Agent>() as Agent;
    }

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
                

                //npcVirtual = Instantiate(npcVirtualPrefab); //creamos el target virtual
                puntero = Instantiate(punteroPrefab);// creamos el puntero
                Destroy(puntero, 0.5f);
                // Mueve los NPC's seleccionados al destino
                foreach (Agent agent in selectedNPCs)
                {
                    agent.setTarget(npcVirtualPrefab);
                                  
                }

            }
        }


        //Si pulsamos en un personaje mientras pulsamos control, se añadirá a la lista de personajes seleccionados
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
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
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
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
 
    }
}