using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mundoGuerra : MonoBehaviour
{
    // Start is called before the first frame update
    private const int INDEXEXPLORADOR = 0;
    private const int INDEXARCHER1 = 1;
    private const int INDEXARCHER2 = 2;
    private const int INDEXPESADA1 = 3;
    private const int INDEXPESADA2 = 4;
    private const int INDEXVIGILANTE = 5;
    private const int INDEX_TORRE_VIGIA = 0;
    private const int INDEX_ARMERIA = 1;
    private const int INDEX_PUENTE_IZQUIERDO_AZUL = 2;
    private const int INDEX_PUENTE_DERECHO_AZUL = 3;
    private const int INDEX_SANTUARIO = 4;
    private const int INDEX_ESCUDERIA = 5;
    private const int INDEX_PUENTE_IZQUIERDO_ROJO = 6;
    private const int INDEX_PUENTE_DERECHO_ROJO = 7;
    
    //Comentario

    private bool verificando = false;
    public Material azul;
    public Material rojo;
    public int rows;
    public int cols;
    public float cellSize;
    public Agent npcVirtual;
    private GridFinal grFinal;
    public Color backgroundColor = Color.white;
    public GameObject wallPrefab;
    public GameObject[] torreVigia;
    public GameObject[] armeria;
    public GameObject[] puenteDerechoAzul;
    public GameObject[] puenteIzquierdoAzul;
    public GameObject[] puenteDerechoRojo;
    public GameObject[] puenteIzquierdoRojo;
    public GameObject[] santuario;
    public GameObject[] escuderia;
    

    //Barras de vida
    private GameObject[] vidaExploradorAzul;
    private GameObject[] vidaExploradorRojo;
    private GameObject[] vidaArquero1Azul;
    private GameObject[] vidaArquero1Rojo;
    private GameObject[] vidaArquero2Rojo;
    private GameObject[] vidaArquero2Azul;
    private GameObject[] vidaUnidadPesada1Rojo;
    private GameObject[] vidaUnidadPesada2Rojo;
    private GameObject[] vidaUnidadPesada1Azul;
    private GameObject[] vidaUnidadPesada2Azul;
    private GameObject[] vidaPatrullaRojo;
    private GameObject[] vidaPatrullaAzul;

    // NPC

    private GameObject[] Azules;
    private GameObject[] Rojos;


    private AgentNPC[] equipoRojo = new AgentNPC[6];
    private Agent[] npcVirtualRojo = new Agent[6];

    private AgentNPC[] equipoAzul = new AgentNPC[numNPC];
    private Agent[] npcVirtualAzul = new Agent[numNPC];
    private BuscaCaminos_A[] buscadoresAzul = new BuscaCaminos_A[numNPC];
    private BuscaCaminos_A[] buscadoresRojo = new BuscaCaminos_A[numNPC];


    public AgentNPC prefabNPCAzul;
    public AgentNPC prefabNPCRojo;
    private const int numNPC = 6;
    private const int numObjetives = 8;
    private Archer[] cArquero;
    private UnidadPesada[] cPesada;
    private Explorador cExplorador;
    private Patrulla cPatrulla;
    private Archer[] rArquero;
    private UnidadPesada[] rPesada;
    private Explorador rExplorador;
    private Patrulla rPatrulla;

    // Minimapa
    public GameObject prefabPlano;
    public Material materialEquipoRojo;
    public Material materialEquipoAzul;
    public Material materialNeutro; 
    public Material materialPeleado;
    private GameObject[,] casillas_minimapa = new GameObject[34, 34];
    private GameObject minimapa;

    // informacion
    
    private ArrayUnidades unidadesAzul;
    private ArrayUnidades unidadesRojo;
    private ArrayEnemigos enemigosTeamBlue;
    private ArrayEnemigos enemigosTeamRed;

    //objetivos del mundo

    private Objetivo[] objetivosMundo;
    private List<Objetivo> objExplorerBlue;
    private List<Objetivo> objExplorerRed;
    private List<Objetivo> objetivosTeamBlue;
    private List<Objetivo> objetivosTeamRed;
    
    private Coordenada[] spawnAzul = new Coordenada[6];
    private Posicion[] teamBlue = new Posicion[6];
    private Coordenada[] spawnRojo = new Coordenada[6];
    private Posicion[] teamRed = new Posicion[6];

    // waypoint explorador azul
    private WayPoint[] puntosAzul = new WayPoint[10];
    private WayPoint[] puntosAzul2 = new WayPoint[13];
    private WayPoint[] puntosRojo = new WayPoint[10];
    private WayPoint[] puntosRojo2 = new WayPoint[13];
    private Ruta rutaAzul;
    private Ruta rutaAzulPesada;
    private Ruta rutaRoja;
    private Ruta rutaRojaPesada;

    private bool todosInBlue = false;
    private bool todosInRed = false;

    // Arrays de caminos
    private List<Vector3>[] caminosAzul = new List<Vector3>[numNPC];
    private List<Vector3>[] caminosRojo = new List<Vector3>[numNPC];

    private TextMesh[,] grid;

    // Selección de personajes
    AgentNPC selectAgent;

    //Modo Debug

    protected bool modoDebug = false;
    private List<GameObject> listaWayPoints = new List<GameObject>();
    public GameObject prefabWayPoint;
    [SerializeField]
    private Text debugNombre, debugComportamiento, debugOjetivo, debugVida;

    void Start()
    {
        cArquero = new Archer[2];
        cPesada = new UnidadPesada[2];
        rArquero = new Archer[2];
        rPesada = new UnidadPesada[2];
        unidadesAzul = new ArrayUnidades(rows,cols);
        unidadesRojo = new ArrayUnidades(rows,cols);
        enemigosTeamBlue = new ArrayEnemigos(rows,cols);
        enemigosTeamRed = new ArrayEnemigos(rows,cols);
        objetivosMundo = new Objetivo[numObjetives];
        objetivosTeamBlue = new List<Objetivo>();
        objetivosTeamRed = new List<Objetivo>();
        objExplorerBlue = new List<Objetivo>();
        objExplorerRed = new List<Objetivo>();
        grFinal = new GridFinal(rows,cols,cellSize);
        grFinal.setDistancia(2);

        //Barras de vida
        vidaExploradorAzul = new GameObject[5];
        vidaExploradorRojo = new GameObject[5];
        vidaArquero1Azul = new GameObject[5];
        vidaArquero2Azul = new GameObject[5];
        vidaArquero1Rojo = new GameObject[5];
        vidaArquero2Rojo = new GameObject[5];
        vidaUnidadPesada1Azul = new GameObject[5];
        vidaUnidadPesada2Azul = new GameObject[5];
        vidaUnidadPesada1Rojo = new GameObject[5];
        vidaUnidadPesada2Rojo = new GameObject[5];
        vidaPatrullaAzul = new GameObject[5];
        vidaPatrullaRojo = new GameObject[5];

        // NPCs
        Azules = new GameObject[numNPC];
        Rojos = new GameObject[numNPC];


        for(int i = 0; i < numNPC; i++){

            spawnAzul[i] = new Coordenada(190 + (i*4),10);
            spawnRojo[i] = new Coordenada(190 + (i*4),390);
        }
        for(int i = 0; i < numNPC; i++){

            int iAux;
            int jAux;

            equipoAzul[i] = Instantiate(prefabNPCAzul);
            npcVirtualAzul[i] = Instantiate(npcVirtual);
            npcVirtualAzul[i].name = "NPCVirtual" + equipoAzul[i];
            equipoAzul[i].virtualTarget = npcVirtualAzul[i];
            equipoAzul[i].setStatus(Agent.STOPPED);

            buscadoresAzul[i] = new BuscaCaminos_A(grFinal,equipoAzul[i],npcVirtualAzul[i]);
            equipoAzul[i].Position = new Vector3(spawnAzul[i].getX(),0,spawnAzul[i].getY());
            grFinal.getCoordenadas(equipoAzul[i].Position,out iAux,out jAux);
            teamBlue[i] = new Posicion(iAux,jAux);

            equipoRojo[i] = Instantiate(prefabNPCRojo);
            npcVirtualRojo[i] = Instantiate(npcVirtual);
            npcVirtualRojo[i].name = "NPCVirtual" + equipoRojo[i];
            equipoRojo[i].virtualTarget = npcVirtualRojo[i];
            equipoRojo[i].setStatus(Agent.STOPPED);
            buscadoresRojo[i] = new BuscaCaminos_A(grFinal,equipoRojo[i],npcVirtualRojo[i]);

            equipoRojo[i].Position = new Vector3(spawnRojo[i].getX(),0,spawnRojo[i].getY());
            grFinal.getCoordenadas(equipoRojo[i].Position,out iAux,out jAux);
            teamRed[i] = new Posicion(iAux,jAux);
        }
        //setTipos();
        
        setWaypointsBlue();
        setWaypointsRed();
        setRutas();
        setTipos();
        setBarrasDeVida();
        setNPCs();
        setPosiciones();
        setTorreVigia();
        setArmeria();
        setPuenteIzqAzul();
        setPuenteDerAzul();
        setPuenteDerRojo();
        setPuenteIzqRojo();
        setSantuario();
        setEscuderia();
        inicializarMinimapa();
        
     
    }

   
    private void setWaypointsBlue(){
        
        // way points lado azul izquierdo

        Posicion p1 = new Posicion(49,10);
        puntosAzul[0] = new WayPoint(true,p1);

        Posicion p2 = new Posicion(41,10);
        puntosAzul[1] = new WayPoint(true,p2);

        Posicion p3 = new Posicion(41,13);
        puntosAzul[2] = new WayPoint(true,p3);

        Posicion p4 = new Posicion(29,13);
        puntosAzul[3] = new WayPoint(true,p4);

        Posicion p5 = new Posicion(16,16);  // torre vigia
        puntosAzul[4] = new WayPoint(false,p5,WayPoint.TORRE_VIGIA);

        Posicion p6 = new Posicion(29,23);
        puntosAzul[5] = new WayPoint(true,p6);

        Posicion p7 = new Posicion(32,23);
        puntosAzul[6] = new WayPoint(true,p7);

        Posicion p8 = new Posicion(32,35);
        puntosAzul[7] = new WayPoint(true,p8);

        Posicion p9 = new Posicion(14,35);
        puntosAzul[8] = new WayPoint(true,p9);

        Posicion p10 = new Posicion(11,46);  // puente izquierdo 
        puntosAzul[9] = new WayPoint(false,p10,WayPoint.PUENTE_IZQUIERDO_AZUL);

        // way points lado azul derecho

        Posicion p11 = new Posicion(48,22);  // armeria
        puntosAzul2[0] = new WayPoint(false,p11,WayPoint.ARMERIA);

        Posicion p13 = new Posicion(57,11);  // deshabilitar si armeria habilitado
        puntosAzul2[1] = new WayPoint(true,p13,WayPoint.CON_ARMERIA);

        Posicion p12 = new Posicion(57,13);  
        puntosAzul2[2] = new WayPoint(true,p12);

        Posicion p14 = new Posicion(70,13); 
        puntosAzul2[3] = new WayPoint(true,p14); 

        Posicion p15 = new Posicion(70,8);  
        puntosAzul2[4] = new WayPoint(true,p15);

        Posicion p16 = new Posicion(84,9);  
        puntosAzul2[5] = new WayPoint(true,p16);

        Posicion p17 = new Posicion(84,19);  
        puntosAzul2[6] = new WayPoint(true,p17);

        Posicion p18 = new Posicion(86,19);  
        puntosAzul2[7] = new WayPoint(true,p18);

        Posicion p19 = new Posicion(85,29); 
        puntosAzul2[8] = new WayPoint(true,p19);

        Posicion p20 = new Posicion(88,29);   
        puntosAzul2[9] = new WayPoint(true,p20);

        Posicion p21 = new Posicion(87,39);  
        puntosAzul2[10] = new WayPoint(true,p21);

        Posicion p22 = new Posicion(90,39); 
        puntosAzul2[11] = new WayPoint(true,p22);

        Posicion p23 = new Posicion(89,46);  // puente derecho 
        puntosAzul2[12] = new WayPoint(false,p23,WayPoint.PUENTE_DERECHO_AZUL);

    }
    private void setWaypointsRed(){
        
        // way points lado rojo izquierdo

        Posicion p1 = new Posicion(50,89);
        puntosRojo[0] = new WayPoint(true,p1);

        Posicion p2 = new Posicion(58,89);
        puntosRojo[1] = new WayPoint(true,p2);

        Posicion p3 = new Posicion(58,86);
        puntosRojo[2] = new WayPoint(true,p3);

        Posicion p4 = new Posicion(70,86);
        puntosRojo[3] = new WayPoint(true,p4);

        Posicion p5 = new Posicion(86,76);  // santuario
        puntosRojo[4] = new WayPoint(false,p5,WayPoint.SANTUARIO);

        Posicion p6 = new Posicion(70,76);
        puntosRojo[5] = new WayPoint(true,p6);

        Posicion p7 = new Posicion(67,76);
        puntosRojo[6] = new WayPoint(true,p7);

        Posicion p8 = new Posicion(68,65);
        puntosRojo[7] = new WayPoint(true,p8);

        Posicion p9 = new Posicion(87,64);
        puntosRojo[8] = new WayPoint(true,p9);

        Posicion p10 = new Posicion(87,53);  // puente izquierdo enemigo
        puntosRojo[9] = new WayPoint(false,p10,WayPoint.PUENTE_IZQUIERDO_ROJO);

        // way points lado rojo derecho

        Posicion p11 = new Posicion(41,89);  
        puntosRojo2[0] = new WayPoint(false,p11);

        Posicion p12 = new Posicion(41,86);
        puntosRojo2[1] = new WayPoint(true,p12);

        Posicion p13 = new Posicion(41,75); // escuderia  
        puntosRojo2[2] = new WayPoint(false,p13,WayPoint.ESCUDERIA);

        Posicion p14 = new Posicion(30,87); 
        puntosRojo2[3] = new WayPoint(true,p14); 

        Posicion p15 = new Posicion(29,91);  
        puntosRojo2[4] = new WayPoint(true,p15);

        Posicion p16 = new Posicion(16,90);  
        puntosRojo2[5] = new WayPoint(true,p16);

        Posicion p17 = new Posicion(16,80);  
        puntosRojo2[6] = new WayPoint(true,p17);

        Posicion p18 = new Posicion(13,80);  
        puntosRojo2[7] = new WayPoint(true,p18);

        Posicion p19 = new Posicion(14,70); 
        puntosRojo2[8] = new WayPoint(true,p19);

        Posicion p20 = new Posicion(11,70);   
        puntosRojo2[9] = new WayPoint(true,p20);

        Posicion p21 = new Posicion(12,61);  
        puntosRojo2[10] = new WayPoint(true,p21);

        Posicion p22 = new Posicion(9,60); 
        puntosRojo2[11] = new WayPoint(true,p22);

        Posicion p23 = new Posicion(9,53);  // puente derecho enemigo
        puntosRojo2[12] = new WayPoint(false,p23,WayPoint.PUENTE_DERECHO_ROJO);
    }
    
    private void setRutas(){

        rutaRoja = new Ruta(puntosRojo,puntosRojo2,puntosAzul2,puntosAzul,false);
        rutaRojaPesada = new Ruta(puntosRojo,puntosRojo2,puntosAzul2,puntosAzul,false);
        rutaAzul = new Ruta(puntosAzul,puntosAzul2,puntosRojo,puntosRojo2,true);
        rutaAzulPesada = new Ruta(puntosAzul,puntosAzul2,puntosRojo2,puntosRojo,true);
    }
    
    // Función que inicializa todas las casillas del minimapa a neutro, 
    // recorriendo cada una de las casillas del grid y creando un plano del tamaño de una casilla
    private void inicializarMinimapa(){

        //Vector3 ajuste = new Vector3(6,-0.2f,6);
        Vector3 ajuste = new Vector3(6,1f,6);
        minimapa = new GameObject("minimapa");
        int contx=0;
        int conty=0;

        // Recorremos todas las casillas del grid
        for (int j = 0; j < cols; j+=3){

            contx=0;

            for (int i = 0; i< rows; i+=3){

                // Creamos un objeto plano 
                GameObject plane = Instantiate(prefabPlano, minimapa.transform);
                plane.name = "minimap_" + contx + "_" + conty;
                // Lo guardamos en el array de casillas
                casillas_minimapa[contx,conty] = plane;
                // Establecemos su tamaño y posición
                plane.transform.localScale = new Vector3(cellSize/3.33f, 1f, cellSize/3.33f);
                plane.transform.position = grFinal.getPosicionReal(i,j) + ajuste;
                // Lo colocamos en la layer de no minimapa para que no se vea
                plane.layer = 8;
                // Le asignamos el material de casilla neutra
                plane.GetComponent<Renderer>().material = materialNeutro;
                plane.GetComponent<Renderer>().enabled = false;

                contx++;
            }

            conty++;
        }

    }

    // Función que actualiza las casillas de los objetivos del minimapa
    private void actualizarMinimapa(){

        // Recorremos los objetivos del mapa
        for (int z=0;z<objetivosMundo.Length;z++){

            // En caso de que un objetivo no sea neutral, coloreamos sus casillas del color del equipo
            if (objetivosMundo[z].getPropiedad() != Objetivo.NEUTRAL){

                int xinicial = objetivosMundo[z].getXInicial();
                int xfinal = objetivosMundo[z].getXFinal();
                int yinicial = objetivosMundo[z].getYInicial();
                int yfinal = objetivosMundo[z].getYFinal();

                for (int j=yinicial-2;j<=yfinal+2;j++){
                            
                    for (int i=xinicial-2;i<=xfinal+2;i++){

                        // Actualizamos las casillas del minimapa con el color de su equipo y la colocamos en la layer del minimapa
                        if (objetivosMundo[z].getPropiedad() == Objetivo.AZUL)
                        {
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().material = materialEquipoAzul;
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().enabled = true;
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].layer = 7;
                        }
                        else
                        {
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().material = materialEquipoRojo;
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().enabled = true;
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].layer = 7;
                        }
                    }

                }
            }
        }
    }

    private void setTorreVigia(){
        // Vigia 
        Coordenada[] coordes = new Coordenada[12];
        int k=0;
        for(int i=15;i<19;i++){

            coordes[k] = new Coordenada(12,i);
            k++;
        }
        for(int i=15;i<19;i++){

            coordes[k] = new Coordenada(15,i);
            k++;
        }
        for(int i=13;i<15;i++){

            coordes[k] = new Coordenada(i,15);
            k++;
            coordes[k] = new Coordenada(i,18);
            k++;
        }

        Objetivo vigia = new Objetivo(1,coordes,Objetivo.TORRE_VIGIA);
        objetivosMundo[INDEX_TORRE_VIGIA] = vigia;
        objExplorerBlue.Add(objetivosMundo[INDEX_TORRE_VIGIA]);

    }
    private void setSantuario(){

        Coordenada[] coordes = new Coordenada[16];
        int k=0;
        for(int i=74;i<79;i++){

            coordes[k] = new Coordenada(86,i);
            k++;
            coordes[k] = new Coordenada(90,i);
            k++;
        }
        for(int i=87;i<90;i++){

            coordes[k] = new Coordenada(i,74);
            k++;
            coordes[k] = new Coordenada(i,78);
            k++;
        }

        Objetivo santuario = new Objetivo(1,coordes,Objetivo.SANTUARIO);
        objetivosMundo[INDEX_SANTUARIO] = santuario;
        objExplorerRed.Add(objetivosMundo[INDEX_SANTUARIO]);

    }
    private void setEscuderia(){

        Coordenada[] coordes = new Coordenada[8];
        int k=0;
        for(int i=55;i<58;i++){

            coordes[k] = new Coordenada(23,i);
            k++;
            coordes[k] = new Coordenada(26,i);
            k++;
        }
        coordes[k] = new Coordenada(24,57);
        k++;
        coordes[k] = new Coordenada(25,57);
        

        Objetivo escud = new Objetivo(2,coordes,Objetivo.ESCUDERIA);
        objetivosMundo[INDEX_ESCUDERIA] = escud;
        objExplorerRed.Add(objetivosMundo[INDEX_ESCUDERIA]);

    }
    private void setArmeria(){

        Coordenada[] coordes = new Coordenada[9];
        int k=0;
        for(int i=40;i<43;i++){

            coordes[k] = new Coordenada(63,i);
            k++;
        }
        for(int i=64;i<67;i++){

            coordes[k] = new Coordenada(i,42);
            k++;
        }
        for(int i=40;i<42;i++){

            coordes[k] = new Coordenada(66,i);
            k++;
        }
        coordes[k] = new Coordenada(64,39);

        Objetivo armeria = new Objetivo(2,coordes,Objetivo.ARMERIA);
        objetivosMundo[INDEX_ARMERIA] = armeria;
        objExplorerBlue.Add(objetivosMundo[INDEX_ARMERIA]);
    }
    private void setPuenteIzqAzul(){

        Coordenada[] coordes = new Coordenada[18];
        int k=0;
        for (int i = 9; i < 15; i++)
        {
            for (int j = 44; j < 47; j++)
            {
                coordes[k] = new Coordenada(i,j);
                k++;
            }
        }
        Objetivo puenteIzqAzul = new Objetivo(3,coordes,Objetivo.PUENTE_IZQUIERDO_AZUL);
        //puenteIzqAzul.setPropiedad(Objetivo.AZUL);
        objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL] = puenteIzqAzul;
        objExplorerBlue.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]);
    }
    private void setPuenteDerAzul(){

        Coordenada[] coordes = new Coordenada[24];
        int k=0;
        for (int i = 87; i < 93; i++)
        {
            for (int j = 43; j < 47; j++)
            {
                coordes[k] = new Coordenada(i,j);
                k++;
            }
        }
        Objetivo puenteDerAzul = new Objetivo(3,coordes,Objetivo.PUENTE_DERECHO_AZUL);
        //puenteDerAzul.setPropiedad(Objetivo.AZUL);
        objetivosMundo[INDEX_PUENTE_DERECHO_AZUL] = puenteDerAzul;
        objExplorerBlue.Add(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]);
    }
    private void setPuenteDerRojo(){

        Coordenada[] coordes = new Coordenada[24];
        int k=0;
        for (int i = 7; i < 13; i++)
        {
            for (int j = 53; j < 57; j++)
            {
                coordes[k] = new Coordenada(i,j);
                k++;
            }
        }
        Objetivo puenteDerRojo = new Objetivo(3,coordes,Objetivo.PUENTE_DERECHO_ROJO);
        objetivosMundo[INDEX_PUENTE_DERECHO_ROJO] = puenteDerRojo;
        objExplorerRed.Add(objetivosMundo[INDEX_PUENTE_DERECHO_ROJO]);
    }
    private void setPuenteIzqRojo(){

        Coordenada[] coordes = new Coordenada[18];
        int k=0;
        for (int i = 85; i < 91; i++)
        {
            for (int j = 53; j < 56; j++)
            {
                coordes[k] = new Coordenada(i,j);
                k++;
            }
        }
        Objetivo puenteIzqRojo = new Objetivo(3,coordes,Objetivo.PUENTE_IZQUIERDO_ROJO);
        //puenteIzqRojo.setPropiedad(Objetivo.AZUL);
        objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO] = puenteIzqRojo;
        objExplorerRed.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO]);
    }
    private void setUnidades(AgentNPC npc, int eq){

        int x = 0;
        int y = 0;

        switch (npc.getTipo())
        {
            case AgentNPC.ARQUERO:
                
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesAzul.setUnidad(x,y,ArrayUnidades.ARQUEROAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesRojo.setUnidad(x,y,ArrayUnidades.ARQUEROROJO);
                }
            break;
            case AgentNPC.PESADA:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesAzul.setUnidad(x,y,ArrayUnidades.UNIDADPESADAAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesRojo.setUnidad(x,y,ArrayUnidades.UNIDADPESADAROJO);
                }
            break;
            case AgentNPC.EXPLORADOR:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesAzul.setUnidad(x,y,ArrayUnidades.EXPLORADOAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesRojo.setUnidad(x,y,ArrayUnidades.EXPLORADORROJO);
                }
            break;
            case AgentNPC.PATRULLA:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesAzul.setUnidad(x,y,ArrayUnidades.PATRULLAAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidadesRojo.setUnidad(x,y,ArrayUnidades.PATRULLAROJO);
                }
            break;
        }
    }

    private void setPosiciones(){

        for(int i=0;i<numNPC;i++){

            setUnidades(equipoAzul[i],0);
            setUnidades(equipoRojo[i],1);
            grFinal.setValor(equipoAzul[i].Position,GridFinal.NPCAZUL);
            grFinal.setValor(equipoRojo[i].Position,GridFinal.NPCROJO);
        }
        
        // Base Rojo

        grFinal.setValor((49),(98),GridFinal.ESTATUAROJA);
        grFinal.setValor((50),(98),GridFinal.ESTATUAROJA);
        grFinal.setValor((49),(99),GridFinal.ESTATUAROJA);
        grFinal.setValor((50),(99),GridFinal.ESTATUAROJA);

        for(int i=92;i<100;i++)
          grFinal.setValor((43),(i),GridFinal.OBSTACULO);
        for(int i=92;i<100;i++)
          grFinal.setValor((56),(i),GridFinal.OBSTACULO);
        for(int i=44;i<49;i++) 
            grFinal.setValor((i),(92),GridFinal.OBSTACULO);
        for(int i=51;i<56;i++) 
            grFinal.setValor((i),(92),GridFinal.OBSTACULO);

        // Base azul

        grFinal.setValor((49),(0),GridFinal.ESTATUAAZUL);
        grFinal.setValor((50),(0),GridFinal.ESTATUAAZUL);
        grFinal.setValor((49),(1),GridFinal.ESTATUAAZUL);
        grFinal.setValor((50),(1),GridFinal.ESTATUAAZUL);


        for(int i=0;i<8;i++)
          grFinal.setValor((43),(i),GridFinal.OBSTACULO);
        for(int i=0;i<8;i++)
          grFinal.setValor((56),(i),GridFinal.OBSTACULO);
        for(int i=44;i<49;i++) 
            grFinal.setValor((i),(7),GridFinal.OBSTACULO);
        for(int i=51;i<57;i++) 
            grFinal.setValor((i),(7),GridFinal.OBSTACULO);
        
        //puente y rio
        
        for(int i=0;i<10;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=0;i<8;i++) 
            grFinal.setValor((i),(52),GridFinal.OBSTACULO);
        for(int i=12;i<25;i++) 
            grFinal.setValor((i),(52),GridFinal.OBSTACULO);
        for(int i=47;i<52;i++) 
            grFinal.setValor((24),(i),GridFinal.OBSTACULO);
        for(int i=25;i<41;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=48;i<53;i++) 
            grFinal.setValor((40),(i),GridFinal.OBSTACULO);
        for(int i=41;i<61;i++) 
            grFinal.setValor((i),(52),GridFinal.OBSTACULO);
        for(int i=53;i<58;i++) 
            grFinal.setValor((60),(i),GridFinal.OBSTACULO);
        for(int i=61;i<85;i++) 
            grFinal.setValor((i),(57),GridFinal.OBSTACULO);
        for(int i=52;i<58;i++) 
            grFinal.setValor((84),(i),GridFinal.OBSTACULO);
        for(int i=90;i<100;i++) 
            grFinal.setValor((i),(52),GridFinal.OBSTACULO);
        for(int i=47;i<50;i++) 
            grFinal.setValor((92),(i),GridFinal.OBSTACULO);
        for(int i=93;i<100;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=80;i<88;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=45;i<65;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=14;i<21;i++) 
            grFinal.setValor((i),(47),GridFinal.OBSTACULO);
        for(int i=20;i<45;i++) 
            grFinal.setValor((i),(42),GridFinal.OBSTACULO);
        for(int i=64;i<81;i++) 
            grFinal.setValor((i),(52),GridFinal.OBSTACULO);
        for(int i=48;i<52;i++){

            grFinal.setValor((80),(i),GridFinal.OBSTACULO);
            grFinal.setValor((64),(i),GridFinal.OBSTACULO);
        }
        for(int i=43;i<48;i++){

            grFinal.setValor((20),(i),GridFinal.OBSTACULO);
            grFinal.setValor((44),(i),GridFinal.OBSTACULO);
        }
        grFinal.setValor((85),(52),GridFinal.OBSTACULO);
        grFinal.setValor((90),(51),GridFinal.OBSTACULO);
        grFinal.setValor((90),(50),GridFinal.OBSTACULO);
        grFinal.setValor((91),(50),GridFinal.OBSTACULO);
        grFinal.setValor((92),(50),GridFinal.OBSTACULO);
        grFinal.setValor((85),(49),GridFinal.OBSTACULO);
        grFinal.setValor((85),(50),GridFinal.OBSTACULO);
        grFinal.setValor((85),(51),GridFinal.OBSTACULO);
        grFinal.setValor((86),(49),GridFinal.OBSTACULO);
        grFinal.setValor((87),(49),GridFinal.OBSTACULO);
        grFinal.setValor((87),(48),GridFinal.OBSTACULO);
        grFinal.setValor((87),(47),GridFinal.OBSTACULO);

        grFinal.setValor((7),(51),GridFinal.OBSTACULO);
        grFinal.setValor((7),(50),GridFinal.OBSTACULO);
        grFinal.setValor((7),(49),GridFinal.OBSTACULO);

        grFinal.setValor((8),(49),GridFinal.OBSTACULO);
        grFinal.setValor((9),(49),GridFinal.OBSTACULO);

        grFinal.setValor((9),(47),GridFinal.OBSTACULO);
        grFinal.setValor((9),(48),GridFinal.OBSTACULO);

        grFinal.setValor((14),(50),GridFinal.OBSTACULO);
        grFinal.setValor((14),(49),GridFinal.OBSTACULO);
        grFinal.setValor((14),(48),GridFinal.OBSTACULO);
        grFinal.setValor((14),(47),GridFinal.OBSTACULO);

        grFinal.setValor((12),(50),GridFinal.OBSTACULO);
        grFinal.setValor((13),(50),GridFinal.OBSTACULO);

        grFinal.setValor((12),(51),GridFinal.OBSTACULO);
        grFinal.setValor((12),(52),GridFinal.OBSTACULO);


        // Torre vigia

        grFinal.setValor((13),(16),GridFinal.VIGIA);
        grFinal.setValor((14),(16),GridFinal.VIGIA);
        grFinal.setValor((13),(17),GridFinal.VIGIA);
        grFinal.setValor((14),(17),GridFinal.VIGIA);

        // Armeria

        grFinal.setValor((64),(40),GridFinal.ARMERIA);
        grFinal.setValor((64),(41),GridFinal.ARMERIA);
        grFinal.setValor((65),(40),GridFinal.ARMERIA);
        grFinal.setValor((65),(41),GridFinal.ARMERIA);

        // Santuario
        for (int i = 87; i < 90; i++)
        {
            for (int j = 75; j < 78; j++)
            {
                grFinal.setValor((i),(j),GridFinal.SANTUARIO);
            }
        }

        // Escuderia
        grFinal.setValor((24),(55),GridFinal.ESCUDERIA);
        grFinal.setValor((25),(55),GridFinal.ESCUDERIA);
        grFinal.setValor((24),(56),GridFinal.ESCUDERIA);
        grFinal.setValor((25),(56),GridFinal.ESCUDERIA);
        // Laberinto Azul y Rojo
        GameObject[] obstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
        grFinal.setObstaculos(obstaculos);
        
    }
    private void setTipos(){

        equipoAzul[INDEXEXPLORADOR].setTipo(AgentNPC.EXPLORADOR);
        equipoAzul[INDEXEXPLORADOR].name = "Explorador Azul";
        equipoAzul[INDEXEXPLORADOR].MaxSpeed = 20;
        equipoAzul[INDEXEXPLORADOR].MaxAngularAcc = 90;
        equipoAzul[INDEXEXPLORADOR].MaxAcceleration = 20;
        //equipoAzul[INDEXEXPLORADOR].setColor();
        
        cExplorador = new Explorador(INDEXEXPLORADOR,vidaExploradorAzul,equipoAzul[INDEXEXPLORADOR],spawnAzul[INDEXEXPLORADOR],Explorador.AZUL);

        equipoAzul[INDEXARCHER1].setTipo(AgentNPC.ARQUERO);
        cArquero[0] = new Archer(INDEXARCHER1,vidaArquero1Azul,equipoAzul[INDEXARCHER1],spawnAzul[INDEXARCHER1],Archer.AZUL);
        equipoAzul[INDEXARCHER1].name = "Arquero Azul 1";
        equipoAzul[INDEXARCHER1].MaxSpeed = 16;
        equipoAzul[INDEXARCHER1].MaxAngularAcc = 120;
        equipoAzul[INDEXARCHER1].MaxAcceleration = 16;

        equipoAzul[INDEXARCHER2].setTipo(AgentNPC.ARQUERO);
        cArquero[1] = new Archer(INDEXARCHER2,vidaArquero2Azul,equipoAzul[INDEXARCHER2],spawnAzul[INDEXARCHER2],Archer.AZUL);
        equipoAzul[INDEXARCHER2].name = "Arquero Azul 2";
        equipoAzul[INDEXARCHER2].MaxSpeed = 16;
        equipoAzul[INDEXARCHER2].MaxAngularAcc = 120;
        equipoAzul[INDEXARCHER2].MaxAcceleration = 16;

        equipoAzul[INDEXPESADA1].setTipo(AgentNPC.PESADA);
        cPesada[0] = new UnidadPesada(INDEXPESADA1,vidaUnidadPesada1Azul,equipoAzul[INDEXPESADA1],spawnAzul[INDEXPESADA1],UnidadPesada.AZUL);
        equipoAzul[INDEXPESADA1].name = "Unidad Pesada Azul 1";

        equipoAzul[INDEXPESADA2].setTipo(AgentNPC.PESADA);
        cPesada[1] = new UnidadPesada(INDEXPESADA2,vidaUnidadPesada2Azul,equipoAzul[INDEXPESADA2],spawnAzul[INDEXPESADA2],UnidadPesada.AZUL);
        equipoAzul[INDEXPESADA2].name = "Unidad Pesada Azul 2";

        equipoAzul[INDEXVIGILANTE].setTipo(AgentNPC.PATRULLA);
        cPatrulla = new Patrulla(INDEXVIGILANTE,vidaPatrullaAzul,equipoAzul[INDEXVIGILANTE],spawnAzul[INDEXVIGILANTE],Patrulla.AZUL);
        equipoAzul[INDEXVIGILANTE].name = "Patrulla Azul";

        equipoRojo[INDEXEXPLORADOR].setTipo(AgentNPC.EXPLORADOR);
        equipoRojo[INDEXEXPLORADOR].name = "Explorador Rojo";
        equipoRojo[INDEXEXPLORADOR].MaxSpeed = 20;
        equipoRojo[INDEXEXPLORADOR].MaxAngularAcc = 90;
        equipoRojo[INDEXEXPLORADOR].MaxAcceleration = 20;

        rExplorador = new Explorador(INDEXEXPLORADOR,vidaExploradorRojo,equipoRojo[INDEXEXPLORADOR],spawnRojo[INDEXEXPLORADOR],Explorador.ROJO);

        equipoRojo[INDEXARCHER1].setTipo(AgentNPC.ARQUERO);
        rArquero[0] = new Archer(INDEXARCHER1,vidaArquero1Rojo,equipoRojo[INDEXARCHER1],spawnRojo[INDEXARCHER1],Archer.ROJO);
        equipoRojo[INDEXARCHER1].name = "Arquero Rojo 1";
        equipoRojo[INDEXARCHER1].MaxSpeed = 16;
        equipoRojo[INDEXARCHER1].MaxAngularAcc = 120;
        equipoRojo[INDEXARCHER1].MaxAcceleration = 16;

        equipoRojo[INDEXARCHER2].setTipo(AgentNPC.ARQUERO);
        rArquero[1] = new Archer(INDEXARCHER2,vidaArquero2Rojo,equipoRojo[INDEXARCHER2],spawnRojo[INDEXARCHER2],Archer.ROJO);
        equipoRojo[INDEXARCHER2].name = "Arquero Rojo 2";
        equipoRojo[INDEXARCHER2].MaxSpeed = 16;
        equipoRojo[INDEXARCHER2].MaxAngularAcc = 120;
        equipoRojo[INDEXARCHER2].MaxAcceleration = 16;

        equipoRojo[INDEXPESADA1].setTipo(AgentNPC.PESADA);
        rPesada[0] = new UnidadPesada(INDEXPESADA1,vidaUnidadPesada1Rojo,equipoRojo[INDEXPESADA1],spawnRojo[INDEXPESADA1],UnidadPesada.ROJO);
        equipoRojo[INDEXPESADA1].name = "Unidad Pesada Rojo 1";

        equipoRojo[INDEXPESADA2].setTipo(AgentNPC.PESADA);
        rPesada[1] = new UnidadPesada(INDEXPESADA2,vidaUnidadPesada2Rojo,equipoRojo[INDEXPESADA2],spawnRojo[INDEXPESADA2],UnidadPesada.ROJO);
        equipoRojo[INDEXPESADA2].name = "Unidad Pesada Rojo 2";

        equipoRojo[INDEXVIGILANTE].setTipo(AgentNPC.PATRULLA);
        rPatrulla = new Patrulla(INDEXVIGILANTE,vidaPatrullaRojo,equipoRojo[INDEXVIGILANTE],spawnRojo[INDEXVIGILANTE],Patrulla.ROJO);
        equipoRojo[INDEXVIGILANTE].name = "Patrulla Rojo";

    }
    public void setNPCs(){

        Azules[INDEXEXPLORADOR] = GameObject.Find("Explorador Azul");
        cExplorador.setObjetoNPC(Azules[INDEXEXPLORADOR]);
        Azules[INDEXARCHER1] = GameObject.Find("Arquero Azul 1");
        cArquero[0].setObjetoNPC(Azules[INDEXARCHER1]);
        Azules[INDEXARCHER2] = GameObject.Find("Arquero Azul 2");
        cArquero[1].setObjetoNPC(Azules[INDEXARCHER2]);
        Azules[INDEXPESADA1] = GameObject.Find("Unidad Pesada Azul 1");
        cPesada[0].setObjetoNPC(Azules[INDEXPESADA1]);
        Azules[INDEXPESADA2] = GameObject.Find("Unidad Pesada Azul 2");
        cPesada[1].setObjetoNPC(Azules[INDEXPESADA2]);
        Azules[INDEXVIGILANTE] = GameObject.Find("Patrulla Azul");
        cPatrulla.setObjetoNPC(Azules[INDEXVIGILANTE]);

        Rojos[INDEXEXPLORADOR] = GameObject.Find("Explorador Rojo");
        rExplorador.setObjetoNPC(Rojos[INDEXEXPLORADOR]);
        Rojos[INDEXARCHER1] = GameObject.Find("Arquero Rojo 1");
        rArquero[0].setObjetoNPC(Rojos[INDEXARCHER1]);
        Rojos[INDEXARCHER2] = GameObject.Find("Arquero Rojo 2");
        rArquero[1].setObjetoNPC(Rojos[INDEXARCHER2]);
        Rojos[INDEXPESADA1] = GameObject.Find("Unidad Pesada Rojo 1");
        rPesada[0].setObjetoNPC(Rojos[INDEXPESADA1]);
        Rojos[INDEXPESADA2] = GameObject.Find("Unidad Pesada Rojo 2");
        rPesada[1].setObjetoNPC(Rojos[INDEXPESADA2]);
        Rojos[INDEXVIGILANTE] = GameObject.Find("Patrulla Rojo");
        rPatrulla.setObjetoNPC(Rojos[INDEXVIGILANTE]);
    }
    public void setBarrasDeVida(){

        for (int i = 0; i < 5; i++)
        {
            vidaExploradorAzul[i] = GameObject.Find("Explorador Azul/Barra de Vida/Vida"+(i+1));
        }
        vidaExploradorAzul[4].SetActive(false);
        
        for (int i = 0; i < 5; i++)
        {
            vidaArquero1Azul[i] = GameObject.Find("Arquero Azul 1/Barra de Vida/Vida"+(i+1));
        }
        vidaArquero1Azul[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaArquero2Azul[i] = GameObject.Find("Arquero Azul 2/Barra de Vida/Vida"+(i+1));
        }
        vidaArquero2Azul[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaUnidadPesada1Azul[i] = GameObject.Find("Unidad Pesada Azul 1/Barra de Vida/Vida"+(i+1));
        }
        vidaUnidadPesada1Azul[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaUnidadPesada2Azul[i] = GameObject.Find("Unidad Pesada Azul 2/Barra de Vida/Vida"+(i+1));
        }
        vidaUnidadPesada2Azul[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaPatrullaAzul[i] = GameObject.Find("Patrulla Azul/Barra de Vida/Vida"+(i+1));
        }
        vidaPatrullaAzul[4].SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            vidaExploradorRojo[i] = GameObject.Find("Explorador Rojo/Barra de Vida/Vida"+(i+1));
        }
        vidaExploradorRojo[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaArquero1Rojo[i] = GameObject.Find("Arquero Rojo 1/Barra de Vida/Vida"+(i+1));
        }
        vidaArquero1Rojo[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaArquero2Rojo[i] = GameObject.Find("Arquero Rojo 2/Barra de Vida/Vida"+(i+1));
        }
        vidaArquero2Rojo[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaUnidadPesada1Rojo[i] = GameObject.Find("Unidad Pesada Rojo 1/Barra de Vida/Vida"+(i+1));
        }
        vidaUnidadPesada1Rojo[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaUnidadPesada2Rojo[i] = GameObject.Find("Unidad Pesada Rojo 2/Barra de Vida/Vida"+(i+1));
        }
        vidaUnidadPesada2Rojo[4].SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            vidaPatrullaRojo[i] = GameObject.Find("Patrulla Rojo/Barra de Vida/Vida"+(i+1));
        }
        vidaPatrullaRojo[4].SetActive(false);
    }
    private void creaTexto(){

        int cont = 0;
        grid = new TextMesh[rows, cols];
        
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                GameObject obj = new GameObject("Tile_" + i + "_" + j);
                obj.transform.position = new Vector3((i * cellSize)+ cellSize/2, 0, (j * cellSize) + cellSize/2);
                obj.transform.parent = transform;

                TextMesh textMesh = obj.AddComponent<TextMesh>();
                textMesh.text = "" + grFinal.getValor(i,j) + "  " + i + "," + j;
                textMesh.text = i + "," + j;
                textMesh.fontSize = 100;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.color = Color.black;
                textMesh.transform.rotation = Quaternion.Euler(90f, 0f, 0f);


                grid[i, j] = textMesh;
                cont++;
            }
        }
    }

    private void crearWayPoints(){
        
        for(int i = 0; i< puntosAzul.Length;  i++){
            GameObject wp = Instantiate(prefabWayPoint);
            wp.transform.position = new Vector3((puntosAzul[i].getX()*cellSize) + cellSize/2, 0, (puntosAzul[i].getY()*cellSize) + cellSize/2);
            listaWayPoints.Add(wp);
        }

        for(int i = 0; i < puntosAzul2.Length; i++){
            GameObject wp = Instantiate(prefabWayPoint);
            wp.transform.position = new Vector3((puntosAzul2[i].getX()*cellSize) + cellSize/2, 0, (puntosAzul2[i].getY()*cellSize) + cellSize/2);
            listaWayPoints.Add(wp);
        }

        for(int i = 0; i < puntosRojo.Length; i++){
            GameObject wp = Instantiate(prefabWayPoint);
            wp.transform.position = new Vector3((puntosRojo[i].getX()*cellSize) + cellSize/2, 0, (puntosRojo[i].getY()*cellSize) + cellSize/2);
            listaWayPoints.Add(wp);
        }

        for(int i = 0; i < puntosRojo2.Length; i++){
            GameObject wp = Instantiate(prefabWayPoint);
            wp.transform.position = new Vector3((puntosRojo2[i].getX()*cellSize) + cellSize/2, 0, (puntosRojo2[i].getY()*cellSize) + cellSize/2);
            listaWayPoints.Add(wp);
        }
            
    }

    private void eliminarWayPoints(){
        
        foreach (GameObject wp in listaWayPoints)
        {
            Destroy(wp);
        }
            
    }

    void FixedUpdate(){

        enemigosTeamBlue.resetArea();
        for (int i = 1; i < equipoRojo.Length; i++)
        {
            int x;
            int y;
            grFinal.getCoordenadas(equipoRojo[i].Position,out x,out y);
            enemigosTeamBlue.setPeligro(x,y);
        }
        enemigosTeamRed.resetArea();
        for (int i = 1; i < equipoAzul.Length; i++)
        {
            int x;
            int y;
            grFinal.getCoordenadas(equipoAzul[i].Position,out x,out y);
            enemigosTeamRed.setPeligro(x,y);
        }
        
    }
    void Update(){

        if (Input.GetKeyDown(KeyCode.H)){
            modoDebug = !modoDebug;
            if(modoDebug){
                crearWayPoints();
            } else{
                eliminarWayPoints();
            }
                
            
        }

        moverNPC();
        if (!verificando)
        {
            Invoke("verificacion",1);
            verificando = true;
        }
        
        if (Input.GetMouseButtonDown(1))
        {   
            if (selectAgent != null){

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject obj = hit.collider.gameObject;
                    int iObjetivo;
                    int jObjetivo;
                    grFinal.getCoordenadas(obj.transform.position,out iObjetivo,out jObjetivo);

                    int indice = System.Array.IndexOf(equipoAzul, selectAgent);

                    if (indice != -1)
                    {
                        buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
                        buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
                        caminosAzul[indice] = buscadoresAzul[indice].A();

                        selectAgent.setLLegada(false);
                        selectAgent.quitarMarcador();
                    }
                    
                }
            }
           
        }
        if (Input.GetMouseButtonDown(0))
        {
            seleccionarNPC();
        }

    }

    void OnDrawGizmos()
    {            
        if (modoDebug){

            AgentNPC[] agentes = FindObjectsOfType<AgentNPC>();

            foreach (AgentNPC agente in agentes)
            {

                Vector3 from = agente.Position; // Origen de la línea
                Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

                from = from + elevation;

                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(from, agente.Velocity);

                float distanciaBigotesExteriores = agente.AnguloExterior/agente.getNumBigotes();
                float distanciaBigotesInteriores = agente.AnguloInterior/agente.getNumBigotes(); 
                
                for (int i=0;i<agente.getNumBigotes();i++){

                    // Mirando en la dirección de la orientación.
                    Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;

                    Gizmos.color = Color.red;
                    Vector3 vectorInterior1 = Bodi.VectorRotate(direction, agente.AnguloInterior-distanciaBigotesInteriores*i);
                    Vector3 vectorInterior2 = Bodi.VectorRotate(direction, -agente.AnguloInterior+distanciaBigotesInteriores*i);  
                    
                    Gizmos.DrawRay(from, vectorInterior1);
                    Gizmos.DrawRay(from, vectorInterior2);

                    // Dibujamos el angulo exterior
                    Vector3 vectorExterior3 = Bodi.VectorRotate(direction, agente.AnguloExterior-distanciaBigotesExteriores*i);
                    Vector3 vectorExterior4 = Bodi.VectorRotate(direction, -agente.AnguloExterior+distanciaBigotesExteriores*i); 
                    
                    Gizmos.color = Color.blue; 
                    Gizmos.DrawRay(from, vectorExterior3);
                    Gizmos.DrawRay(from, vectorExterior4);

                }

                // Dibujamos el circulo interior
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(agente.Position, agente.RadioInterior);

                // Dibujamos el circulo exterior
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(agente.Position, agente.RadioExterior);

            }
        }

    }

    private void verificacion(){

        verificaTorreVigia();
        verificaArmeria();
        verificaPuenteAzulDerecho();
        verificaPuenteAzulIzquierdo();
        verificaPuenteRojoDerecho();
        verificaPuenteRojoIzquierdo();
        verificaSantuario();
        verificaEscuderia();
        actualizaObjetivos();
        verificando = false;
    }
    private void actualizaObjetivos(){

        if (!todosInBlue && objetivosTeamBlue.Count == 4)
        {
            if (objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getPropiedad() == Objetivo.AZUL 
                || objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getPropiedad() == Objetivo.AZUL)
            {
                todosInBlue = true;
                for (int i = INDEX_SANTUARIO; i < numObjetives; i++)
                {
                    objExplorerBlue.Add(objetivosMundo[i]);
                }
            }
        }
        if (!todosInRed && objetivosTeamRed.Count == 4)
        {
            if (objetivosMundo[INDEX_PUENTE_DERECHO_ROJO].getPropiedad() == Objetivo.ROJO 
                || objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO].getPropiedad() == Objetivo.ROJO)
            {
                todosInRed = true;
                for (int i = INDEX_TORRE_VIGIA; i < INDEX_SANTUARIO; i++)
                {
                    objExplorerRed.Add(objetivosMundo[i]);
                }
            }
        }
    }
    private void seleccionarNPC(){

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject npcObject = hit.collider.gameObject;
            if (npcObject.CompareTag("NPC"))
            {
                selectAgent = npcObject.GetComponent<AgentNPC>();
                foreach(AgentNPC pl in equipoAzul)
                {
                    if(!pl.Equals(selectAgent))
                    {
                        pl.quitarMarcador();
                        //debugNombre.text = "Unidad: ";
                        
                    }else{
                        pl.activarMarcador();
                        debugNombre.text = "Unidad: " + selectAgent.name;
                        int indice = System.Array.IndexOf(equipoAzul, selectAgent);
                        if(indice == INDEXEXPLORADOR){
                            debugComportamiento.text = "Comportamiento: " + cExplorador.getComportamientoString();
                            debugVida.text = "Vida: " + cExplorador.getVida();
                            //debugOjetivo.text = "Objetivo " + cExplorador.getDecision();
                        } 
                        else if(indice == INDEXARCHER1){
                            debugComportamiento.text = "Comportamiento: " + cArquero[0].getComportamientoString();
                            debugVida.text = "Vida: " + cArquero[0].getVida();
                        }
                        else if(indice == INDEXARCHER2){
                            debugComportamiento.text = "Comportamiento: " + cArquero[1].getComportamientoString();
                            debugVida.text = "Vida: " + cArquero[1].getVida();
                        }

                        else if(indice == INDEXPESADA1){
                            debugComportamiento.text = "Comportamiento: " + cPesada[0].getComportamientoString();
                            debugVida.text = "Vida: " + cPesada[0].getVida();
                        } 
                        else if(indice == INDEXPESADA2){
                            debugComportamiento.text = "Comportamiento: " + cPesada[1].getComportamientoString();
                            debugVida.text = "Vida: " + cPesada[1].getVida();
                        }

                        else {
                            debugComportamiento.text = "Comportamiento: " + cPatrulla.getComportamientoString();
                            debugVida.text = "Vida: " + cPatrulla.getVida();
                        }
                        
                    }
                }
            }
            else
            {
                if (selectAgent != null){
                    selectAgent.quitarMarcador();
                }
            }
        }
    }

    private void moverNPC(){
        
        for(int i=0;i<numNPC;i++)
        {
            
            if(equipoAzul[i].getTipo() == AgentNPC.ARQUERO)
            {
                if (i == INDEXARCHER1)
                {
                    
                    if (cArquero[0].getMuerto() && cArquero[0].getEstado() == Archer.MUERTO)
                    {
                        comprobarArqueros(0,"Azul");

                    }else if(!cArquero[0].getMuerto()){

                       movArcher(equipoAzul[i],0);
                    }
                    if (rArquero[0].getMuerto() && rArquero[0].getEstado() == Archer.MUERTO)
                    {
                        comprobarArqueros(0,"Rojo");

                    }else if(!rArquero[0].getMuerto()){

                        movArcher2(equipoRojo[i],0);
                    }
                    
                }else{

                    if (cArquero[1].getMuerto() && cArquero[1].getEstado() == Archer.MUERTO)
                    {
                        comprobarArqueros(1,"Azul");
                        
                    }else if(!cArquero[1].getMuerto()){

                        movArcher(equipoAzul[i],1);
                    }
                    if (rArquero[1].getMuerto() && rArquero[1].getEstado() == Archer.MUERTO)
                    {
                        comprobarArqueros(1,"Rojo");

                    }else if(!rArquero[1].getMuerto()){

                        movArcher2(equipoRojo[i],1);
                    }
                }
                
            }else if (equipoAzul[i].getTipo() == AgentNPC.PESADA)
            {
                if (i == INDEXPESADA1)
                {
                    if (cPesada[0].getMuerto() && cPesada[0].getEstado() == UnidadPesada.MUERTO)
                    {
                        comprobarUnidadesPesadas(0,"Azul");

                    }else if(!cPesada[0].getMuerto()){

                        movPesada(equipoAzul[i],0);
                    }
                    if (rPesada[0].getMuerto() && rPesada[0].getEstado() == Archer.MUERTO)
                    {
                        comprobarUnidadesPesadas(0,"Rojo");

                    }else if(!rPesada[0].getMuerto()){

                        movPesada2(equipoRojo[i],0);
                    }
                }else{

                    if (cPesada[1].getMuerto() && cPesada[1].getEstado() == UnidadPesada.MUERTO)
                    {
                        comprobarUnidadesPesadas(1,"Azul");

                    }else if(!cPesada[1].getMuerto()){

                        movPesada(equipoAzul[i],1);
                    }
                    if (rPesada[1].getMuerto() && rPesada[1].getEstado() == Archer.MUERTO)
                    {
                        comprobarUnidadesPesadas(1,"Rojo");

                    }else if(!rPesada[1].getMuerto()){

                        movPesada2(equipoRojo[i],1);
                    }
                }
            }
            else if (equipoAzul[i].getTipo() == AgentNPC.EXPLORADOR)
            {
                if (cExplorador.getMuerto() && cExplorador.getEstado() == Explorador.MUERTO)
                {
                    comprobarExploradores("Azul");

                }else if(!cExplorador.getMuerto()){

                    movExplorer(equipoAzul[i],objExplorerBlue);
                }
                if (rExplorador.getMuerto() && rExplorador.getEstado() == Explorador.MUERTO)
                {
                    comprobarExploradores("Rojo");
                }else if(!rExplorador.getMuerto()){

                    movExplorer2(equipoRojo[i],objExplorerRed);
                }
            }
            else if (equipoAzul[i].getTipo() == AgentNPC.PATRULLA)
            {
                if (cPatrulla.getMuerto() && cPatrulla.getEstado() == Patrulla.MUERTO)
                {
                    comprobarPatrullas("Azul");

                }else if(!cPatrulla.getMuerto()){

                    movPatrulla(equipoAzul[i]);
                }
                if (rPatrulla.getMuerto() && rPatrulla.getEstado() == Patrulla.MUERTO)
                {
                    comprobarPatrullas("Rojo");
                }else if(!rPatrulla.getMuerto()){

                    movPatrulla2(equipoRojo[i]);
                }
            }
        }
    }
    private void movArcher(AgentNPC pl, int index){
        
        int xDespues;
        int yDespues;
        int indice = cArquero[index].getIndexNPC();

        if(pl.getLLegada() && !(cArquero[index].getComportamiento() == Archer.ATACAR || cArquero[index].getComportamiento() == Archer.RELOAD)){
            
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            cArquero[index].setLimites(i,j);
            npcVirtualAzul[indice].Position = cArquero[index].getDecision(rutaAzulPesada,grFinal,objetivosTeamBlue,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);

            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosAzul[indice] = buscadoresAzul[indice].A();

            pl.setLLegada(false);
            
        }else if(pl.getLLegada() && cArquero[index].getComportamiento() == Archer.ATACAR){
            
            //funcion de ataque
            Debug.Log("aqui");
            cArquero[index].setComportamiento(Archer.RELOAD);
            if (index == 0)
            {
                Invoke("ataqueArcher1Azul",cArquero[index].getAtackSpeed());
            }else{

                Invoke("ataqueArcher2Azul",cArquero[index].getAtackSpeed());
            }
            
        }else if(!pl.getLLegada()){

            buscadoresAzul[indice].comprobarCamino(caminosAzul[indice]);
                
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);
        
        //Debug.Log("Coorde: "+xDespues+","+yDespues+"  Valor: "+grFinal.getValor(xDespues,yDespues));       
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            cArquero[index].setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidadesAzul.setUnidad(xDespues,yDespues,ArrayUnidades.ARQUEROAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul;
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7; 
        }  
    }
    private void movArcher2(AgentNPC pl, int index){
        
        int xDespues;
        int yDespues;
        int indice = rArquero[index].getIndexNPC();

        if(pl.getLLegada() && !(rArquero[index].getComportamiento() == Archer.ATACAR || rArquero[index].getComportamiento() == Archer.RELOAD)){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            rArquero[index].setLimites(i,j);
            npcVirtualRojo[indice].Position = rArquero[index].getDecision(rutaRojaPesada, grFinal,objetivosTeamRed,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualRojo[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresRojo[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualRojo[indice]);
            buscadoresRojo[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosRojo[indice] = buscadoresRojo[indice].A();

            pl.setLLegada(false);
            
        }else if(pl.getLLegada() && rArquero[index].getComportamiento() == Archer.ATACAR){
            
            //funcion de ataque
            rArquero[index].setComportamiento(Archer.RELOAD);
            if (index == 0)
            {
                Invoke("ataqueArcher1Rojo",rArquero[index].getAtackSpeed());
            }else{

                Invoke("ataqueArcher2Rojo",rArquero[index].getAtackSpeed());
            }
            
        
        }else if(!pl.getLLegada()){
                            
            buscadoresRojo[indice].comprobarCamino(caminosRojo[indice]);
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);
        
                
        if(teamRed[indice].getI() != xDespues || teamRed[indice].getJ() != yDespues){
            
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            rArquero[index].setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCROJO);
            unidadesRojo.setUnidad(xDespues,yDespues,ArrayUnidades.ARQUEROROJO);
            teamRed[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoRojo;  
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7;
        }  
    }
    private void movPesada(AgentNPC pl, int index){

        int xDespues;
        int yDespues;
        int indice = cPesada[index].getIndexNPC();
        
        if(pl.getLLegada() && !(cPesada[index].getComportamiento() == UnidadPesada.ATACAR || cPesada[index].getComportamiento() == UnidadPesada.RELOAD)){

                
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
                    
            cPesada[index].setLimites(i,j);
            npcVirtualAzul[indice].Position = cPesada[index].getDecision(rutaAzulPesada,grFinal,objetivosTeamBlue,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosAzul[indice] = buscadoresAzul[indice].A();

            pl.setLLegada(false);

            
        }else if(!pl.getLLegada()){
                            
            buscadoresAzul[indice].comprobarCamino(caminosAzul[indice]);
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);
    
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            cPesada[index].setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidadesAzul.setUnidad(xDespues,yDespues,ArrayUnidades.UNIDADPESADAAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul; 
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7;
        } 
    }
    private void movPesada2(AgentNPC pl, int index){

        int xDespues;
        int yDespues;
        int indice = rPesada[index].getIndexNPC();
        
        if(pl.getLLegada() && !(rPesada[index].getComportamiento() == UnidadPesada.ATACAR || rPesada[index].getComportamiento() == UnidadPesada.RELOAD)){
             
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
                    
            rPesada[index].setLimites(i,j);
            npcVirtualRojo[indice].Position = rPesada[index].getDecision(rutaRojaPesada,grFinal,objetivosTeamRed,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualRojo[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresRojo[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualRojo[indice]);
            buscadoresRojo[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosRojo[indice] = buscadoresRojo[indice].A();

            pl.setLLegada(false);
            
        }else if(pl.getLLegada() && rPesada[index].getComportamiento() == UnidadPesada.ATACAR){
            
            //funcion de ataque
            rPesada[index].setComportamiento(UnidadPesada.RELOAD);
            if (index == 0)
            {
                Invoke("ataquePesada1Rojo",rPesada[index].getAtackSpeed());
            }else{

                Invoke("ataquePesada2Rojo",rPesada[index].getAtackSpeed());
            }

        }else if(!pl.getLLegada()){
                            
            buscadoresRojo[indice].comprobarCamino(caminosRojo[indice]);
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);

        if(teamRed[indice].getI() != xDespues || teamRed[indice].getJ() != yDespues){
            
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            rPesada[index].setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCROJO);
            unidadesRojo.setUnidad(xDespues,yDespues,ArrayUnidades.UNIDADPESADAROJO);
            teamRed[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoRojo; 
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7;
        }  
    }
    private void verificaTorreVigia(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_TORRE_VIGIA].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_TORRE_VIGIA].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_TORRE_VIGIA]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_TORRE_VIGIA]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in torreVigia)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_TORRE_VIGIA].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in torreVigia)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_TORRE_VIGIA].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_TORRE_VIGIA,WayPoint.TORRE_VIGIA);
        }
    }
    private void verificaPuenteAzulIzquierdo(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteIzquierdoAzul)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteIzquierdoAzul)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].setPropiedad(Objetivo.ROJO);
            }
        }else {

            compruebaProipedadObjetivo(INDEX_PUENTE_IZQUIERDO_AZUL,WayPoint.PUENTE_IZQUIERDO_AZUL);
        }  
        
    }
    private void verificaPuenteAzulDerecho(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteDerechoAzul)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteDerechoAzul)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].setPropiedad(Objetivo.ROJO);
            }
        }else {

            compruebaProipedadObjetivo(INDEX_PUENTE_DERECHO_AZUL,WayPoint.PUENTE_DERECHO_AZUL);
        }
        
    }
    private void verificaPuenteRojoDerecho(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_DERECHO_ROJO].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_DERECHO_ROJO].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_ROJO]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_DERECHO_ROJO]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_ROJO]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_DERECHO_ROJO]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteDerechoRojo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_DERECHO_ROJO].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteDerechoRojo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_DERECHO_ROJO].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_PUENTE_DERECHO_ROJO,WayPoint.PUENTE_DERECHO_ROJO);
        }
        
    }
    private void verificaPuenteRojoIzquierdo(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteIzquierdoRojo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteIzquierdoRojo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_ROJO].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_PUENTE_IZQUIERDO_ROJO,WayPoint.PUENTE_IZQUIERDO_ROJO);
        }
        
    }
    private void compruebaProipedadObjetivo(int index,string objetivo){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[index].getPropiedad() == Objetivo.AZUL)
        {
            foreach (Coordenada coor in objetivosMundo[index].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                    && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                        contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_TORRE_VIGIA]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_TORRE_VIGIA]);
                }
            }
            if (contAzul == 0 && contRojo >= 2)
            {
                objetivosMundo[index].setPropiedad(Objetivo.ROJO);
                rutaAzul.setDisponible(objetivo,false);
                rutaAzulPesada.setDisponible(objetivo,false);
                rutaRoja.setDisponible(objetivo,true);
                rutaRojaPesada.setDisponible(objetivo,true);

            }else if (!rutaAzul.getDisponible(objetivo))
            {
                rutaAzul.setDisponible(objetivo,true);
                rutaAzulPesada.setDisponible(objetivo,true);
                rutaRoja.setDisponible(objetivo,false);
                rutaRojaPesada.setDisponible(objetivo,false);
            }
        }else if (objetivosMundo[index].getPropiedad() == Objetivo.ROJO)
        {
            foreach (Coordenada coor in objetivosMundo[index].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                    && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_TORRE_VIGIA]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_TORRE_VIGIA]);
                }
            }
            if (contRojo == 0 && contAzul >= 2)
            {
                objetivosMundo[index].setPropiedad(Objetivo.ROJO);
                rutaAzul.setDisponible(objetivo,true);
                rutaAzulPesada.setDisponible(objetivo,true);
                rutaRoja.setDisponible(objetivo,false);
                rutaRojaPesada.setDisponible(objetivo,false);

            }else if (!rutaRoja.getDisponible(objetivo))
            {
                rutaAzul.setDisponible(objetivo,false);
                rutaAzulPesada.setDisponible(objetivo,false);
                rutaRoja.setDisponible(objetivo,true);
                rutaRojaPesada.setDisponible(objetivo,true);
            }
        } 
    }
    private void verificaArmeria(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_ARMERIA].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_ARMERIA].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_ARMERIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_ARMERIA]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_ARMERIA]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_ARMERIA]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in armeria)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;
                }
                objetivosMundo[INDEX_ARMERIA].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in armeria)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_ARMERIA].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_ARMERIA,WayPoint.ARMERIA);
        }
    }

    private void verificaSantuario(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_SANTUARIO].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_SANTUARIO].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_SANTUARIO]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_SANTUARIO]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_SANTUARIO]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_SANTUARIO]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in santuario)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_SANTUARIO].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in santuario)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_SANTUARIO].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_SANTUARIO,WayPoint.SANTUARIO);
        }
        
    }
    private void verificaEscuderia(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_ESCUDERIA].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_ESCUDERIA].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidadesRojo.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_ESCUDERIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_ESCUDERIA]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidadesAzul.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_ESCUDERIA]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_ESCUDERIA]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in escuderia)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_ESCUDERIA].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in escuderia)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_ESCUDERIA].setPropiedad(Objetivo.ROJO);
            }
        }else{

            compruebaProipedadObjetivo(INDEX_ESCUDERIA,WayPoint.ESCUDERIA);
        }
    }
    private void movExplorer(AgentNPC pl,List<Objetivo> objs){

        int xDespues;
        int yDespues;
        int indice = cExplorador.getIndexNPC();
        if(pl.getLLegada()){
            
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            cExplorador.setLimites(i,j);
            npcVirtualAzul[indice].Position = cExplorador.getDecision(grFinal,objs,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosAzul[indice] = buscadoresAzul[indice].A();

            pl.setLLegada(false);
            
        }else if(!pl.getLLegada()){    
            buscadoresAzul[indice].comprobarCamino(caminosAzul[indice]);
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidadesAzul.setUnidad(xDespues,yDespues,ArrayUnidades.EXPLORADOAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul; 
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7; 
        }  
    }
    private void movExplorer2(AgentNPC pl,List<Objetivo> objs){

        int xDespues;
        int yDespues;
        int indice = rExplorador.getIndexNPC();
        if(pl.getLLegada()){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            rExplorador.setLimites(i,j);
            npcVirtualRojo[indice].Position = rExplorador.getDecision(grFinal,objs,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualRojo[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresRojo[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualRojo[indice]);
            buscadoresRojo[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosRojo[indice] = buscadoresRojo[indice].A();

            pl.setLLegada(false);
            
        }else if(!pl.getLLegada()){
            
            buscadoresRojo[indice].comprobarCamino(caminosRojo[indice]);
            
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamRed[indice].getI() != xDespues || teamRed[indice].getJ() != yDespues){
            
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCROJO);
            unidadesRojo.setUnidad(xDespues,yDespues,ArrayUnidades.EXPLORADORROJO);
            teamRed[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoRojo;  
            casilla.GetComponent<Renderer>().enabled = true;
            casilla.layer = 7;
        }  
    }
    private void movPatrulla(AgentNPC pl){

        int xDespues;
        int yDespues;
        int indice = cPatrulla.getIndexNPC();
        if(pl.getLLegada() && !(cPatrulla.getComportamiento() == Patrulla.ATACAR || cPatrulla.getComportamiento() == Patrulla.RELOAD)){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            cPatrulla.setLimites(i,j);
            npcVirtualAzul[indice].Position = cPatrulla.getDecision(grFinal,rutaAzul,objetivosTeamBlue,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosAzul[indice] = buscadoresAzul[indice].A();

            pl.setLLegada(false);

           }else if(pl.getLLegada() && cPatrulla.getComportamiento() == Patrulla.ATACAR){
            
            //funcion de ataque
            cPatrulla.setComportamiento(Patrulla.RELOAD);
            Invoke("ataquePatrullaAzul",cPatrulla.getAtackSpeed());
        
        }else if(!pl.getLLegada()){

            buscadoresAzul[indice].comprobarCamino(caminosAzul[indice]);
        }


        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            cPatrulla.setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidadesAzul.setUnidad(xDespues,yDespues,ArrayUnidades.PATRULLAAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            casillas_minimapa[xcasilla,ycasilla].GetComponent<Renderer>().material = materialEquipoAzul;  
            casillas_minimapa[xcasilla,ycasilla].GetComponent<Renderer>().enabled = true;
            casillas_minimapa[xcasilla,ycasilla].layer = 7;
        }  
    }

    private void movPatrulla2(AgentNPC pl){

        int xDespues;
        int yDespues;
        int indice = rPatrulla.getIndexNPC();
        if(pl.getLLegada() && !(rPatrulla.getComportamiento() == Patrulla.ATACAR || rPatrulla.getComportamiento() == Patrulla.RELOAD)){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            rPatrulla.setLimites(i,j);
            npcVirtualRojo[indice].Position = rPatrulla.getDecision(grFinal,rutaRoja,objetivosTeamRed,unidadesAzul.getArray(),unidadesRojo.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualRojo[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresRojo[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualRojo[indice]);
            buscadoresRojo[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            caminosRojo[indice] = buscadoresRojo[indice].A();

            pl.setLLegada(false);
            
        }else if(pl.getLLegada() && rPatrulla.getComportamiento() == Patrulla.ATACAR){
            
            //funcion de ataque
            rPatrulla.setComportamiento(Patrulla.RELOAD);
            Invoke("ataquePatrullaRojo",rPatrulla.getAtackSpeed());
            

        }else if(!pl.getLLegada()){
                            
            buscadoresRojo[indice].comprobarCamino(caminosRojo[indice]);
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamRed[indice].getI() != xDespues || teamRed[indice].getJ() != yDespues){
            
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();
            rPatrulla.setLimites(xDespues,yDespues);
            grFinal.setValor(xDespues,yDespues,GridFinal.NPCROJO);
            unidadesRojo.setUnidad(xDespues,yDespues,ArrayUnidades.PATRULLAROJO);
            teamRed[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            casillas_minimapa[xcasilla,ycasilla].GetComponent<Renderer>().material = materialEquipoRojo; 
            casillas_minimapa[xcasilla,ycasilla].GetComponent<Renderer>().enabled = true;
            casillas_minimapa[xcasilla,ycasilla].layer = 7; 
        }  
    }

    private void comprobarArqueros(int i,string eq){

        if (eq == "Azul")
        {
            int indice = cArquero[i].getIndexNPC();
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnArcher"+(i+1)+eq,cArquero[i].getDelay());
            cArquero[i].setNewDelay(10);
            cArquero[i].setEstado(Archer.SPAWNING);
        }else{

            int indice = rArquero[i].getIndexNPC();
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnArcher"+(i+1)+eq,rArquero[i].getDelay());
            rArquero[i].setNewDelay(10);
            rArquero[i].setEstado(Archer.SPAWNING);
        }
        
    }
    private void comprobarUnidadesPesadas(int i,string eq){

        if (eq == "Azul")
        {
            int indice = cPesada[i].getIndexNPC();
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnPesada"+(i+1)+eq,cPesada[i].getDelay());
            cPesada[i].setNewDelay(10);
            cPesada[i].setEstado(UnidadPesada.SPAWNING);
        }else{

            int indice = rPesada[i].getIndexNPC();
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnPesada"+(i+1)+eq,rPesada[i].getDelay());
            rPesada[i].setNewDelay(10);
            rPesada[i].setEstado(UnidadPesada.SPAWNING);
        }
        
    }
    private void comprobarPatrullas(string eq){

        if (eq == "Azul")
        {
            int indice = cPatrulla.getIndexNPC();
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnPatrulla"+eq,cPatrulla.getDelay());
            cPatrulla.setNewDelay(10);
            cPatrulla.setEstado(UnidadPesada.SPAWNING);
        }else{

            int indice = rPatrulla.getIndexNPC();
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnPatrulla"+eq,rPatrulla.getDelay());
            rPatrulla.setNewDelay(10);
            rPatrulla.setEstado(UnidadPesada.SPAWNING);
        }
        
    }
    private void comprobarExploradores(string eq){

        if (eq == "Azul")
        {
            int indice = cExplorador.getIndexNPC();
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidadesAzul.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnExplorador"+eq,cExplorador.getDelay());
            cExplorador.setNewDelay(10);
            cExplorador.setEstado(Explorador.SPAWNING);
        }else{

            int indice = rExplorador.getIndexNPC();
            grFinal.setValor(teamRed[indice].getI(),teamRed[indice].getJ(),GridFinal.LIBRE);
            unidadesRojo.setUnidad(teamRed[indice].getI(),teamRed[indice].getJ(),ArrayUnidades.LIBRE);
            Invoke("respawnExplorador"+eq,rExplorador.getDelay());
            rExplorador.setNewDelay(10);
            rExplorador.setEstado(Explorador.SPAWNING);
        }
        
    }

    // Respawn
    private void respawnArcher1Azul(){

        npcVirtualAzul[INDEXARCHER1].Position = equipoAzul[INDEXARCHER1].Position;
        cArquero[0].restablecerVida();

    }
    private void respawnArcher2Azul(){

        npcVirtualAzul[INDEXARCHER2].Position = equipoAzul[INDEXARCHER2].Position;
        cArquero[1].restablecerVida();
    }
    private void respawnArcher1Rojo(){

        npcVirtualRojo[INDEXARCHER1].Position = equipoRojo[INDEXARCHER1].Position;
        rArquero[0].restablecerVida();
    }
    private void respawnArcher2Rojo(){

        npcVirtualRojo[INDEXARCHER2].Position = equipoRojo[INDEXARCHER2].Position;
        rArquero[1].restablecerVida();
    }

     private void respawnPesada1Azul(){

        npcVirtualAzul[INDEXPESADA1].Position = equipoAzul[INDEXPESADA1].Position;
        cPesada[0].restablecerVida();

    }
    private void respawnPesada2Azul(){

        npcVirtualAzul[INDEXPESADA2].Position = equipoAzul[INDEXPESADA2].Position;
        cPesada[1].restablecerVida();
    }
    private void respawnPesada1Rojo(){

        npcVirtualRojo[INDEXPESADA1].Position = equipoRojo[INDEXPESADA1].Position;
        rPesada[0].restablecerVida();
    }
    private void respawnPesada2Rojo(){

        npcVirtualRojo[INDEXPESADA2].Position = equipoRojo[INDEXPESADA2].Position;
        rPesada[1].restablecerVida();
    }
    private void respawnPatrullaAzul(){

        npcVirtualAzul[INDEXVIGILANTE].Position = equipoAzul[INDEXVIGILANTE].Position;
        cPatrulla.restablecerVida();

    }
    private void respawnPatrullaRojo(){

        npcVirtualRojo[INDEXVIGILANTE].Position = equipoRojo[INDEXVIGILANTE].Position;
        rPatrulla.restablecerVida();

    }
    private void respawnExploradorAzul(){

        npcVirtualAzul[INDEXEXPLORADOR].Position = equipoAzul[INDEXEXPLORADOR].Position;
        rExplorador.restablecerVida();

    }
    private void respawnExploradorRojo(){

        npcVirtualRojo[INDEXEXPLORADOR].Position = equipoRojo[INDEXEXPLORADOR].Position;
        rExplorador.restablecerVida();

    }
    
    // Ataques

    private void ataqueArcher1Azul(){

        int index = 0;
        int tipo = -1;
        int enemy = cArquero[index].Atacar(grFinal,teamRed,unidadesRojo,out tipo);
        if (enemy != -1)
        {
            
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROROJO:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero rojo enemigo
                        if (!rArquero[0].getMuerto())
                            rArquero[0].setVida(cArquero[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        
                        if (!rArquero[1].getMuerto())
                            rArquero[1].setVida(cArquero[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADORROJO:{
                    
                    if (!rExplorador.getMuerto())
                        rExplorador.setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.PATRULLAROJO:{

                    if (!rPatrulla.getMuerto())
                        rPatrulla.setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAROJO:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada roja enemiga
                        if (!rPesada[0].getMuerto())
                            rPesada[0].setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!rPesada[0].getMuerto())
                            rPesada[1].setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!cArquero[index].getMuerto())
            {
                cArquero[index].setComportamiento(Archer.ATACAR);
            }
            
        }

    }
    private void ataqueArcher2Azul(){

        int index = 1;
        int tipo = -1;
        int enemy = cArquero[index].Atacar(grFinal,teamRed,unidadesRojo,out tipo);
        if (enemy != -1)
        {
            
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROROJO:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero rojo enemigo
                        if (!rArquero[0].getMuerto())
                            rArquero[0].setVida(cArquero[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        
                        if (!rArquero[1].getMuerto())
                            rArquero[1].setVida(cArquero[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADORROJO:{
                    
                    if (!rExplorador.getMuerto())
                        rExplorador.setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.PATRULLAROJO:{

                    if (!rPatrulla.getMuerto())
                        rPatrulla.setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAROJO:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada roja enemiga
                        if (!rPesada[0].getMuerto())
                            rPesada[0].setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!rPesada[0].getMuerto())
                            rPesada[1].setVida(cArquero[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!cArquero[index].getMuerto())
            {
                cArquero[index].setComportamiento(Archer.ATACAR);
            }
            
        }

    }
    private void ataqueArcher1Rojo(){

        int index = 0;
        int tipo = -1;
        int enemy = rArquero[index].Atacar(grFinal,teamBlue,unidadesAzul,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROAZUL:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero rojo enemigo
                        if (!cArquero[0].getMuerto())
                            cArquero[0].setVida(rArquero[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!cArquero[1].getMuerto())
                            cArquero[1].setVida(rArquero[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADOAZUL:{
                    
                    if (!cExplorador.getMuerto())
                        cExplorador.setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.PATRULLAAZUL:{
                    if (!cPatrulla.getMuerto())
                        cPatrulla.setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAAZUL:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!cPesada[0].getMuerto())
                            cPesada[0].setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!cPesada[0].getMuerto())
                            cPesada[1].setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!rArquero[index].getMuerto())
            {
                rArquero[index].setComportamiento(Archer.ATACAR);
            }
        }

    }
    private void ataqueArcher2Rojo(){

        int index = 1;
        int tipo = -1;
        int enemy = rArquero[index].Atacar(grFinal,teamBlue,unidadesAzul,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROAZUL:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero rojo enemigo
                        if (!cArquero[0].getMuerto())
                            cArquero[0].setVida(rArquero[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!cArquero[1].getMuerto())
                            cArquero[1].setVida(rArquero[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADOAZUL:{
                    
                    if (!cExplorador.getMuerto())
                        cExplorador.setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.PATRULLAAZUL:{
                    if (!cPatrulla.getMuerto())
                        cPatrulla.setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAAZUL:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!cPesada[0].getMuerto())
                            cPesada[0].setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!cPesada[0].getMuerto())
                            cPesada[1].setVida(rArquero[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!rArquero[index].getMuerto())
            {
                rArquero[index].setComportamiento(Archer.ATACAR);
            }
        }

    }

     private void ataquePesada1Azul(){

        int index = 0;
        int tipo = -1;
        int enemy = cPesada[index].Atacar(grFinal,teamRed,unidadesRojo,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROROJO:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if (!rArquero[0].getMuerto())
                            rArquero[0].setVida(cPesada[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!rArquero[1].getMuerto())
                            rArquero[1].setVida(cPesada[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADORROJO:{
                    if (!rExplorador.getMuerto())
                        rExplorador.setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAROJO:{
                    if (!rPatrulla.getMuerto())
                        rPatrulla.setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAROJO:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!rPesada[0].getMuerto())
                            rPesada[0].setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!rPesada[1].getMuerto())
                            rPesada[1].setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!cPesada[index].getMuerto())
            {
                cPesada[index].setComportamiento(UnidadPesada.ATACAR);
            }
        }

    }
    private void ataquePesada2Azul(){

        int index = 1;
        int tipo = -1;
        int enemy = cPesada[index].Atacar(grFinal,teamRed,unidadesRojo,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROROJO:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if (!rArquero[0].getMuerto())
                            rArquero[0].setVida(cPesada[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!rArquero[1].getMuerto())
                            rArquero[1].setVida(cPesada[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADORROJO:{
                    if (!rExplorador.getMuerto())
                        rExplorador.setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAROJO:{
                    if (!rPatrulla.getMuerto())
                        rPatrulla.setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAROJO:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!rPesada[0].getMuerto())
                            rPesada[0].setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!rPesada[1].getMuerto())
                            rPesada[1].setVida(cPesada[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!cPesada[index].getMuerto())
            {
                cPesada[index].setComportamiento(UnidadPesada.ATACAR);
            }
        }
    }
    private void ataquePesada1Rojo(){

        int index = 0;
        int tipo = -1;
        int enemy = rPesada[index].Atacar(grFinal,teamBlue,unidadesAzul,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROAZUL:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if (!cArquero[0].getMuerto())
                            cArquero[0].setVida(rPesada[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!cArquero[1].getMuerto())
                            cArquero[1].setVida(rPesada[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADOAZUL:{
                    if (!cExplorador.getMuerto())
                        cExplorador.setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAAZUL:{
                    if (!cPatrulla.getMuerto())
                        cPatrulla.setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAAZUL:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!cPesada[0].getMuerto())
                            cPesada[0].setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!cPesada[1].getMuerto())
                            cPesada[1].setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!rPesada[index].getMuerto())
            {
                rPesada[index].setComportamiento(UnidadPesada.ATACAR);
            }
        }

    }
    private void ataquePesada2Rojo(){

        int index = 0;
        int tipo = -1;
        int enemy = rPesada[index].Atacar(grFinal,teamBlue,unidadesAzul,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROAZUL:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if (!cArquero[0].getMuerto())
                            cArquero[0].setVida(rPesada[index].getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if (!cArquero[1].getMuerto())
                            cArquero[1].setVida(rPesada[index].getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADOAZUL:{
                    if (!cExplorador.getMuerto())
                        cExplorador.setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAAZUL:{
                    if (!cPatrulla.getMuerto())
                        cPatrulla.setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAAZUL:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if (!cPesada[0].getMuerto())
                            cPesada[0].setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if (!cPesada[1].getMuerto())
                            cPesada[1].setVida(rPesada[index].getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!rPesada[index].getMuerto())
            {
                rPesada[index].setComportamiento(UnidadPesada.ATACAR);
            }
        }

    }
    private void ataquePatrullaRojo(){

        int tipo = -1;
        int enemy = rPatrulla.Atacar(grFinal,teamBlue,unidadesAzul,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROAZUL:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if(!cArquero[0].getMuerto())
                            cArquero[0].setVida(rPatrulla.getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if(!cArquero[1].getMuerto())
                            cArquero[1].setVida(rPatrulla.getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADOAZUL:{
                    if(!cExplorador.getMuerto())
                        cExplorador.setVida(rPatrulla.getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAAZUL:{
                    if(!cPatrulla.getMuerto())
                        cPatrulla.setVida(rPatrulla.getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAAZUL:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if(!cPesada[0].getMuerto())
                            cPesada[0].setVida(rPatrulla.getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if(!cPesada[1].getMuerto())
                            cPesada[1].setVida(rPatrulla.getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!rPatrulla.getMuerto())
            {
                rPatrulla.setComportamiento(Patrulla.ATACAR);
            }
        }

    }

    private void ataquePatrullaAzul(){

        int tipo = -1;
        int enemy = cPatrulla.Atacar(grFinal,teamRed,unidadesRojo,out tipo);

        if (enemy != -1)
        {
            switch (tipo)
            {
                case ArrayUnidades.ARQUEROROJO:{
                    if (enemy == INDEXARCHER1)
                    {
                        //arquero azul enemigo
                        if(!rArquero[0].getMuerto())
                        rArquero[0].setVida(cPatrulla.getDaño(AgentNPC.ARQUERO));
                        
                    }else{
                        if(!rArquero[1].getMuerto())
                        rArquero[1].setVida(cPatrulla.getDaño(AgentNPC.ARQUERO));
                    }
                    break;
                }
                case ArrayUnidades.EXPLORADORROJO:{
                    if(!rExplorador.getMuerto())
                        rExplorador.setVida(cPatrulla.getDaño(AgentNPC.PESADA));
                    
                    break;
                }
                case ArrayUnidades.PATRULLAROJO:{
                    if(!rPatrulla.getMuerto())
                        rPatrulla.setVida(cPatrulla.getDaño(AgentNPC.PESADA));
                    break;
                }
                case ArrayUnidades.UNIDADPESADAROJO:{
                    if (enemy == INDEXPESADA1)
                    {
                        //Unidad Pesada azul enemiga
                        if(!rPesada[0].getMuerto())
                            rPesada[0].setVida(cPatrulla.getDaño(AgentNPC.PESADA));
                        
                    }else{
                        if(!rPesada[1].getMuerto())
                            rPesada[1].setVida(cPatrulla.getDaño(AgentNPC.PESADA));
                    }
                    break;
                }
                default:
                break;
            }
            if (!cPatrulla.getMuerto())
            {
                cPatrulla.setComportamiento(Patrulla.ATACAR);
            }
        }

    }


}
