using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Explorer
{
    // Start is called before the first frame update
    private AgentNPC pj;
    private int rangoVision = 15;
    private int centroVision;
    private int indexSpawn;
    private int[,] mVision;
    private int[] limVision;
    private const int MAXVALOR = 99;
    private const int MINVALOR = 0;

    private int com;
    private int lastCom;

    public const int MOVERSE = 20;
    public const int SEGUIR = 22;
    public const int QUIETO = 23;
    public const int HUIR = 24;

    public Explorer(){

        mVision = new int[rangoVision,rangoVision];
        centroVision = (rangoVision - 1)/2;
        com = Explorer.QUIETO;
    }
    public void setLimites(int i, int j){

        limVision = setLimitesVision(i,j);


    }
    private int[] setLimitesVision(int i, int j){

        int limiteInferior = i - centroVision;
        int limiteSuperior = i + centroVision;
        int limiteIzquierdo = j - centroVision;
        int limiteDerecho = j + centroVision;

        int[] limites = new int[4];

        if(limiteInferior < MINVALOR){

            limiteInferior = MINVALOR;
        }
        if(limiteSuperior > MAXVALOR){

            limiteSuperior = MAXVALOR;
        }
        if(limiteIzquierdo < MINVALOR){

            limiteIzquierdo = MINVALOR;
        }
        if(limiteDerecho > MAXVALOR){

            limiteDerecho = MAXVALOR;
        }
        limites[0] = limiteInferior;
        limites[1] = limiteSuperior;
        limites[2] = limiteIzquierdo;
        limites[3] = limiteDerecho;

        return limites;
    }
    private bool enemigosEnVision(int[,] mundo, out int x, out int y){

        bool enemigos = false;
        int x1 = 0;
        int y1 = 0;
        for (int i = limVision[0]; i < limVision[1]; i++)
        {
            for (int j = limVision[2]; j < limVision[3]; j++)
            {
                if (mundo[i,j] == GridFinal.NPCROJO)
                {
                    enemigos = true;
                    x1 = i;
                    y1 = j;
                    
                }
            }
        }
        x = x1;
        y = y1;
        return enemigos;
    }
    public bool posicionObjetivo(List<Objetivo> lisObj, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = false;
        int menor = 99;
        int index = 0;
        int k = 0;
        int x1 = 0;
        int y1 = 0;
        double  distancia = 999999;
        foreach (Objetivo obj in lisObj)
        {
            if(obj.getPropiedad() == Objetivo.NEUTRAL){

                if (obj.getPrioridad() < menor)
                {
                    menor = obj.getPrioridad();
                    index = k;
                }
                k++;
            }
        }
        foreach (Coordenada cr in lisObj[index].getSlots())
        {
            if(PosMundo[cr.getX(),cr.getY()] == 0){

                double disAux = Mathf.Max(Mathf.Abs(i-cr.getX()),Mathf.Abs(j-cr.getY()));
                if (disAux < distancia)
                {
                    distancia = disAux;
                    objetivo = true;
                    x1 = cr.getX();
                    y1 = cr.getY();
                }
            }
        }
        x = x1;
        y = y1;
        return objetivo;
    }
    // Update is called once per frame
    public Vector3 getDecision(GridFinal mundo,List<Objetivo> listaObjetivos, int[,] posNPCs, int i, int j){

        int x = i;
        int y = j;
        Vector3 target = mundo.getPosicionReal(x,y);
        
        if(enemigosEnVision(mundo.getArray(),out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Explorer.HUIR;

        }else if(posicionObjetivo(listaObjetivos,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Explorer.MOVERSE;
        }
        //buscar objetivo que no esta en propiedad y con menor prioridad (siginifa que es el mas cercano a la base) 
        
        return target;
    }
    public bool cambioCom(){

        if(lastCom == com){

            return false;
        }else{

            return true;
        }
    }
}
