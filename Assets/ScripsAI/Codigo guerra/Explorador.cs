using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorador : MonoBehaviour
{
    // Start is called before the first frame update
    private int pj;
    private string equipo;
    private GameObject referencia;
    private AgentNPC npc;
    private Coordenada spawn;
    private int santuario = 1;
    private int delay = 10;
    private bool muerto = false;
    private int rangoVision = 11;
    private int centroVision;
    private int indexSpawn;
    private int[,] mVision;
    private int[] limVision;
    private const int MAXVALOR = 99;
    private const int MINVALOR = 0;
    private int vida = 80;
    private GameObject[] barra_vida;

    private int com;
    private int lastCom;
    private int posI;
    private int posJ;
    private int estado;

    //Equipos
    public const string ROJO = "FF0000";
    public const string AZUL = "0000FF"; 

    public const int MOVERSE = 20;
    public const int HUIR = 21;
    public const int QUIETO = 23;
    
    //Estado
    public const int MUERTO = 11;
    public const int VIVO = 12;
    public const int SPAWNING = 13;

    public Explorador(int index,GameObject[] v,AgentNPC n,Coordenada c,string eq){

        mVision = new int[rangoVision,rangoVision];
        centroVision = (rangoVision - 1)/2;
        com = Explorador.QUIETO;
        pj = index;
        barra_vida = v;
        npc = n;
        spawn = c;
        estado = VIVO;
        equipo = eq;
    }
    public void setObjetoNPC(GameObject n){

        referencia = n;
    }
    public int getVida(){

        return vida;
    }
    public int getEstado(){

        return estado;
    }
    public void setEstado(int val){

        estado = val;
    }
    public bool getMuerto(){

        return muerto;
    }
    private void actualizaBarraDeVida(){

        if (vida <= 0)
        {
            barra_vida[0].SetActive(false);
        }else if (vida <20)
        {
            barra_vida[1].SetActive(false);
        }else if(vida < 40){

            barra_vida[2].SetActive(false);
        }else if(vida < 60){

            barra_vida[3].SetActive(false);
        }else if(vida < 80){

            barra_vida[4].SetActive(false);
        }
    }
    public void setVida(int val){

        vida = vida - val;
        actualizaBarraDeVida();
        if (vida == 0)
        {
            npc.Position = new Vector3(spawn.getX(),0,spawn.getY());
            muerto = true;
            estado = MUERTO;
            referencia.SetActive(false);
            com = QUIETO;
        }
    }
    public int getDelay(){

        return delay;
    }
    public void setNewDelay(int val){

        delay+=10;
    }
    public void restablecerVida(){

        vida = 80;
        for (int i = 0; i < barra_vida.Length - santuario; i++)
        {
            barra_vida[i].SetActive(true);
        }
        muerto = false;
        reaparecer();
    }
    private void reaparecer(){

        npc.setLLegada(true);
        referencia.SetActive(true);
        estado = VIVO;

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
    private bool enemigosEnVision(int[,] mundo){

        bool enemigos = false;

        for (int i = limVision[0]; i <= limVision[1]; i++)
        {
            for (int j = limVision[2]; j <= limVision[3]; j++)
            {
                if (equipo == AZUL && (mundo[i,j] >= ArrayUnidades.ARQUEROROJO && mundo[i,j] <= ArrayUnidades.PATRULLAROJO))
                {
                    enemigos = true;
                    break;
                    
                }else if(equipo == ROJO && (mundo[i,j] >= ArrayUnidades.ARQUEROAZUL && mundo[i,j] <= ArrayUnidades.PATRULLAAZUL)){
                    
                    break;
                    enemigos = true;
                }
            }
        }
        return enemigos;
    }
    public bool posicionObjetivo(List<Objetivo> lisObj, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = false;
        int menor = 99;
        int x1 = 0;
        int y1 = 0;
        double  distancia = 999999;
        List<Objetivo> listaN = new List<Objetivo>();
        List<Objetivo> listaE = new List<Objetivo>();

        if (equipo == AZUL)
        {
            for (int k = 0; k < lisObj.Count; k++)
            {
                if (lisObj[k].getPropiedad() == Objetivo.NEUTRAL)
                {
                    listaN.Add(lisObj[k]);
                }else if(lisObj[k].getPropiedad() == Objetivo.ROJO){
                    
                    listaE.Add(lisObj[k]);
                }
            }
        }else{

            for (int k = 0; k < lisObj.Count; k++)
            {
                if (lisObj[k].getPropiedad() == Objetivo.NEUTRAL)
                {
                    listaN.Add(lisObj[k]);
                }else if(lisObj[k].getPropiedad() == Objetivo.AZUL){
                    
                    listaE.Add(lisObj[k]);
                }
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
        }else if(listaE.Count > 0)
        {
            int index = Random.Range(0, listaE.Count-1);
            foreach (Coordenada cr in listaE[index].getSlots())
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
    public Vector3 getDecision(GridFinal mundo,List<Objetivo> listaObjetivos, int[,] azules,int[,] rojos, int i, int j){

        int x = i;
        int y = j;
        int[,] enemigos;
        int[,] aliados;

        Vector3 target = mundo.getPosicionReal(x,y);

        if (equipo == AZUL)
        {
            aliados = azules;
            enemigos = rojos;
        }else{
            
            aliados = rojos;
            enemigos = azules;
        }

        if(enemigosEnVision(enemigos)){

            target = new Vector3(spawn.getX(),0,spawn.getY());
            lastCom = com;
            com = Explorador.HUIR;

        }else if(posicionObjetivo(listaObjetivos,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Explorador.MOVERSE;
            
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
