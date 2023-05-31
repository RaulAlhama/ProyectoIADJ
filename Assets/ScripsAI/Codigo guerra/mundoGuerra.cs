using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] puenteDerecho;
    public GameObject[] puenteIzquierdo;
    //public GameObject[] santuario;

    private AgentNPC[] equipoRojo = new AgentNPC[6];
    private Agent[] npcVirtualRojo = new Agent[6];

    private AgentNPC[] equipoAzul = new AgentNPC[numNPC];
    private Agent[] npcVirtualAzul = new Agent[numNPC];
    private BuscaCaminos[] buscadoresAzul = new BuscaCaminos[numNPC];


    public AgentNPC prefabNPCAzul;
    public AgentNPC prefabNPCRojo;
    private const int numNPC = 6;
    private const int numObjetives = 4;
    private Archer[] cArquero;
    private UnidadPesada[] cPesada;
    private Explorador cExplorador;
    private Patrulla cPatrulla;

    // Minimapa
    public GameObject prefabPlano;
    public Material materialEquipoRojo;
    public Material materialEquipoAzul;
    public Material materialNeutro; 
    public Material materialPeleado;
    private GameObject[,] casillas_minimapa = new GameObject[34, 34];
    private GameObject minimapa;

    // informacion
    
    public ArrayUnidades unidades;

    //objetivos del mundo

    private Objetivo[] objetivosMundo;
    private List<Objetivo> objetivosTeamBlue;
    private List<Objetivo> objetivosTeamRed;
    
    private Coordenada[] spawnAzul = new Coordenada[6];
    private Posicion[] teamBlue = new Posicion[6];
    private Coordenada[] spawnRojo = new Coordenada[6];
    private Posicion[] teamRed = new Posicion[6];

    // waypoint explorador azul
    private Ruta rutaAzul;
    private Ruta rutaAzulPesada;

    private TextMesh[,] grid;

    void Start()
    {
        cArquero = new Archer[2];
        cPesada = new UnidadPesada[2];
        unidades = new ArrayUnidades(rows,cols);
        objetivosMundo = new Objetivo[numObjetives];
        objetivosTeamBlue = new List<Objetivo>();
        objetivosTeamRed = new List<Objetivo>();
        grFinal = new GridFinal(rows,cols,cellSize);
        grFinal.setDistancia(2);

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

            buscadoresAzul[i] = new BuscaCaminos(grFinal,equipoAzul[i],npcVirtualAzul[i]);
            equipoAzul[i].Position = new Vector3(spawnAzul[i].getX(),0,spawnAzul[i].getY());
            grFinal.getCoordenadas(equipoAzul[i].Position,out iAux,out jAux);
            teamBlue[i] = new Posicion(iAux,jAux);

            equipoRojo[i] = Instantiate(prefabNPCRojo);
            equipoRojo[i].Position = new Vector3(spawnRojo[i].getX(),0,spawnRojo[i].getY());
        }
        
        setWaypointsBlue();
        setTipos();
        setPosiciones();
        setTorreVigia();
        setArmeria();
        setPuenteIzqAzul();
        setPuenteDerAzul();
        //creaTexto();
        inicializarMinimapa();
     
    }

    // Función que inicializa todas las casillas del minimapa a neutro, 
    // recorriendo cada una de las casillas del grid y creando un plano del tamaño de una casilla
    private void setWaypointsBlue(){

        //way point base azul
        WayPoint[] puntos = new WayPoint[10];
        WayPoint[] puntos2 = new WayPoint[13];
        
        // way points lado azul izquierdo

        Posicion p1 = new Posicion(49,10);
        puntos[0] = new WayPoint(true,p1);

        Posicion p2 = new Posicion(41,10);
        puntos[1] = new WayPoint(true,p2);

        Posicion p3 = new Posicion(41,13);
        puntos[2] = new WayPoint(true,p3);

        Posicion p4 = new Posicion(29,13);
        puntos[3] = new WayPoint(true,p4);

        Posicion p5 = new Posicion(16,16);  // torre vigia
        puntos[4] = new WayPoint(false,p5,WayPoint.TORRE_VIGIA);

        Posicion p6 = new Posicion(29,23);
        puntos[5] = new WayPoint(true,p6);

        Posicion p7 = new Posicion(32,23);
        puntos[6] = new WayPoint(true,p7);

        Posicion p8 = new Posicion(32,35);
        puntos[7] = new WayPoint(true,p8);

        Posicion p9 = new Posicion(14,35);
        puntos[8] = new WayPoint(true,p9);

        Posicion p10 = new Posicion(11,46);  // puente izquierdo
        puntos[9] = new WayPoint(false,p10,WayPoint.PUENTE_IZQUIERDO);

        // way points lado azul derecho

        Posicion p11 = new Posicion(48,22);  // armeria
        puntos2[0] = new WayPoint(false,p11,WayPoint.ARMERIA);

        Posicion p12 = new Posicion(58,11);  // deshabilitar si armeria habilitado
        puntos2[1] = new WayPoint(true,p12,WayPoint.CON_ARMERIA);

        Posicion p13 = new Posicion(53,13);  
        puntos2[2] = new WayPoint(true,p13);

        Posicion p14 = new Posicion(70,13); 
        puntos2[3] = new WayPoint(true,p14); 

        Posicion p15 = new Posicion(70,8);  
        puntos2[4] = new WayPoint(true,p15);

        Posicion p16 = new Posicion(84,9);  
        puntos2[5] = new WayPoint(true,p16);

        Posicion p17 = new Posicion(84,19);  
        puntos2[6] = new WayPoint(true,p17);

        Posicion p18 = new Posicion(86,19);  
        puntos2[7] = new WayPoint(true,p18);

        Posicion p19 = new Posicion(85,29); 
        puntos2[8] = new WayPoint(true,p19);

        Posicion p20 = new Posicion(88,29);   
        puntos2[9] = new WayPoint(true,p20);

        Posicion p21 = new Posicion(87,39);  
        puntos2[10] = new WayPoint(true,p21);

        Posicion p22 = new Posicion(90,39); 
        puntos2[11] = new WayPoint(true,p22);

        Posicion p23 = new Posicion(89,46);  // puente derecho 
        puntos2[12] = new WayPoint(false,p23,WayPoint.PUENTE_DERECHO);

        rutaAzul = new Ruta(puntos,puntos2);
        rutaAzulPesada = new Ruta(puntos,puntos2);
        

    }
    private void inicializarMinimapa(){

        Vector3 ajuste = new Vector3(6,-0.2f,6);
        minimapa = new GameObject("minimapa");
        int contx=0;
        int conty=0;

        for (int j = 0; j < cols; j+=3){

            contx=0;

            for (int i = 0; i< rows; i+=3){

                GameObject plane = Instantiate(prefabPlano, minimapa.transform);
                plane.name = "minimap_" + contx + "_" + conty;
                casillas_minimapa[contx,conty] = plane;
                plane.transform.localScale = new Vector3(cellSize/3.33f, 1f, cellSize/3.33f);
                plane.transform.position = grFinal.getPosicionReal(i,j) + ajuste;
                //Debug.Log("(" + i + "," + j + ") = " + grFinal.getPosicionReal(i,j));
                plane.layer = 7;
                plane.GetComponent<Renderer>().material = materialNeutro;

                contx++;
            }

            conty++;
        }

    }

    private void actualizarMinimapa(){

        for (int z=0;z<objetivosMundo.Length;z++){

            if (objetivosMundo[z].getPropiedad() != Objetivo.NEUTRAL){

                int xinicial = objetivosMundo[z].getXInicial();
                int xfinal = objetivosMundo[z].getXFinal();
                int yinicial = objetivosMundo[z].getYInicial();
                int yfinal = objetivosMundo[z].getYFinal();

                for (int j=yinicial-2;j<=yfinal+2;j++){
                            
                    for (int i=xinicial-2;i<=xfinal+2;i++){

                        // Actualizamos las casillas del minimapa
                        //GameObject casilla = GameObject.Find("minimap_" + i + "_" + j);
                        //GameObject casilla = casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)];

                        if (objetivosMundo[z].getPropiedad() == Objetivo.AZUL)
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().material = materialEquipoAzul;

                        else
                            casillas_minimapa[Mathf.FloorToInt(i/3), Mathf.FloorToInt(j/3)].GetComponent<Renderer>().material = materialEquipoRojo;
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
        objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL] = puenteIzqAzul;
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
        objetivosMundo[INDEX_PUENTE_DERECHO_AZUL] = puenteDerAzul;
    }
    private void setUnidades(AgentNPC npc, int eq){

        int x = 0;
        int y = 0;

        switch (npc.getTipo())
        {
            case AgentNPC.ARQUERO:
                
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.ARQUEROAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.ARQUEROROJO);
                }
            break;
            case AgentNPC.PESADA:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.UNIDADPESADAAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.UNIDADPESADAROJO);
                }
            break;
            case AgentNPC.EXPLORADOR:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.EXPLORADOAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.EXPLORADORROJO);
                }
            break;
            case AgentNPC.PATRULLA:
                if(eq == 0){

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.PATRULLAAZUL);
                }else{

                    grFinal.getCoordenadas(npc.Position, out x, out y);
                    unidades.setUnidad(x,y,ArrayUnidades.PATRULLAROJO);
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

        // Laberinto Azul
        GameObject[] obstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
        grFinal.setObstaculos(obstaculos);
        
    }
    private void setTipos(){

        equipoAzul[INDEXEXPLORADOR].setTipo(AgentNPC.EXPLORADOR);
        equipoAzul[INDEXEXPLORADOR].name = "Explorador Azul";
        equipoAzul[INDEXEXPLORADOR].MaxSpeed = 20;
        equipoAzul[INDEXEXPLORADOR].MaxAngularAcc = 90;
        equipoAzul[INDEXEXPLORADOR].MaxAcceleration = 20;
        
        cExplorador = new Explorador(INDEXEXPLORADOR);

        equipoAzul[INDEXARCHER1].setTipo(AgentNPC.ARQUERO);
        cArquero[0] = new Archer(INDEXARCHER1);
        equipoAzul[INDEXARCHER1].name = "Arquero Azul 1";
        equipoAzul[INDEXARCHER1].MaxSpeed = 16;
        equipoAzul[INDEXARCHER1].MaxAngularAcc = 120;
        equipoAzul[INDEXARCHER1].MaxAcceleration = 16;

        equipoAzul[INDEXARCHER2].setTipo(AgentNPC.ARQUERO);
        cArquero[1] = new Archer(INDEXARCHER2);
        equipoAzul[INDEXARCHER2].name = "Arquero Azul 2";
        equipoAzul[INDEXARCHER2].MaxSpeed = 16;
        equipoAzul[INDEXARCHER2].MaxAngularAcc = 120;
        equipoAzul[INDEXARCHER2].MaxAcceleration = 16;

        equipoAzul[INDEXPESADA1].setTipo(AgentNPC.PESADA);
        cPesada[0] = new UnidadPesada(INDEXPESADA1);
        equipoAzul[INDEXPESADA1].name = "Unidad Pesada Azul 1";

        equipoAzul[INDEXPESADA2].setTipo(AgentNPC.PESADA);
        cPesada[1] = new UnidadPesada(INDEXPESADA2);
        equipoAzul[INDEXPESADA2].name = "Unidad Pesada Azul 2";

        equipoAzul[INDEXVIGILANTE].setTipo(AgentNPC.PATRULLA);
        cPatrulla = new Patrulla(INDEXVIGILANTE);
        equipoAzul[INDEXVIGILANTE].name = "Patrulla Azul";

        equipoRojo[0].setTipo(AgentNPC.ARQUERO);
        /*equipoRojo[1].setTipo(AgentNPC.ARQUERO);
        equipoRojo[2].setTipo(AgentNPC.ARQUERO);
        equipoRojo[3].setTipo(AgentNPC.PESADA);
        equipoRojo[4].setTipo(AgentNPC.PESADA);
        equipoRojo[5].setTipo(AgentNPC.PATRULLA);*/
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
                //textMesh.text = i + "," + j;
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

    void Update(){

        actualizarMinimapa();
        moverNPC();
        verificaTorreVigia();
        verificaArmeria();
        verificaPuenteAzulDerecho();
        verificaPuenteAzulIzquierdo();
        if (Input.GetMouseButtonDown(1))
        {   
            foreach (Objetivo item in objetivosTeamBlue)
            {
                Debug.Log(item.getNombre()+" : "+item.getPropiedad());
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            seleccionarNPC();
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
                AgentNPC selectAgent = npcObject.GetComponent<AgentNPC>();
                foreach(AgentNPC pl in equipoAzul)
                {
                    if(!pl.Equals(selectAgent))
                    {
                        pl.quitarMarcador();
                    }else{
                        pl.activarMarcador();
                    }
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
                    movArcher(equipoAzul[i],0);
                }else{

                    movArcher(equipoAzul[i],1);
                }
                
            }else if (equipoAzul[i].getTipo() == AgentNPC.PESADA)
            {
                if (i == INDEXPESADA1)
                {
                    movPesada(equipoAzul[i],0);
                }else{

                    movPesada(equipoAzul[i],1);
                }
            }
            else if (equipoAzul[i].getTipo() == AgentNPC.EXPLORADOR)
            {
                movExplorer(equipoAzul[i]);
            }
            else if (equipoAzul[i].getTipo() == AgentNPC.PATRULLA)
            {
                movPatrulla(equipoAzul[i]);
            }
        }
    }
    private void movArcher(AgentNPC pl, int index){
        
        int xDespues;
        int yDespues;
        int indice = cArquero[index].getIndexNPC();

        if(pl.getLLegada()){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            cArquero[index].setLimites(i,j);
            npcVirtualAzul[indice].Position = cArquero[index].getDecision(grFinal,objetivosTeamBlue,unidades.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            buscadoresAzul[indice].LRTA();
            
        }else if((pl.status == Agent.STOPPED)){
                            
            buscadoresAzul[indice].LRTA();
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);
        
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidades.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidades.setUnidad(xDespues,yDespues,ArrayUnidades.ARQUEROAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul;  
        }  
    }
    private void movPesada(AgentNPC pl, int index){

        int xDespues;
        int yDespues;
        int indice = cPesada[index].getIndexNPC();
        
        if(pl.getLLegada()){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
                    
            cPesada[index].setLimites(i,j);
            npcVirtualAzul[indice].Position = cPesada[index].getDecision(rutaAzulPesada,grFinal,objetivosTeamBlue,unidades.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            buscadoresAzul[indice].LRTA();
            
        }else if((pl.status == Agent.STOPPED)){
                            
            buscadoresAzul[indice].LRTA();
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);

  
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidades.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidades.setUnidad(xDespues,yDespues,ArrayUnidades.UNIDADPESADAAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul; 
        }  
    }
    private void verificaTorreVigia(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_TORRE_VIGIA].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_TORRE_VIGIA].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_TORRE_VIGIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_TORRE_VIGIA]);

                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
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
        }else if (objetivosMundo[INDEX_TORRE_VIGIA].getPropiedad() == Objetivo.AZUL)
        {
            if (!rutaAzul.getDisponible(WayPoint.TORRE_VIGIA))
            {
                rutaAzul.setDisponible(WayPoint.TORRE_VIGIA);
            }
        }
        
    }
    private void verificaPuenteAzulIzquierdo(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteIzquierdo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteIzquierdo)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].setPropiedad(Objetivo.ROJO);
            }
        }else if (objetivosMundo[INDEX_PUENTE_IZQUIERDO_AZUL].getPropiedad() == Objetivo.AZUL)
        {
            if (!rutaAzul.getDisponible(WayPoint.PUENTE_IZQUIERDO))
            {
                rutaAzul.setDisponible(WayPoint.PUENTE_IZQUIERDO);
            }
        }
        
    }
    private void verificaPuenteAzulDerecho(){

        int contAzul = 0;
        int contRojo = 0;
        if (objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getPropiedad() == Objetivo.NEUTRAL)
        {
            foreach (Coordenada coor in objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getSlots())
            {
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
                            && !objetivosTeamBlue.Contains(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]))
                {
                    objetivosTeamBlue.Add(objetivosMundo[INDEX_PUENTE_DERECHO_AZUL]);
                }
            }
            if(contAzul > 0 && contRojo == 0){

                foreach (GameObject item in puenteDerecho)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = azul;

                }
                objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].setPropiedad(Objetivo.AZUL);
            }else if(contRojo > 0 && contAzul == 0){

                foreach (GameObject item in puenteDerecho)
                {
                    Renderer renderer = item.GetComponent<Renderer>(); // Obtén el componente Renderer
                    renderer.material = rojo;
                }
                objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].setPropiedad(Objetivo.ROJO);
            }
        }else if (objetivosMundo[INDEX_PUENTE_DERECHO_AZUL].getPropiedad() == Objetivo.AZUL)
        {
            if (!rutaAzul.getDisponible(WayPoint.PUENTE_DERECHO))
            {
                rutaAzul.setDisponible(WayPoint.PUENTE_DERECHO);
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
                if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADOAZUL)
                {
                    contAzul++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO && unidades.getValorUnidad(coor.getX(),coor.getY()) != ArrayUnidades.EXPLORADORROJO)
                {
                    contRojo++;
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCROJO 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADORROJO
                            && !objetivosTeamRed.Contains(objetivosMundo[INDEX_ARMERIA]))
                {
                    objetivosTeamRed.Add(objetivosMundo[INDEX_ARMERIA]);
                    
                }else if (grFinal.getValor(coor.getX(),coor.getY()) == GridFinal.NPCAZUL 
                            && unidades.getValorUnidad(coor.getX(),coor.getY()) == ArrayUnidades.EXPLORADOAZUL
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
        }else if (objetivosMundo[INDEX_ARMERIA].getPropiedad() == Objetivo.AZUL)
        {
            if (!rutaAzul.getDisponible(WayPoint.ARMERIA))
            {
                rutaAzul.setDisponible(WayPoint.ARMERIA);
            }
        }
        
    }

    private void movExplorer(AgentNPC pl){

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
            npcVirtualAzul[indice].Position = cExplorador.getDecision(grFinal,objetivosMundo,unidades.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            buscadoresAzul[indice].LRTA();
            
        }else if((pl.status == Agent.STOPPED)){
                            
            buscadoresAzul[indice].LRTA();
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidades.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidades.setUnidad(xDespues,yDespues,ArrayUnidades.EXPLORADOAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            GameObject casilla = casillas_minimapa[xcasilla,ycasilla];
            casilla.GetComponent<Renderer>().material = materialEquipoAzul;  
        }  
    }
    private void movPatrulla(AgentNPC pl){

        int xDespues;
        int yDespues;
        int indice = cPatrulla.getIndexNPC();
        if(pl.getLLegada()){
                    
            int i;
            int j;
            grFinal.getCoordenadas(pl.Position,out i, out j);
                    
            int iObjetivo = i;
            int jObjetivo = j;
            
            cPatrulla.setLimites(i,j);
            npcVirtualAzul[indice].Position = cPatrulla.getDecision(grFinal,rutaAzul,unidades.getArray(),i,j) + new Vector3(2,0,2);
            grFinal.getCoordenadas(npcVirtualAzul[indice].Position,out iObjetivo,out jObjetivo);
                    
            buscadoresAzul[indice].setObjetivos(iObjetivo,jObjetivo, npcVirtualAzul[indice]);
            buscadoresAzul[indice].setGrafoMovimiento(grFinal.getGrafo(iObjetivo,jObjetivo));
            buscadoresAzul[indice].LRTA();
            
        }else if((pl.status == Agent.STOPPED)){
                            
            buscadoresAzul[indice].LRTA();
        }
        grFinal.getCoordenadas(pl.Position,out xDespues,out yDespues);


        // Actualizamos las casillas por las que se trasladan los personaje
        
                
        if(teamBlue[indice].getI() != xDespues || teamBlue[indice].getJ() != yDespues){
            
            grFinal.setValor(teamBlue[indice].getI(),teamBlue[indice].getJ(),GridFinal.LIBRE);
            unidades.setUnidad(teamBlue[indice].getI(),teamBlue[indice].getJ(),ArrayUnidades.LIBRE);

            //grid[teamBlue[indice].getI(),teamBlue[indice].getJ()].text = "" + grFinal.getValor(teamBlue[indice].getI(),teamBlue[indice].getJ()) + "  " + teamBlue[indice].getI() + "," + teamBlue[indice].getJ();

            grFinal.setValor(xDespues,yDespues,GridFinal.NPCAZUL);
            unidades.setUnidad(xDespues,yDespues,ArrayUnidades.PATRULLAAZUL);
            teamBlue[indice].setNueva(xDespues,yDespues);

            //grid[xDespues,yDespues].text = "" + grFinal.getValor(xDespues,yDespues) + "  " + xDespues + "," + yDespues;
                  // Actualizamos las casillas por las que se trasladan los personajes
            int xcasilla = Mathf.FloorToInt(pl.Position.x / 12);
            int ycasilla = Mathf.FloorToInt(pl.Position.z / 12);
            casillas_minimapa[xcasilla,ycasilla].GetComponent<Renderer>().material = materialEquipoAzul;  
        }  
    }
}
