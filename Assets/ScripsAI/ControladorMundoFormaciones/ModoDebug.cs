using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModoDebug : MonoBehaviour
{

    protected bool modoDebug = false;

    public virtual void Update(){

        if (Input.GetKeyDown(KeyCode.H))
            modoDebug = !modoDebug;
    }

    void OnDrawGizmos()
    {
                     
        if (modoDebug){

            AgentNPC[] agentes = FindObjectsOfType<AgentNPC>();

            foreach (Agent agente in agentes)
            {

                Vector3 from = agente.Position; // Origen de la línea
                Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

                from = from + elevation;

                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(from, agente.Velocity);

                float distanciaBigotesExteriores = agente.AnguloExterior/agente.getNumBigotes();
                float distanciaBigotesInteriores = agente.AnguloInterior/agente.getNumBigotes(); 
                
                for (int i=0;i<agente.getNumBigotes();i++){

                    // Mirando en la dirección de la orientación.
                    Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;

                    Gizmos.color = Color.red;
                    Vector3 vectorInterior1 = Bodi.VectorRotate(direction, agente.AnguloInterior-distanciaBigotesInteriores*i);
                    Vector3 vectorInterior2 = Bodi.VectorRotate(direction, -agente.AnguloInterior+distanciaBigotesInteriores*i);  
                    
                    Gizmos.DrawRay(from, vectorInterior1);
                    Gizmos.DrawRay(from, vectorInterior2);

                    // Dibujamos el angulo exterior
                    Vector3 vectorExterior3 = Bodi.VectorRotate(direction, agente.AnguloExterior-distanciaBigotesExteriores*i);
                    Vector3 vectorExterior4 = Bodi.VectorRotate(direction, -agente.AnguloExterior+distanciaBigotesExteriores*i); 
                    
                    Gizmos.color = Color.blue; 
                    Gizmos.DrawRay(from, vectorExterior3);
                    Gizmos.DrawRay(from, vectorExterior4);

                }

                // Dibujamos el circulo interior
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(agente.Position, agente.RadioInterior);

                // Dibujamos el circulo exterior
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(agente.Position, agente.RadioExterior);

            }

            Gizmos.color = Color.black;
            Collider[] colliders = FindObjectsOfType<Collider>();

            foreach (Collider collider in colliders)
            {
                Gizmos.DrawWireCube(collider.transform.position, collider.bounds.size);
            }


        }

    }
}
