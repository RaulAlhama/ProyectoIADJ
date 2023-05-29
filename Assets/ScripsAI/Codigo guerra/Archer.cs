using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Archer
{
    // Start is called before the first frame update
    private int pj;
    private int rangoVision = 11;
    private int rangoAtaque = 7;
    private int centroAtaque;
    private int centroVision;
    private int indexSpawn;
    private int[,] mAtaque;
    private int[,] mVision;
    private int[] limAtaque;
    private int[] limVision;
    private const int MAXVALOR = 99;
    private const int MINVALOR = 0;

    private int com;
    private int lastCom;
    private int posI;
    private int posJ;

    public const int MOVERSE = 20;
    public const int ATACAR = 21;
    public const int SEGUIR = 22;
    public const int QUIETO = 23;

    public Archer(int index){

        mAtaque = new int[rangoAtaque,rangoAtaque];
        mVision = new int[rangoVision,rangoVision];
        centroAtaque = (rangoAtaque - 1)/2;
        centroVision = (rangoVision - 1)/2;
        com = Archer.QUIETO;
        pj = index;
    }
    public int getIndexNPC(){

        return pj;
    }
    public void setLimites(int i, int j){

        limAtaque = setLimitesAtaque(i,j);
        limVision = setLimitesVision(i,j);
        posI = i;
        posJ = j;


    }
    private int[] setLimitesAtaque(int i, int j){

        int limiteInferior = i - centroAtaque;
        int limiteSuperior = i + centroAtaque;
        int limiteIzquierdo = j - centroAtaque;
        int limiteDerecho = j + centroAtaque;

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
        limites[0] = limiteInferior; // i inicio
        limites[1] = limiteSuperior; // i final
        limites[2] = limiteIzquierdo; // j inicio
        limites[3] = limiteDerecho; // j final

        return limites;
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
    private bool unidadPesadaEnVision(GridFinal valmundo,int[,] mundo, out int x,out int y){

        bool aliado = false;
        int x1 = 0;
        int y1 = 0;
        for (int i = limVision[0]; i < limVision[1]; i++)
        {
            for (int j = limVision[2]; j < limVision[3]; j++)
            {
                if (mundo[i,j] == ArrayUnidades.UNIDADPESADAAZUL)
                {
                    aliado = true;
                    x1 = i;
                    y1 = j;
                }
            }
        }
        List<Coordenada> slots = new List<Coordenada>();
        if(x1-1 >= MINVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1-1,y1-1)){

            Coordenada cr = new Coordenada(x1-1,y1-1);
            slots.Add(cr);
        }
        if(y1-1 >= MINVALOR && valmundo.Posible(x1,y1-1) ){

            Coordenada cr = new Coordenada(x1,y1-1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1+1,y1-1)){

            Coordenada cr = new Coordenada(x1+1,y1-1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && valmundo.Posible(x1-1,y1)){

            Coordenada cr = new Coordenada(x1-1,y1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1)){

            Coordenada cr = new Coordenada(x1+1,y1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1-1,y1+1)){

            Coordenada cr = new Coordenada(x1-1,y1+1);
            slots.Add(cr);
        }
        if( y1+1 <= MAXVALOR && valmundo.Posible(x1,y1+1)){

            Coordenada cr = new Coordenada(x1,y1+1);
            slots.Add(cr);
        }
        if( x1+1 <= MAXVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1+1)){

            Coordenada cr = new Coordenada(x1+1,y1+1);
            slots.Add(cr);
        }

        double distancia = 999999;

        foreach (Coordenada cr in slots)
        {
            double disAux = Mathf.Max(Mathf.Abs(posI-cr.getX()),Mathf.Abs(posJ-cr.getY()));
            if (disAux < distancia)
            {
                distancia = disAux;
                x1 = cr.getX();
                y1 = cr.getY();
            }
        }
        x = x1;
        y = y1;
        return aliado;
    }
    public bool posicionObjetivo(Objetivo[] lisObj, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = false;
        int menor = 99;
        int index = 0;
        int x1 = 0;
        int y1 = 0;
        double  distancia = 999999;
        for (int k = 0; k < lisObj.Length; k++)
        {
            if(lisObj[k].getPropiedad() == Objetivo.NEUTRAL){

                if (lisObj[k].getPrioridad() < menor)
                {
                    menor = lisObj[k].getPrioridad();
                    index = k;
                }
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
    public Vector3 getDecision(GridFinal mundo,Objetivo[] listaObjetivos, int[,] posNPCs, int i, int j){

        int x = i;
        int y = j;
        Vector3 target = mundo.getPosicionReal(x,y);
        
        if(enemigosEnVision(mundo.getArray(),out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.ATACAR;

        }else if(unidadPesadaEnVision(mundo, posNPCs,out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.SEGUIR;
            //Debug.Log("Arquero "+pj+": "+x+","+y);
        }else if(posicionObjetivo(listaObjetivos,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.MOVERSE;
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
