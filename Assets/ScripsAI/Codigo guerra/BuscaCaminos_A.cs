using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscaCaminos_A: MonoBehaviour
{
    public AgentNPC pl;
    public AEstrella buscador;
    public Agent npcVirtual;

        
    public BuscaCaminos_A(GridFinal wrld,AgentNPC p,Agent npv){

        pl = p;
        buscador = new AEstrella(wrld,pl);
        npcVirtual = npv;

    }

    public void setObjetivos(int i,int j,Agent npcVr){

        buscador.setObjetivos(i,j, npcVr);
    }
    public void setGrafoMovimiento(double[,] grMov){

        buscador.setGrafoMovimiento(grMov);
    }
    public List<Vector3> A(){

        return buscador.aestrella();
    }

    public void comprobarCamino(List<Vector3> caminosAzul){

        buscador.comprobarCamino(caminosAzul);
    }
}
