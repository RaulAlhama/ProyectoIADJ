using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MovimientoMouse : MonoBehaviour
{
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

    private int playersInWorld = 6;
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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public FormationManager getFormacion(Agent agent){
        foreach (FormationManager formacion in formaciones){
            foreach (SlotAssignment slot in formacion.slotAssignments){
                if (slot.character == agent)
                {
                    return formacion;
                }
            }

            if (formacion.lider == agent)
                return formacion;
        }
        return null;
    }

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

    public void comprobarFormacion(){

        foreach (FormationManager formacion in formaciones.ToList()){

            if (formacion.slotAssignments.Count == 0){
                formaciones.Remove(formacion);
                Destroy(formacion.pattern.gameObject);   
                Destroy(formacion.gameObject);
            }

        }
    }


    private Agent restauraFormacion(AgentNPC agent, Vector3 tr){

        
        Agent aux = npcVirtual;
        foreach (FormationManager formacion in formaciones){
            //ActualizaFormacion(formacion);
            foreach (SlotAssignment slot in formacion.slotAssignments){
                if (slot.character == agent){
                    
                    aux.Position = Bodi.VectorRotate(formacion.pattern.getSlotLocation(slot.slotNumber).position,((AgentNPC)formacion.lider).virtualTarget.Orientation) + tr;
                    aux.Orientation = formacion.pattern.getSlotLocation(slot.slotNumber).orientation + ((AgentNPC)formacion.lider).virtualTarget.Orientation;
                }
                    
            }
            
        }
        
        return aux;
    }


    private void ActualizaFormacion(FormationManager formacion){

        foreach(SlotAssignment slt in formacion.slotAssignments){

                foreach(BuscaCaminos bC in buscadores){

                    if(bC.pl.Equals(slt.character)){

                        bC.pl.inFormacion = true;
                        slt.character.inFormacion = true;
                        int iObjetivo;
                        int jObjetivo;
                        
                        bC.npcVirtual.Position = slt.target.Position;
                        mundo.getCoordenadas(slt.target.Position,out iObjetivo,out jObjetivo);

                        bC.buscador.setObjetivos(iObjetivo,jObjetivo, bC.npcVirtual);
                        bC.buscador.setPosicionNpcVirtual(mundo.getPosicionReal(iObjetivo,jObjetivo));
                        bC.buscador.setOrientacionNpcVirtual(bC.npcVirtual.Orientation);

                        bC.buscador.setGrafoMovimiento(mundo.getGrafo(iObjetivo,jObjetivo));
                        bC.buscador.LRTAestrella();
                        bC.pl.setStatus(Agent.STOPPED);
                        slt.character.setStatus(Agent.STOPPED);
                    }
                }
            }
    }
    


    void Start()
    {
        obstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
        grafoMovimiento = new double[21,21];
        mundo = new GridFinal(21,21,tam);

        buscadores = new List<BuscaCaminos>();
        selectedNPCs = new List<Agent>();

        player = Instantiate(player);
        AgentNPC player2 = Instantiate(player);
        AgentNPC player3 = Instantiate(player);
        AgentNPC player4 = Instantiate(player);
        AgentNPC player5 = Instantiate(player);
        AgentNPC player6 = Instantiate(player);
        

        player.Position = new Vector3(21,0,35);
        player2.Position = new Vector3(23,0,35);
        player3.Position = new Vector3(19,0,35);
        player4.Position = new Vector3(15,0,35);
        player5.Position = new Vector3(25,0,35);
        player6.Position = new Vector3(17,0,35);


        player.name = "NPC1";
        player2.name = "NPC2";
        player3.name = "NPC3";
        player4.name = "NPC4";
        player5.name = "NPC5";
        player6.name = "NPC6";
        
        mundo.setObstaculos(obstaculos);
        
        agregaPlayers(player);
        agregaPlayers(player2);
        agregaPlayers(player3);
        agregaPlayers(player4);
        agregaPlayers(player5);
        agregaPlayers(player6);
        
    }


    void Update()
    {

        // Comprobamos si los agentes tienen que volver a formar
        if (leaderFollowing)
        {

            comprobarFormacion();
        }

        // Comprueba si se ha hecho clic derecho en el mapa
        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   
            
            bool romperFormacion = false;
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

                if (leaderFollowing)
                {

                    foreach (Agent agent in selectedNPCs)
                    {   

                        // Si un agente de los seleccionados no está en formación, se debe romper la formación
                        if (agent.inFormacion == false){
                            romperFormacion = true;
                        }

                        agent.setTarget(npcVirtualPrefab);

                    }

                    // Comprobamos si hay que romper la formación
                    if (romperFormacion){

                        foreach (Agent agent in selectedNPCs)
                        {
                            // Se cambia el estado del agente
                            agent.inFormacion = false;
                            // Si el personaje está en una formación, esta se destruye
                            if (getFormacion(agent) != null){
                                Destroy(getFormacion(agent).pattern.gameObject);
                                Destroy(getFormacion(agent).gameObject);
                            }
                        }
                          

                    }

                    romperFormacion = false;
                }

                else {
                    
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

                                    if(bC.pl.inFormacion){

                                        //bC.npcVirtual.transform.position = Bodi.VectorRotate(restauraFormacion(bC.pl),bC.npcVirtual.Orientation) + targetPosition;
                                        //bC.npcVirtual.Position = (mundo.getPosicionReal(iObjetivo,jObjetivo) + new Vector3(tam/2,0,tam/2)) + restauraFormacion(bC.pl);
                                        Agent aux = restauraFormacion(bC.pl, targetPosition);
                                        bC.npcVirtual.Position = aux.Position;
                                        bC.npcVirtual.Orientation = aux.Orientation;
                                        mundo.getCoordenadas(bC.npcVirtual.transform.position,out iObjetivo,out jObjetivo);
                                        
                                        if(!mundo.Posible(iObjetivo,jObjetivo))
                                        {
                                            bC.pl.inFormacion = false;
                                            bC.pl.setStatus(Agent.NOSELECTED);
                                            break;
                                        }

                                    }
                                    
                                    else
                                    {

                                        bC.npcVirtual.transform.position = targetPosition;
                                    }
                                    
                                    
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
            }


        }

        else if (!leaderFollowing)
        {

            foreach (AgentNPC agent in selectedNPCs){

                foreach(BuscaCaminos bC in buscadores){

                        if(bC.pl.Equals(agent) && (bC.pl.getStatus() == Agent.STOPPED) && !bC.pl.getLLegada()){
                            
                            
                            bC.buscador.LRTAestrella();
                        }
                }
            }
        }

        //Si pulsamos en un personaje mientras pulsamos control, se añadirá a la lista de personajes seleccionados
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                if (npcObject.CompareTag("NPC"))
                {
                    Agent selectAgent = npcObject.GetComponent<AgentNPC>();

                    if (leaderFollowing)
                    {
                        if (selectedNPCs.Contains(selectAgent))
                        {
                            selectAgent.quitarMarcador();
                            selectedNPCs.Remove(selectAgent);
                        }
                        else
                        {
                            selectAgent.activarMarcador();
                            selectedNPCs.Add(selectAgent);
                        }
                    }

                    else
                    {
                        foreach (BuscaCaminos bC in buscadores)
                        {

                            if(bC.pl.getStatus() != Agent.NOSELECTED && bC.pl.Equals(selectAgent))
                            {
                                
                                if(!selectedNPCs.Contains(bC.pl)){

                                    selectedNPCs.Add(bC.pl);
                                    selectAgent.activarMarcador();
                                }
                                    
                                else{

                                    selectAgent.quitarMarcador();
                                    selectedNPCs.Remove(bC.pl);
                                }
                                    
                            }
                            else if(bC.pl.Equals(selectAgent))
                            {
                                
                                if(selectedNPCs.Contains(bC.pl)){

                                    bC.pl.quitarMarcador();
                                    selectedNPCs.Remove(bC.pl);
                                }
                                    
                            }
                        }

                    }


                }
                
            }
        }

        
        //Si pulsamos en un personaje y no tenemos pulsado CONTROL la lista solo contendrá a este personaje
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                if (npcObject.CompareTag("NPC"))
                {
                    Agent selectAgent = npcObject.GetComponent<AgentNPC>();
                    
                    if (selectedNPCs.Count > 0)
                    { //Si hay varios personajes seleccionados, los deseleccionamos 
                        
                        foreach (AgentNPC otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();

                            // añadido
                            if (!leaderFollowing){
                                foreach(BuscaCaminos bC in buscadores)
                                {

                                    if(bC.pl.Equals(otherAgent))
                                    {

                                        bC.pl.setStatus(Agent.NOSELECTED);
                                        bC.pl.quitarMarcador();

                                    }
                        
                                }

                            }

                            // fin del añadido
                        }
                        selectedNPCs.Clear();

                        if (!leaderFollowing){

                            foreach(BuscaCaminos bC in buscadores){

                                if(bC.pl.Equals(selectAgent))
                                {

                                    bC.pl.setStatus(Agent.SELECTED);
                                    bC.pl.activarMarcador();
                                    selectedNPCs.Add(bC.pl);
                                    
                                }
                            }
                        }

                        
                    }

                    else if (!leaderFollowing)
                    {
                        
                        foreach(BuscaCaminos bC in buscadores){

                            if(bC.pl.Equals(selectAgent)){
                                bC.pl.activarMarcador();
                                selectedNPCs.Add(bC.pl);
                            }
                        }

                    }
                    
                    if (leaderFollowing)
                    {
                        selectedNPCs.Add(selectAgent);
                        selectAgent.activarMarcador();
                    }


                } 
                else if (leaderFollowing)
                {
                    foreach (Agent otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();
                        }
                    selectedNPCs.Clear();
                }
            }


        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            selectedNPCs[0].isLider = true;
            if (getFormacion(lider) != null)
                getFormacion(lider).removeCharacter(lider);

            string nombre = "";
            for (int i=1;i<selectedNPCs.Count;i++)
                nombre = nombre + "_" + selectedNPCs[i]; 

            objForm = new GameObject("Formacion1_" + lider + nombre);
            formacion = objForm.AddComponent<FormationManager>();
            formacion.setComportamiento(leaderFollowing);
            formacion.lider = lider;
            lider.inFormacion = true;
            lider.setQuitarMov(true);

            formacion.slotAssignments = new List<SlotAssignment>();

            objUno = new GameObject("Uno_" + lider + nombre);
            uno = objUno.AddComponent<Formacion1>();
            formacion.pattern = uno;
            
            for (int i=1;i<selectedNPCs.Count;i++)
            {
                if (selectedNPCs[i] != lider){
                    // Si el agente ya estaba en una formación, se elimina de dicha formación
                    if (getFormacion(selectedNPCs[i]) != null)
                    {   
                        getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);
                    }

                    // Se añade el agente a la formación
                    if (formacion.addCharacter(selectedNPCs[i]))
                        selectedNPCs[i].inFormacion = true;
                    
                }
            }
            formacion.updateSlots();
            formaciones.Add(formacion);

            if (!leaderFollowing)
            {
                ActualizaFormacion(formacion);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){

            GameObject objForm;
            FormationManager formacion;

            Agent lider = selectedNPCs[0];
            if (getFormacion(lider) != null)
                getFormacion(lider).removeCharacter(lider);

            string nombre = "";
            for (int i=1;i<selectedNPCs.Count;i++)
                nombre = nombre + "_" + selectedNPCs[i]; 

            objForm = new GameObject("Formacion2_" + lider + nombre);
            formacion = objForm.AddComponent<FormationManager>();
            formacion.setComportamiento(leaderFollowing);
            formacion.lider = lider;
            lider.inFormacion = true;

            formacion.slotAssignments = new List<SlotAssignment>();

            objDos = new GameObject("Dos_" + lider + nombre);
            dos = objDos.AddComponent<Formacion2>();
            formacion.pattern = dos;

            for (int i=1;i<selectedNPCs.Count;i++)
            {
                if (selectedNPCs[i] != lider){
                    // Si el agente ya estaba en una formación, se elimina de dicha formación
                    if (getFormacion(selectedNPCs[i]) != null)
                    {   
                        getFormacion(selectedNPCs[i]).removeCharacter(selectedNPCs[i]);
                    }

                    // Se añade el agente a la formación
                    if (formacion.addCharacter(selectedNPCs[i]))
                        selectedNPCs[i].inFormacion = true;
                    
                }
            }
            formacion.updateSlots();
            formaciones.Add(formacion);

            if (!leaderFollowing)
            {
                ActualizaFormacion(formacion);
            }

        }
 
    }
}