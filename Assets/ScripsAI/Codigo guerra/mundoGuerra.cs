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


    public int rows;
    public int cols;
    public float cellSize;
    private GridFinal grFinal;
    public Color backgroundColor = Color.white;
    public GameObject wallPrefab;
    private AgentNPC[] equipoRojo = new AgentNPC[6];
    private AgentNPC[] equipoAzul = new AgentNPC[1];
    public AgentNPC prefabNPCAzul;
    public AgentNPC prefabNPCRojo;
    private const int numNPC = 1;
    private const int numObjetives = 4;
    private Archer cArquero;

    // informacion
    
    public ArrayUnidades unidades;

    //objetivos del mundo

    private Objetivo[] objetivosMundo;
    
    private Coordenada[] spawnAzul = new Coordenada[6];
    private Coordenada[] spawnRojo = new Coordenada[6];

    private TextMesh[,] grid;

    void Start()
    {
        cArquero = new Archer();
        unidades = new ArrayUnidades(rows,cols);
        objetivosMundo = new Objetivo[numObjetives];

        for(int i = 0; i < numNPC; i++){

            spawnAzul[i] = new Coordenada(190 + (i*4),10);
            spawnRojo[i] = new Coordenada(190 + (i*4),390);
        }
        for(int i = 0; i < numNPC; i++){

            equipoAzul[i] = Instantiate(prefabNPCAzul);
            equipoAzul[i].Position = new Vector3(spawnAzul[i].getX(),0,spawnAzul[i].getY());
            equipoRojo[i] = Instantiate(prefabNPCRojo);
            equipoRojo[i].Position = new Vector3(spawnRojo[i].getX(),0,spawnRojo[i].getY());
        }
        grFinal = new GridFinal(rows,cols,cellSize);
        setTipos();
        setPosiciones();
        setTorreVigia();
        setArmeria();
        setPuenteIzqAzul();
        setPuenteDerAzul();
        //creaTexto();
        
     
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
        objetivosMundo[0] = vigia;

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
        objetivosMundo[1] = armeria;
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
        objetivosMundo[2] = puenteIzqAzul;
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
        objetivosMundo[3] = puenteDerAzul;
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

        //equipoAzul[0].setTipo(AgentNPC.EXPLORADOR);
        equipoAzul[0].setTipo(AgentNPC.ARQUERO);
        //equipoAzul[2].setTipo(AgentNPC.ARQUERO);
        //equipoAzul[1].setTipo(AgentNPC.PESADA);
        //equipoAzul[4].setTipo(AgentNPC.PESADA);
        //equipoAzul[5].setTipo(AgentNPC.PATRULLA);
        
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

        if (Input.GetMouseButtonDown(1))
        {   
            moverNPC();
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

        foreach(AgentNPC pl in equipoAzul)
        {
            if(pl.getTipo() == AgentNPC.ARQUERO)
            {
                int i;
                int j;
                grFinal.getCoordenadas(pl.Position,out i, out j);
                cArquero.setLimites(i,j);
                Debug.Log(cArquero.getDesicion(grFinal,objetivosMundo,unidades.getArray(),i,j));

            }
        }
    }
}
