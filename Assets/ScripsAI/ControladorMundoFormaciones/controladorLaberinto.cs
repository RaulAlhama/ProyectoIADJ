using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class controladorLaberinto : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Agent> selectedNPCs; //Lista de los agentes
    public List<FormationManager> formaciones;
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    private GameObject obj;

    private GameObject objUno;
    private Formacion1 uno;
    private GameObject objDos;
    private Formacion2 dos;
    private GameObject puntero = null; // puntero a instanciar
    public int dis = 1;
    public bool leaderFollowing=true;


    // LRTA

    private List<BuscaCaminos> buscadores;
    public GridFinal mundo;
    private GameObject copiaPuntero;
    private int iObjetivo;
    private int jObjetivo;
    private double[,] grafoMovimiento;
    private GameObject[] obstaculos;
    private const double infinito = Double.PositiveInfinity;
    public Node aNodo;
    public Agent npcVirtual;
    public AgentNPC player;

    struct Coordenadas{
        public int x;
        public int y;
    }
    struct nAbierto{

        public Coordenadas corde;
        public double valor;
    }
    struct BuscaCaminos{

        public AgentNPC pl;
        public PathFindingLRTA buscador;
        public Agent npcVirtual;
        public BuscaCaminos(AgentNPC p, PathFindingLRTA b, Agent v){

            pl = p;
            buscador = b;
            npcVirtual = v;

        }
    }
    private int tam=2;
    private void agregaPlayers(AgentNPC pl){

        mundo.setValor(pl.Position,GridFinal.PLAYER);
        PathFindingLRTA buscador = new PathFindingLRTA(mundo,pl);
        BuscaCaminos bc = new BuscaCaminos();
        bc.pl = pl;
        bc.buscador = buscador;
        bc.npcVirtual = Instantiate(npcVirtual);
        bc.npcVirtual.name = "NPCVirtual" + pl;
        buscadores.Add(bc);
    }
    void Start()
    {
        obstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
        grafoMovimiento = new double[21,21];
        mundo = new GridFinal(21,21,tam);
        mundo.setDistancia(dis);
        buscadores = new List<BuscaCaminos>();
        selectedNPCs = new List<Agent>();

        player = Instantiate(player);
        
        player.Position = new Vector3(11,0,39);
        
        player.name = "NPC1";
        

        player.liderFollowing = leaderFollowing;
        
        mundo.setObstaculos(obstaculos);
        
        agregaPlayers(player);

    }

    // Update is called once per frame
    void Update()
    {

        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
                if(copiaPuntero != null){

                    Destroy(copiaPuntero);
                }

                targetPosition = hit.point; //Nos quedamos con la posfción donde hemos hecho click
                targetPosition.y = 0.1f;
                npcVirtualPrefab.Position = targetPosition; //Asignamos esa posición al target Virtual 
                punteroPrefab.transform.position = targetPosition; //Se la asignamos también al puntero
                puntero = Instantiate(punteroPrefab);// creamos el puntero
                Destroy(puntero, 0.5f);
                // Mueve los NPC's seleccionados al destino

                    foreach (AgentNPC agent in selectedNPCs)
                    {   
                        foreach(BuscaCaminos bC in buscadores)
                        {

                            if(bC.pl.Equals(agent)){

                                int iObjetivo;
                                int jObjetivo;

                                mundo.getCoordenadas(targetPosition,out iObjetivo,out jObjetivo);
                                
                                if(mundo.Posible(iObjetivo,jObjetivo))
                                {
                                    bC.pl.setLLegada(false);
                                    //Debug.Log(mundo.getPosicionReal(iObjetivo,jObjetivo));

                                    bC.npcVirtual.transform.position = targetPosition;
                                    
                                    bC.buscador.setObjetivos(iObjetivo,jObjetivo, bC.npcVirtual);
                                    bC.buscador.setPosicionNpcVirtual(mundo.getPosicionReal(iObjetivo,jObjetivo));
                                    bC.buscador.setOrientacionNpcVirtual(bC.npcVirtual.Orientation);

                                    puntero.transform.position = mundo.getPosicionReal(iObjetivo,jObjetivo);

                                    bC.buscador.setGrafoMovimiento(mundo.getGrafo(iObjetivo,jObjetivo));
                                    copiaPuntero = (GameObject) Instantiate(puntero);
                            
                                    bC.buscador.LRTAestrella();
                                }
                            }
                        }

                    }
                
            }


        }else
        {

            foreach (AgentNPC agent in selectedNPCs){

                foreach(BuscaCaminos bC in buscadores){

                        if(bC.pl.Equals(agent) && (bC.pl.status == Agent.STOPPED) && !bC.pl.getLLegada()){
                            
                            bC.buscador.LRTAestrella();
                        }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                if (npcObject.CompareTag("NPC"))
                {
                    Agent selectAgent = npcObject.GetComponent<AgentNPC>();
                        
                        foreach(BuscaCaminos bC in buscadores){

                            if(bC.pl.Equals(selectAgent)){
                                bC.pl.activarMarcador();
                                selectedNPCs.Add(bC.pl);
                            }
                        }

                    
                


                } 
                
            }


        }
    }
}
