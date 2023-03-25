using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class controladorMdFormaciones : MonoBehaviour
{
    // Start is called before the first frame update
   /* public GridFinal mundo;
    public Agent npcVirtual;
    public AgentNPC player;
    public GameObject puntero;
    private Vector3 targetPosition; // posición donde haremos click
    public Agent npcVirtualPrefab; // NPC virtual pasado por parámetro
    public GameObject punteroPrefab; // Puntero (en nuestro caso una esfera) pasado por parámetro
    //private Object copiaAgent;
    private GameObject copiaPuntero;
    private int iObjetivo;
    private int jObjetivo;
    private double[,] grafoMovimiento;
    private GameObject[] obstaculos;
    private const double infinito = Double.PositiveInfinity;
    public Node aNodo;

    private List<BuscaCaminos> buscadores;

    public List<FormationManager> formaciones;
    public List<Agent> selectedNPCs;
    private GameObject obj;

    private GameObject objUno;
    private Formacion1 uno;
    private GameObject objDos;
    private Formacion2 dos;

    public bool liderFollowing=true;

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
    }
    private int tam=2;


    public void comprobarFormacion(){

        foreach (FormationManager formacion in formaciones.ToList()){

            if (formacion.slotAssignments.Count == 0){
                formaciones.Remove(formacion);
                Destroy(formacion.pattern.gameObject);   
                Destroy(formacion.gameObject);
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
        
        mundo.setObstaculos(obstaculos);
        
        agregaPlayers(player);
        agregaPlayers(player2);
        agregaPlayers(player3);
        agregaPlayers(player4);
        agregaPlayers(player5);
        agregaPlayers(player6);
        
    }
    private void agregaPlayers(AgentNPC pl){

        mundo.setValor(pl.Position,GridFinal.PLAYER);
        PathFindingLRTA buscador = new PathFindingLRTA(mundo,pl);
        BuscaCaminos bc = new BuscaCaminos();
        bc.pl = pl;
        bc.buscador = buscador;
        bc.npcVirtual = Instantiate(npcVirtual);
        buscadores.Add(bc);
    }
    public FormationManager getFormacion(Agent agent){
        foreach (FormationManager formacion in formaciones){
            foreach (SlotAssignment slot in formacion.slotAssignments){
                if (slot.character == agent){

                    return formacion;
                }
                    
            }
        }
        return null;
    }
    private Vector3 restauraFormacion(AgentNPC agent){

        foreach (FormationManager formacion in formaciones){
            //ActualizaFormacion(formacion);
            foreach (SlotAssignment slot in formacion.slotAssignments){
                if (slot.character == agent){

                    return formacion.pattern.getSlotLocation(slot.slotNumber).position;
                }
                    
            }
            
        }
        return Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        // Comprobamos si los agentes tienen que volver a formar
        comprobarFormacion();

        if (Input.GetMouseButtonDown(1) && selectedNPCs.Count > 0)
        {   
            
            bool romperFormacion = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){

                targetPosition = hit.point; //Nos quedamos con la posfción donde hemos hecho click
                targetPosition.y = 0;
                npcVirtualPrefab.Position = targetPosition; //Asignamos esa posición al target Virtual 
                punteroPrefab.transform.position = targetPosition; //Se la asignamos también al puntero
                puntero = Instantiate(punteroPrefab);// creamos el puntero
                Destroy(puntero, 0.5f);
                // Mueve los NPC's seleccionados al destino
                
                if (!liderFollowing){

                    foreach (AgentNPC agent in selectedNPCs)
                    {   
                        foreach(BuscaCaminos bC in buscadores){

                            if(bC.pl.Equals(agent)){

                                int iObjetivo;
                                int jObjetivo;

                                mundo.getCoordenadas(targetPosition,out iObjetivo,out jObjetivo);

                                if(mundo.Posible(iObjetivo,jObjetivo)){
                                    Debug.Log(mundo.getPosicionReal(iObjetivo,jObjetivo));
                                    if(bC.pl.inFormacion){

                                        bC.npcVirtual.transform.position = (mundo.getPosicionReal(iObjetivo,jObjetivo) + new Vector3(tam/2,0,tam/2)) + restauraFormacion(bC.pl);
                                        mundo.getCoordenadas(bC.npcVirtual.transform.position,out iObjetivo,out jObjetivo);

                                    }else{

                                        bC.npcVirtual.transform.position = targetPosition;
                                    }
                                    
                                    
                                    bC.buscador.setObjetivos(iObjetivo,jObjetivo, bC.npcVirtual);
                                    bC.buscador.setPosicionNpcVirtual(mundo.getPosicionReal(iObjetivo,jObjetivo));

                                    puntero.transform.position = mundo.getPosicionReal(iObjetivo,jObjetivo);

                                    bC.buscador.setGrafoMovimiento(mundo.getGrafo(iObjetivo,jObjetivo));
                                    copiaPuntero = (GameObject) Instantiate(puntero);
                            
                                    bC.buscador.LRTAestrella();
                                }
                            }
                        }
                    }
                }

                else {

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

            }



        }
        else {

            foreach (AgentNPC agent in selectedNPCs){

                foreach(BuscaCaminos bC in buscadores){

                        if(bC.pl.Equals(agent) && (bC.pl.status == Agent.STOPPED)){
                            
                            
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
                    Agent selectAgent;
                    if(npcObject.GetComponent<AgentPlayer>() != null)
                        selectAgent = npcObject.GetComponent<AgentPlayer>();
                    else
                        selectAgent = npcObject.GetComponent<AgentNPC>();
                    

                    if (!liderFollowing){
                        foreach (BuscaCaminos bC in buscadores){

                            if(bC.pl.status != Agent.NOSELECTED && bC.pl.Equals(selectAgent)){
                                
                                if(!selectedNPCs.Contains(bC.pl))
                                    selectedNPCs.Add(bC.pl);
                                else
                                    selectedNPCs.Remove(bC.pl);
                            }else if(bC.pl.Equals(selectAgent)){
                                
                                if(selectedNPCs.Contains(bC.pl))
                                    selectedNPCs.Remove(bC.pl);
                            }
                        }
                    }

                    else{
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
                
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject npcObject = hit.collider.gameObject;
                Agent selectAgent;

                if (npcObject.CompareTag("NPC"))
                {
                    if(npcObject.GetComponent<AgentNPC>() != null)
                        selectAgent = npcObject.GetComponent<AgentNPC>();
                    else
                        selectAgent = npcObject.GetComponent<AgentNPC>();
                    if (selectedNPCs.Count > 0){ //Si hay varios personajes seleccionados, los deseleccionamos 
                        foreach (Agent otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();
                            foreach(BuscaCaminos bC in buscadores){

                                if(bC.pl.Equals(otherAgent)){

                                    bC.pl.setStatus(Agent.NOSELECTED);

                                }
                        
                            }

                        }
                        selectedNPCs.Clear();

                        foreach(BuscaCaminos bC in buscadores){

                            if(bC.pl.Equals(selectAgent)){

                                bC.pl.setStatus(Agent.SELECTED);
                                selectedNPCs.Add(bC.pl);
                            }
                        }
                    }

                    foreach(BuscaCaminos bC in buscadores){

                        if(bC.pl.Equals(selectAgent)){

                            selectedNPCs.Add(bC.pl);
                        }
                    }
                    
                    selectedNPCs.Add(selectAgent);
                    selectAgent.activarMarcador();
                } 
                else{
                    foreach (Agent otherAgent in selectedNPCs)
                        {
                            otherAgent.quitarMarcador();
                        }
                    selectedNPCs.Clear();
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){

            Formar();

        }
        
    }


    private void Formar(){

        GameObject objForm;
        FormationManager formacion;

        Agent lider = selectedNPCs[0];
        if (getFormacion(lider) != null)
            getFormacion(lider).removeCharacter(lider);

        string nombre = "";
        for (int i=1;i<selectedNPCs.Count;i++)
            nombre = nombre + "_" + selectedNPCs[i]; 

        objForm = new GameObject("Formacion1_" + lider + nombre);
        formacion = objForm.AddComponent<FormationManager>();
        formacion.lider = lider;
        lider.inFormacion = true;

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

        formaciones.Add(formacion);
        ActualizaFormacion(formacion);

    }

    private void ActualizaFormacion(FormationManager formacion){

        foreach(SlotAssignment slt in formacion.slotAssignments){

                foreach(BuscaCaminos bC in buscadores){

                    if(bC.pl.Equals(slt.character)){

                        bC.pl.inFormacion = true;
                        slt.character.inFormacion = true;
                        int iObjetivo;
                        int jObjetivo;
                        
                        bC.npcVirtual.transform.position = slt.target;
                        mundo.getCoordenadas(slt.target,out iObjetivo,out jObjetivo);

                        bC.buscador.setObjetivos(iObjetivo,jObjetivo, bC.npcVirtual);
                        bC.buscador.setPosicionNpcVirtual(mundo.getPosicionReal(iObjetivo,jObjetivo));
                        bC.buscador.setGrafoMovimiento(mundo.getGrafo(iObjetivo,jObjetivo));
                        bC.buscador.LRTAestrella();
                        bC.pl.setStatus(Agent.STOPPED);
                        slt.character.setStatus(Agent.STOPPED);
                    }
                }
            }
    }
    */
}
