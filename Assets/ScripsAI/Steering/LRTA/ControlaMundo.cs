using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlaMundo : MonoBehaviour
{
    private GridFinal mundo;
    public Agent npcVirtual;
    public AgentPlayer player;
    public GameObject puntero;
    //private Object copiaAgent;
    private GameObject copiaPuntero;
    private int i;
    private int j;
    private double[,] grafoMovimiento;
    private GameObject[] obstaculos;
    private const double infinito = Double.PositiveInfinity;
    public Node aNodo;
    //private bool siguiente = true;

    struct Coordenadas{
        public int x;
        public int y;
    }
    struct nAbierto{

        public Coordenadas corde;
        public double valor;
    }

    // Start is called before the first frame update
    void Start()
    {
        obstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
        grafoMovimiento = new double[21,21];
        mundo = new GridFinal(21,21,2);
        player = Instantiate(player);
        player.Position = new Vector3(11,0,39);
        mundo.setValor(player.Position,GridFinal.PLAYER);
        mundo.setObstaculos(obstaculos);
        grafoMovimiento = mundo.getGrafo();

    }
    void Update()
    {

        if(Input.GetMouseButtonDown(1)){

            LRTAestrella();
            //player.setPath();
            /*RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    
                    if(copiaPuntero != null){

                        Destroy(copiaPuntero);
                    }
                    Vector3 aux = hit.point;
                    aux.y = 0;
                    npcVirtual.transform.position = aux;
                    puntero.transform.position = aux;

                    copiaPuntero = (GameObject) Instantiate(puntero);

                    player.target = npcVirtual;
                    
            }*/
        }else if(player.status == AgentPlayer.STOPPED){

            LRTAestrella();
        }
    }
    public void LRTAestrella(){

        //int iObjetivo = 7;
        //int jObjetivo = 19;
        int i;
        int j;
        //int tamLista;
        //int k = 0;

        mundo.getCoordenadas(player.Position,out i,out j);
        List<Coordenadas> puntos = new List<Coordenadas>();
        Coordenadas puntoActual = new Coordenadas();
        
        puntoActual.x = i;
        puntoActual.y = j;

        puntos = generaEspacioLocal(puntoActual);
        puntoActual = actualizaPesos(puntos, puntoActual);
        Vector3 aux = mundo.getPosicionReal(puntoActual.x,puntoActual.y) + new Vector3(1,0,1);
        npcVirtual.transform.position = aux;
        player.target = npcVirtual;
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
}
