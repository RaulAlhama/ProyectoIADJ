using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrulla : MonoBehaviour
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

    public Patrulla(int index){

        mAtaque = new int[rangoAtaque,rangoAtaque];
        mVision = new int[rangoVision,rangoVision];
        centroAtaque = (rangoAtaque - 1)/2;
        centroVision = (rangoVision - 1)/2;
        com = Patrulla.QUIETO;
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
    public bool posicionRuta(Ruta ruta, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = false;
        int x1 = 0;
        int y1 = 0;

        while(!objetivo){

            WayPoint aux = ruta.getSiguiente();
            //Debug.Log(aux.getNombre());
            if (aux.getDisponible())
            {
                objetivo = true;
                x1 = aux.getX();
                y1 = aux.getY();
                Debug.Log(x1+","+y1);
            }
        }
        x = x1;
        y = y1;
        return objetivo;
    }
    // Update is called once per frame
    public Vector3 getDecision(GridFinal mundo,Ruta ruta, int[,] posNPCs, int i, int j){

        int x = i;
        int y = j;
        Vector3 target = mundo.getPosicionReal(x,y);
        
        if(enemigosEnVision(mundo.getArray(),out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Patrulla.ATACAR;

        }else if(posicionRuta(ruta,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Patrulla.MOVERSE;
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
