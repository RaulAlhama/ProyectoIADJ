using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscaCaminos: MonoBehaviour
{
    public AgentNPC pl;
    public PathFindingLRTA buscador;
    public Agent npcVirtual;

        
    public BuscaCaminos(GridFinal wrld,AgentNPC p,Agent npv){

        pl = p;
        buscador = new PathFindingLRTA(wrld,pl);
        npcVirtual = npv;

    }

    public void setObjetivos(int i,int j,Agent npcVr){

        buscador.setObjetivos(i,j, npcVr);
    }
    public void setGrafoMovimiento(double[,] grMov){

        buscador.setGrafoMovimiento(grMov);
    }
    public void LRTA(){

        buscador.LRTAestrella();
    }
}
