using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Archer
{
    // Start is called before the first frame update
    private int pj;
    private string equipo;
    private float atackSpeed = 1.5f;
    private int dañoBase = 2;
    private GameObject referencia;
    private AgentNPC npc;
    private Coordenada spawn;
    private int santuario = 1;
    private int delay = 10;
    private bool muerto = false;
    private int rangoVision = 11;
    private int rangoAtaque = 5;
    private int centroAtaque;
    private int centroVision;
    private int indexSpawn;
    private int[,] mAtaque;
    private int[,] mVision;
    private int[] limAtaque;
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

    //Comportamiento
    public const int MOVERSE = 20;
    public const int ATACAR = 21;
    public const int SEGUIR = 22;
    public const int QUIETO = 23;
    public const int RELOAD = 24;

    //Estado
    public const int MUERTO = 11;
    public const int VIVO = 12;
    public const int SPAWNING = 13;
    
    

    public Archer(int index, GameObject[] v,AgentNPC n,Coordenada c,string eq){

        mAtaque = new int[rangoAtaque,rangoAtaque];
        mVision = new int[rangoVision,rangoVision];
        centroAtaque = (rangoAtaque - 1)/2;
        centroVision = (rangoVision - 1)/2;
        com = Archer.QUIETO;
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
    public int getDaño(int tipo){

        switch (tipo)
        {
            case AgentNPC.ARQUERO:{

                return dañoBase;
            }
            case AgentNPC.EXPLORADOR:{

                return dañoBase*4;
            }
            case AgentNPC.PATRULLA:{

                return dañoBase*2;
            }
            case AgentNPC.PESADA:{

                return dañoBase/2;
            }
            default:
            return dañoBase;
        }
    }
    public float getAtackSpeed(){

        return atackSpeed;
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
    private bool enemigosEnRango(int[,] mundo,out int x, out int y){

        bool enemigos = false;
        int x1 = 0;
        int y1 = 0;
        for (int i = limAtaque[0]; i <= limAtaque[1]; i++)
        {
            for (int j = limAtaque[2]; j <= limAtaque[3]; j++)
            {
                if (equipo == AZUL && (mundo[i,j] >= ArrayUnidades.ARQUEROROJO && mundo[i,j] <= ArrayUnidades.PATRULLAROJO))
                {
                    enemigos = true;
                    x1 = i;
                    y1 = j;
                    
                }else if(equipo == ROJO && (mundo[i,j] >= ArrayUnidades.ARQUEROAZUL && mundo[i,j] <= ArrayUnidades.PATRULLAAZUL)){

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
    private bool enemigosEnVision(GridFinal valmundo,int[,] mundo, out int x, out int y){

        bool enemigos = false;
        int x1 = 0;
        int y1 = 0;
        for (int i = limVision[0]; i <= limVision[1]; i++)
        {
            for (int j = limVision[2]; j <= limVision[3]; j++)
            {
                if (equipo == AZUL && (mundo[i,j] >= ArrayUnidades.ARQUEROROJO && mundo[i,j] <= ArrayUnidades.PATRULLAROJO))
                {
                    enemigos = true;
                    x1 = i;
                    y1 = j;
                    
                }else if(equipo == ROJO && (mundo[i,j] >= ArrayUnidades.ARQUEROAZUL && mundo[i,j] <= ArrayUnidades.PATRULLAAZUL)){

                    enemigos = true;
                    x1 = i;
                    y1 = j;
                }
            }
        }
        List<Coordenada> slots = new List<Coordenada>();
        if(x1-1 >= MINVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1-1,y1-1) ){

            Coordenada cr = new Coordenada(x1-1,y1-1);
            slots.Add(cr);
        }
        if(y1-1 >= MINVALOR && valmundo.Posible(x1,y1-1)){

            Coordenada cr = new Coordenada(x1,y1-1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1+1,y1-1) ){

            Coordenada cr = new Coordenada(x1+1,y1-1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && valmundo.Posible(x1-1,y1) ){

            Coordenada cr = new Coordenada(x1-1,y1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1) ){

            Coordenada cr = new Coordenada(x1+1,y1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1-1,y1+1) ){

            Coordenada cr = new Coordenada(x1-1,y1+1);
            slots.Add(cr);
        }
        if( y1+1 <= MAXVALOR && valmundo.Posible(x1,y1+1) ){

            Coordenada cr = new Coordenada(x1,y1+1);
            slots.Add(cr);
        }
        if( x1+1 <= MAXVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1+1) ){

            Coordenada cr = new Coordenada(x1+1,y1+1);
            slots.Add(cr);
        }

        if (equipo == AZUL)
        {
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
        }else{

            double distancia = -1;

            foreach (Coordenada cr in slots)
            {
                double disAux = Mathf.Max(Mathf.Abs(posI+cr.getX()),Mathf.Abs(posJ+cr.getY()));
                if (disAux > distancia)
                {
                    distancia = disAux;
                    x1 = cr.getX();
                    y1 = cr.getY();
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
        for (int i = limVision[0]; i <= limVision[1]; i++)
        {
            for (int j = limVision[2]; j <= limVision[3]; j++)
            {
                if (equipo == AZUL && mundo[i,j] == ArrayUnidades.UNIDADPESADAAZUL)
                {
                    aliado = true;
                    x1 = i;
                    y1 = j;
                }else if (equipo == ROJO && mundo[i,j] == ArrayUnidades.UNIDADPESADAROJO)
                {
                    aliado = true;
                    x1 = i;
                    y1 = j;
                }
            }
        }
        List<Coordenada> slots = new List<Coordenada>();
        if(x1-1 >= MINVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1-1,y1-1) ){

            Coordenada cr = new Coordenada(x1-1,y1-1);
            slots.Add(cr);
        }
        if(y1-1 >= MINVALOR && valmundo.Posible(x1,y1-1)){

            Coordenada cr = new Coordenada(x1,y1-1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && y1-1 >= MINVALOR && valmundo.Posible(x1+1,y1-1) ){

            Coordenada cr = new Coordenada(x1+1,y1-1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && valmundo.Posible(x1-1,y1) ){

            Coordenada cr = new Coordenada(x1-1,y1);
            slots.Add(cr);
        }
        if(x1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1) ){

            Coordenada cr = new Coordenada(x1+1,y1);
            slots.Add(cr);
        }
        if(x1-1 >= MINVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1-1,y1+1) ){

            Coordenada cr = new Coordenada(x1-1,y1+1);
            slots.Add(cr);
        }
        if( y1+1 <= MAXVALOR && valmundo.Posible(x1,y1+1) ){

            Coordenada cr = new Coordenada(x1,y1+1);
            slots.Add(cr);
        }
        if( x1+1 <= MAXVALOR && y1+1 <= MAXVALOR && valmundo.Posible(x1+1,y1+1) ){

            Coordenada cr = new Coordenada(x1+1,y1+1);
            slots.Add(cr);
        }

        if (equipo == AZUL)
        {
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
        }else{

            double distancia = -1;

            foreach (Coordenada cr in slots)
            {
                double disAux = Mathf.Max(Mathf.Abs(posI+cr.getX()),Mathf.Abs(posJ+cr.getY()));
                if (disAux > distancia)
                {
                    distancia = disAux;
                    x1 = cr.getX();
                    y1 = cr.getY();
                }
            }
        }
        
        x = x1;
        y = y1;
        return aliado;
    }
    public bool posicionObjetivo(List<Objetivo> lisObj, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = false;
        int mayor = -1;
        int x1 = 0;
        int y1 = 0;
        double  distancia = 999999;
        List<Objetivo> posibles = new List<Objetivo>();

        for (int k = 0; k < lisObj.Count; k++)
        {
            if(equipo == AZUL && lisObj[k].getPropiedad() == Objetivo.AZUL){

                if (lisObj[k].getPrioridad() > mayor)
                {
                    mayor = lisObj[k].getPrioridad();
                    posibles.Clear();
                    posibles.Add(lisObj[k]);
                }else if (lisObj[k].getPrioridad() == mayor)
                {
                    posibles.Add(lisObj[k]);
                }
            }else if(equipo == ROJO && lisObj[k].getPropiedad() == Objetivo.ROJO){

                if (lisObj[k].getPrioridad() > mayor)
                {
                    mayor = lisObj[k].getPrioridad();
                    posibles.Clear();
                    posibles.Add(lisObj[k]);
                }else if (lisObj[k].getPrioridad() == mayor)
                {
                    posibles.Add(lisObj[k]);
                }
            }
        }
        if (posibles.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, posibles.Count-1);

            foreach (Coordenada cr in posibles[index].getSlots())
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
    public bool posicionRuta(Ruta ruta, int[,] PosMundo,int i,int j,out int x, out int y){
        
        bool objetivo = true;

        WayPoint aux = ruta.getAleatoreo();

        x = aux.getX();
        y = aux.getY();
        return objetivo;
    }
    public Vector3 getDecision(Ruta ruta,GridFinal mundo,List<Objetivo> listaObjetivos, int[,] azules,int[,] rojos, int i, int j){

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
        
        
        if(enemigosEnVision(mundo,enemigos,out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.ATACAR;

        }else if(unidadPesadaEnVision(mundo, aliados,out x, out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.SEGUIR;

        }else if(posicionObjetivo(listaObjetivos,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = Archer.MOVERSE;
        }else if(posicionRuta(ruta,mundo.getArray(),i,j,out x,out y)){

            target = mundo.getPosicionReal(x,y);
            lastCom = com;
            com = UnidadPesada.MOVERSE;
        }
        //buscar objetivo que no esta en propiedad y con menor prioridad (siginifa que es el mas cercano a la base) 
        
        return target;
    }
    public int Atacar(GridFinal mundo,Posicion[] teamEnemy,ArrayUnidades unidades, out int tipo){

        int x = 0;
        int y = 0;

        if(enemigosEnRango(unidades.getArray(),out x, out y)){

            lastCom = com;
            com = Archer.ATACAR;
            int indice = -1;
            int aux = -1;
            for (int i = 0; i < teamEnemy.Length; i++)
            {
                if (teamEnemy[i].getI() == x && teamEnemy[i].getJ() == y)
                {
                    aux = unidades.getValorUnidad(x,y);
                    indice = i;
                }
            }
            tipo = aux;
            return indice;

        }else{

            if (!muerto)
            {
                com = QUIETO; 
            }
            
            tipo = -1;
            return -1;
        }
    }
    public void setComportamiento(int val){

        com = val;
    }
    public int getComportamiento(){

        return com;
    }
    public bool cambioCom(){

        if(lastCom == com){

            return false;
        }else{

            return true;
        }
    }
}
