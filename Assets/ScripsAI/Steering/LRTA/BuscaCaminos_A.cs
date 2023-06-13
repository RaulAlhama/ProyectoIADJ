using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Clase que representa un npc que utiliza el algoritmo A*
public class BuscaCaminos_A
{
    public AgentNPC pl;
    public AEstrella buscador;
    public Agent npcVirtual;
        
    public BuscaCaminos_A(GridFinal wrld,AgentNPC p,Agent npv){

        pl = p;
        buscador = new AEstrella(wrld,pl);
        npcVirtual = npv;

    }

    // Función para asignar un objetivo al NPC
    public void setObjetivos(int i,int j,Agent npcVr){

        buscador.setObjetivos(i,j, npcVr);
    }

    // Función para cambiar el valor de su grafo de movimiento
    public void setGrafoMovimiento(double[,] grMov){

        buscador.setGrafoMovimiento(grMov);
    }

    // Función que calcula el camino óptimo a su objetivo
    public List<Vector3> A(int[,] peligro){

        return buscador.aestrella(peligro);
    }

    // Función que comprueba el estado del camino óptimo a su objetivo
    public void comprobarCamino(List<Vector3> caminosAzul){

        buscador.comprobarCamino(caminosAzul);
    }
}
