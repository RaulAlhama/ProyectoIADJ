using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public List<GameObject> selectedNPCs; //Lista de los agentes
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro

    private Agent npcVirtual = null; // target virtual a instanciar
    private GameObject puntero = null; // puntero a instanciar
    private bool selected; //booleano que nos indica si se ha hecho clic en un lugar para ejecutar el movimiento de los agentes.
    public GameObject indicadorPrefab = null;

    void Update()
    {
        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   
            if (puntero != null){ //Si existe puntero, lo eliminamos y también el targetVirtual
                Destroy(puntero);
                Destroy(npcVirtual.gameObject);
            }

           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
            targetPosition = hit.point; //Nos quedamos con la posición donde hemos hecho click
            targetPosition.y = 0;
            npcVirtualPrefab.transform.position = targetPosition; //Asignamos esa posición al target Virtual
            punteroPrefab.transform.position = targetPosition; //Se la asignamos también al puntero

            npcVirtual = Instantiate(npcVirtualPrefab); //creamos el target virtual
            puntero = Instantiate(punteroPrefab);// creamos el puntero
            
            selected = true;

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
                    if (selectedNPCs.Contains(npcObject))
                    {
                        selectedNPCs.Remove(npcObject);
                    }
                    else
                    {
                        selectedNPCs.Add(npcObject);
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
                    if (selectedNPCs.Count > 0){
                    selectedNPCs.Clear();
                    }

                    selectedNPCs.Add(npcObject);
                } 
                else{
                    selectedNPCs.Clear();
                }
            }
        }


        // Mueve los NPC's seleccionados al destino
        foreach (GameObject npcObject in selectedNPCs)
        {
            if(selected){
            AgentPlayer player = npcObject.GetComponent<AgentPlayer>();
            player.target = npcVirtualPrefab; //Asignamos al target del agente, el target Virtual
            }
            

        }
    }
}