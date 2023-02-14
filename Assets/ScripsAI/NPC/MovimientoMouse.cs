using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMouse : MonoBehaviour
{
    public Agent npcVirtual;
    public AgentPlayer player;
    public GameObject puntero;

    void Update()
    {

        if(player.selected && Input.GetMouseButtonDown(1)){
            
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    
                    Vector3 aux = hit.point;
                    aux.y = 0;
                    npcVirtual.transform.position = aux;
                    puntero.transform.position = aux;

                    Instantiate(npcVirtual);
                    Instantiate(puntero);
                    player.target = npcVirtual;
                    
            }
        }
    }
}
