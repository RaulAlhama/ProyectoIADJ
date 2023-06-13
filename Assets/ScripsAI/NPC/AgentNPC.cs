using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public const int ARQUERO = 0;
    public const int PESADA = 1;
    public const int EXPLORADOR = 2;
    public const int PATRULLA = 3;
    public Steering steer;
    public SteeringBehaviour[] listSteerings;
    public SteeringBehaviour[] steeringsIniciales;
    public SteeringBehaviour[] steeringsTargets;
    private BlendedSteering arbitro;
    public bool liderFollowing = true;
    public GameObject indicadorPrefab = null;
    //private GameObject indicador = null;
    public Agent virtualTarget;
    private GameObject objVirtual;
    private bool firstTime;
    private float tiempo; //Tiempo a esperar para activar el wander en en lider
    private bool objetivo=true;
    private int tipo;


    public bool getLLegada(){

        return objetivo;
    }
    public void setLLegada(bool valor){

        objetivo = valor;
    }

    public void setTipo(int t){

        tipo = t;
        if (tipo == ARQUERO){
            this.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
        else if (tipo == PESADA){
            this.GetComponentInChildren<Renderer>().material.color = Color.black;
        }
        else if (tipo == EXPLORADOR){
            this.GetComponentInChildren<Renderer>().material.color = Color.magenta;
        }
         else {
            this.GetComponentInChildren<Renderer>().material.color = Color.yellow;
        }

    }
    public int getTipo(){

        return tipo;
    }

    public void setColor(){
    
    }

    void Awake()
    {
        this.steer = new Steering();
        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Usa GetComponents<>()
        arbitro = new BlendedSteering(); //Inicializamos el árbitro
        listSteerings = GetComponents<SteeringBehaviour>(); //obtenemos los steerings
        steeringsIniciales = GetComponents<SteeringBehaviour>();
        
    }

    // Use this for initialization
    void Start()
    {
        Orientation = transform.eulerAngles.y;
        //setStatus(NPC);
        firstTime = true;
        tiempo = 150.0f;
        //Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Cuando decimos a una unidad que vaya a un lugar. Cuando pasa un tiempo recupera su movimiento inicial
        if(virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < 0.0001f && !inFormacion){
            //Debug.Log("Asignando movimientos iniciales");
            listSteerings = steeringsIniciales;
            setStatus(NPC);
        }

        
        //Llega al target seleccionado espera, si pasa X tiempo se añadirá wander
        if(isLider && virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < this.RadioInterior && !(tiempo <= 0)){
            tiempo -= Time.deltaTime;
            setStatus(ENFORMACION);
        }

        //Nada mas realizada la formación, esperamos un tiempo. Si al acabar este contador no se ha seleccionado ningún lugar
        if(quitarMovimiento && inFormacion && !(tiempo <= 0)){
            tiempo -= Time.deltaTime;
            setStatus(ENFORMACION);
        }

        //Le asignamos el wander ya que ha pasado X tiempo
        if(tiempo <= 0 && inFormacion){
            quitarMovimiento = false;
            if(this.gameObject.GetComponent<Wander>() == null){
                this.gameObject.AddComponent<Wander>();
            }
            //setStatus(MOVING);
            listSteerings = GetComponents<SteeringBehaviour>();
            //tiempo = 10.0f;
        }


        // Solo entra si es lider
        if (tiempo <= 0f)
        {
            tiempo = 10f;
            listSteerings = steeringsIniciales;
            setStatus(MOVING);
        }


        this.ApplySteering();
        this.ApplyTerreno();
        //listSteerings = GetComponents<SteeringBehaviour>();
        
    }
    


    private void ApplySteering()
    {
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        // La actualización de las propiedades se puede hacer en LateUpdate()
        Velocity += this.steer.linear * Time.deltaTime;
        //Debug.Log("Movimiento angular: " + steer.angular);
        Rotation += this.steer.angular * Time.deltaTime;
        Position += Velocity * ApplyTerreno() * Time.deltaTime;
        Orientation += Rotation * Time.deltaTime;

        // Aplicar las actualizaciones a la componente Transform
        transform.rotation = new Quaternion(); //Quaternion.identity;
        transform.Rotate(Vector3.up, Orientation);
    }


    private float ApplyTerreno()
    {

        // Tracemos un rayo hacia abajo desde la posición del objeto
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up, Vector3.down, out hit, 2f))
        {
            // Si el rayo colisiona con un objeto en la capa "groundLayerMask", podemos determinar el tipo de suelo
        
            switch(hit.collider.gameObject.tag) 
            {
                case "Cesped":
                    return 1.5f;
                case "Tierra":
                    return 0.75f;
                default:
                    return 1f;
            }

        }

        return 1f;

    }


    
    public override void setTarget(Agent virtualTargetPrefab){
        if(virtualTarget == null){
            objVirtual = new GameObject("NPCVirtual");
            virtualTarget = objVirtual.AddComponent<Agent>();
        }
        setStatus(MOVING);
        quitarMovimiento = false;
        tiempo = 10.0f;
        Velocity = Vector3.zero;
        //Rotation = 0;     
        virtualTarget.Position = virtualTargetPrefab.GetComponent<Agent>().Position;      
        virtualTarget.Orientation = Bodi.PositionToAngle(virtualTarget.Position - this.Position);
        if(firstTime){
            foreach (SteeringBehaviour behavior in listSteerings) //Eliminamos Steering del NPC
                DestroyImmediate(gameObject.GetComponent<SteeringBehaviour>());

            //DestroyImmediate(gameObject.GetComponent<Wander>());
            
            this.gameObject.AddComponent<Align>();
            this.gameObject.GetComponent<Align>().target = virtualTarget;
            this.gameObject.GetComponent<Align>().weight = 0.5f;

            this.gameObject.AddComponent<Arrive>();
            this.gameObject.GetComponent<Arrive>().target = virtualTarget;
            this.gameObject.GetComponent<Arrive>().weight = 0.5f;
            firstTime = false;
            steeringsTargets = GetComponents<SteeringBehaviour>();
            foreach (SteeringBehaviour behavior in steeringsTargets) //Eliminamos Steering del NPC
                DestroyImmediate(gameObject.GetComponent<SteeringBehaviour>());
        } else{
            steeringsTargets[0].target = virtualTarget;
            steeringsTargets[1].target = virtualTarget;
        }
        listSteerings = steeringsTargets;
    }




    public override void activarMarcador(){
        
        indicadorPrefab.SetActive(true);
        setStatus(SELECTED);
        select = true;
    }




    public override void quitarMarcador(){
        
        indicadorPrefab.SetActive(false);
        setStatus(NOSELECTED);
        select = false;
        
    }

    public virtual void LateUpdate() //Se ejecuta después de Update
    {
        if(!quitarMovimiento){
            this.steer = new Steering();
            this.steer = arbitro.GetSteering(this,listSteerings);
            //Debug.Log("Aplicando árbitro: " + "Movimiento Lineal: " + this.steer.linear + "Movimiento angular: " + this.steer.angular);
            // Reseteamos el steering final.
            /*this.steer = new Steering();
        //Si únicamente se aplica un movimiento, no aplicamos árbitro
            if(listSteerings.Length < 2){
                this.steer = listSteerings[0].GetSteering(this);
                Debug.Log("Usando un único steering");
            } else if (listSteerings.Length > 1) {
            //Debug.Log("Aplicando árbitro");
                this.steer = arbitro.GetSteering(this,listSteerings);
                Debug.Log("Aplicando árbitro: " + "Movimiento Lineal: " + this.steer.linear + "Movimiento angular: " + this.steer.angular);
            } else{

            } */
        } else{
            Velocity = Vector3.zero;
            Rotation = 0;
        }
        
            

        // Recorremos cada steering
        //foreach (SteeringBehaviour behavior in listSteerings)
        //    GetSteering(behavior);
        

    }

    

    
    /*void OnMouseDown(){
        
        if(!select){

            select = true;
            indicadorPrefab.SetActive(true);
            if(status == STOPPED || status == NOSELECTED)
                setStatus(SELECTED);
        }else{
            if(status == SELECTED || status == STOPPED)
                setStatus(NOSELECTED);
            select = false;
            indicadorPrefab.SetActive(false);
        }
    }

    /*private void GetSteering(SteeringBehaviour behavior)
    {
        // Calcula el steeringbehaviour
        Steering kinematic = behavior.GetSteering(this);

        // Usar kinematic con el árbitro desesado para combinar todos los SteeringBehaviour.
        // Llamaremos kinematicFinal a la aceleraciones finales.
        Steering kinematicFinal = kinematic; // Si solo hay un SteeringBehaviour

        // El resultado final se guarda para ser aplicado en el siguiente frame.
        this.steer = kinematicFinal;
    }*/
}