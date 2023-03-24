using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFindingLRTA : MonoBehaviour
{
    // Start is called before the first frame update
    private GridFinal mundo;
    private AgentPlayer player;
    private Agent npcVirtual;
    private double[,] grafoMovimiento;
    private const double infinito = Double.PositiveInfinity;
    private int iObjetivo;
    private int jObjetivo;
    struct Coordenadas{
        public int x;
        public int y;
    }
    struct nAbierto{

        public Coordenadas corde;
        public double valor;
    }
    public PathFindingLRTA(GridFinal wld, AgentPlayer pl){

        mundo = wld;
        player = pl;

    }
    public void setPosicionNpcVirtual(Vector3 pos){

        npcVirtual.transform.position = pos;
    }
    public void setGrafoMovimiento(double[,] grm){

        grafoMovimiento = grm;
    }
    public void LRTAestrella(){

        int i;
        int j;

        mundo.getCoordenadas(player.Position,out i,out j);
        List<Coordenadas> puntos = new List<Coordenadas>();
        Coordenadas puntoActual = new Coordenadas();
        
        puntoActual.x = i;
        puntoActual.y = j;

        puntos = generaEspacioLocal(puntoActual);
        puntoActual = actualizaPesos(puntos, puntoActual);
        Vector3 aux = mundo.getPosicionReal(puntoActual.x,puntoActual.y) + new Vector3(1,0,1);
        npcVirtual.transform.position = aux;
        player.setTarget(npcVirtual);
        if(puntoActual.x == iObjetivo && puntoActual.y == jObjetivo)
            player.setLLegada(true);

    }
    private List<Coordenadas> generaEspacioLocal(Coordenadas puntoActual){

        int i = puntoActual.x;
        int j = puntoActual.y;
        List<Coordenadas> puntos = new List<Coordenadas>();
        
        if(grafoMovimiento[i,j] != infinito){ // centro

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j;
            puntos.Add(nPunto);
            if(grafoMovimiento[i,j] != 0)
                grafoMovimiento[i,j] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i-1,j] != infinito){ // izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j;
            puntos.Add(nPunto);
            if(grafoMovimiento[i-1,j] != 0)
                grafoMovimiento[i-1,j] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i-1,j+1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ // arriba-izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j+1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i-1,j+1] != 0)
                grafoMovimiento[i-1,j+1] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i+1,j+1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ // arriba-derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j+1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i+1,j+1] != 0)
                grafoMovimiento[i+1,j+1] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i-1,j-1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ // abajo-izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j-1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i-1,j-1] != 0)
                grafoMovimiento[i-1,j-1] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i+1,j-1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ // abajo-derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j-1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i+1,j-1] != 0)
                grafoMovimiento[i+1,j-1] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i,j+1] != infinito){ // arriba

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j+1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i,j+1] != 0)
                grafoMovimiento[i,j+1] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i+1,j] != infinito){ // derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j;
            puntos.Add(nPunto);
            if(grafoMovimiento[i+1,j] != 0)
                grafoMovimiento[i+1,j] = Double.PositiveInfinity;

        }
        if(grafoMovimiento[i,j-1] != infinito){ // abajo

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j-1;
            puntos.Add(nPunto);
            if(grafoMovimiento[i,j-1] != 0)
                grafoMovimiento[i,j-1] = Double.PositiveInfinity;

        }
        return puntos;
    }
    private Coordenadas actualizaPesos(List<Coordenadas> puntos, Coordenadas puntoActual){

        int tamLista = puntos.Count;
        int indice = 0;
        int continua = puntos.Count;

        

        while (continua != 0){

            Coordenadas auxPunto = puntos[indice];
            List<nAbierto> rutasP = new List<nAbierto>();
            double menor = infinito;

            if(grafoMovimiento[auxPunto.x,auxPunto.y] != infinito){

                continua--;
            }

            if(grafoMovimiento[auxPunto.x - 1,auxPunto.y + 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // arriba izquierda

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x - 1;
                pAux.y = auxPunto.y + 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x-1,auxPunto.y + 1] + 1.4f;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x,auxPunto.y + 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // arriba

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x;
                pAux.y = auxPunto.y + 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x,auxPunto.y + 1] + 1;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x + 1,auxPunto.y + 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // arriba derecha

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x + 1;
                pAux.y = auxPunto.y + 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x+1,auxPunto.y + 1] + 1.4f;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x + 1,auxPunto.y] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // derecha

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x + 1;
                pAux.y = auxPunto.y;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x + 1,auxPunto.y] + 1;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x + 1,auxPunto.y - 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // abajo derecha

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x + 1;
                pAux.y = auxPunto.y - 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x+1,auxPunto.y-1] + 1.4f;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x,auxPunto.y - 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // abajo

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x;
                pAux.y = auxPunto.y - 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x,auxPunto.y - 1] + 1;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x -1,auxPunto.y - 1] < grafoMovimiento[auxPunto.x,auxPunto.y]){ // abajo izquierda

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x - 1;
                pAux.y = auxPunto.y - 1;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x - 1,auxPunto.y - 1] + 1.4f;
                rutasP.Add(aAux);

            }
            if(grafoMovimiento[auxPunto.x - 1,auxPunto.y] <  grafoMovimiento[auxPunto.x,auxPunto.y]){ // izquierda

                nAbierto aAux = new nAbierto();
                Coordenadas pAux = new Coordenadas();
                pAux.x = auxPunto.x - 1;
                pAux.y = auxPunto.y;
                aAux.corde = pAux; 
                aAux.valor = grafoMovimiento[auxPunto.x - 1,auxPunto.y] + 1;
                rutasP.Add(aAux);

            }

            foreach(nAbierto p in rutasP){

                if(p.valor < menor){

                    menor = p.valor;
                    grafoMovimiento[auxPunto.x,auxPunto.y] = menor;
                }
            }
            
            indice++;
            if(indice == tamLista){

                indice = 0;
            }
            
        }
        double sigMenor = infinito;
        foreach(Coordenadas cp in puntos){

            if(sigMenor > grafoMovimiento[cp.x,cp.y]){

                sigMenor = grafoMovimiento[cp.x,cp.y];
                puntoActual.x = cp.x;
                puntoActual.y = cp.y;
            }
                
        }
        return puntoActual;

    }
    public void setObjetivos(int i,int j,Agent npcVr){

        iObjetivo = i;
        jObjetivo = j;
        npcVirtual = npcVr;
    }
}
