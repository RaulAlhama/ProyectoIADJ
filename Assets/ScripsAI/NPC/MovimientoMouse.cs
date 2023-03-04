using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public List<GameObject> selectedNPCs;
    private Vector3 targetPosition;
    public Agent npcVirtual;
    public GameObject puntero;
    private bool selected;

    void Update()
    {
        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
            targetPosition = hit.point;
            targetPosition.y = 0;
            npcVirtual.transform.position = targetPosition;
            puntero.transform.position = targetPosition;
            Instantiate(npcVirtual);
            Instantiate(puntero);
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
            player.target = npcVirtual;
            }
            

        }
    }
}