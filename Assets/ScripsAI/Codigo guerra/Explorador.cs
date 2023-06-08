using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorador
{
    // Start is called before the first frame update
    private int pj;
    private int rangoVision = 11;
    private int centroVision;
    private int indexSpawn;
    private int[,] mVision;
    private int[] limVision;
    private const int MAXVALOR = 99;
    private const int MINVALOR = 0;

    private int com;
    private int lastCom;
    private int posI;
    private int posJ;

    public const int MOVERSE = 20;
    public const int HUIR = 21;
    public const int QUIETO = 23;

    public Explorador(int index){

        mVision = new int[rangoVision,rangoVision];
        centroVision = (rangoVision - 1)/2;
        com = Explorador.QUIETO;
        pj = index;
    }
    public int getIndexNPC(){

        return pj;
    }
    public void setLimites(int i, int j){

        limVision = setLimitesVision(i,j);
        posI = i;
        posJ = j;


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
        int x1 = 0;
        int y1 = 0;
        double  distancia = 999999;
        List<Objetivo> listaN = new List<Objetivo>();
        List<Objetivo> listaR = new List<Objetivo>();

        
        for (int k = 0; k < lisObj.Count; k++)
        {
            if (lisObj[k].getPropiedad() == Objetivo.NEUTRAL)
            {
                listaN.Add(lisObj[k]);
            }else if(lisObj[k].getPropiedad() == Objetivo.ROJO){
                
                listaR.Add(lisObj[k]);
            }
        }
        if (listaN.Count > 0)
        {
           int index = Random.Range(0, listaN.Count-1);
            foreach (Coordenada cr in listaN[index].getSlots())
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
        }else if(listaN.Count > 0)
        {
            int index = Random.Range(0, listaR.Count-1);
            foreach (Coordenada cr in listaR[index].getSlots())
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
        }else{

            int index = Random.Range(0, lisObj.Count-1);
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
        
         if(posicionObjetivo(listaObjetivos,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Explorador.MOVERSE;
            
        }else if(enemigosEnVision(mundo.getArray(),out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Explorador.HUIR;

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
