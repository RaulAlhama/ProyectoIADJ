using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public Steering steer;
    public SteeringBehaviour[] listSteerings;
    public SteeringBehaviour[] steeringsIniciales;
    public SteeringBehaviour[] steeringsTargets;
    private BlendedSteering arbitro;

    public GameObject indicadorPrefab = null;
    private GameObject indicador = null;
    public Agent virtualTarget = null;
    private GameObject objVirtual;
    private bool firstTime;
    private float tiempo; //Tiempo a esperar para activar el wander en en lider

    public enum TIPO_NPC
    {
        LANCERO,
        ARQUERO,
        MAGO,
        JINETE
    }

    [SerializeField]
    protected internal TIPO_NPC tipo;

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
        setStatus(NPC);
        firstTime = true;
        tiempo = 150.0f;
        //Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Cuando decimos a una unidad que vaya a un lugar. Cuando pasa un tiempo recupera su movimiento inicial
        if(virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < 0.0001f && !inFormacion){
            Debug.Log("Asignando movimientos iniciales");
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


        this.ApplySteering();
        this.ApplyTerreno();
        //listSteerings = GetComponents<SteeringBehaviour>();

        if (Input.GetKeyDown(KeyCode.H))
            modoDebug = !modoDebug;
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
                    return 1f;
                case "Hielo":
                    return 2f;
                case "Tierra":
                    return 0.5f;
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
        indicador = Instantiate(indicadorPrefab, transform);
        indicador.transform.localPosition = Vector3.up * 4;
        setStatus(SELECTED);
        select = true;
    }




    public override void quitarMarcador(){
        if (indicador != null){
            Destroy(indicador);
        }
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

    void OnDrawGizmos()
    {
                     
        if (modoDebug){

            Vector3 from = transform.position; // Origen de la línea
            Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

            from = from + elevation;

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(from, Velocity);

            float distanciaBigotesExteriores = this.AnguloExterior/numBigotes;
            float distanciaBigotesInteriores = this.AnguloInterior/numBigotes; 
            
            for (int i=0;i<numBigotes;i++){

                // Mirando en la dirección de la orientación.
                Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;

                Gizmos.color = Color.red;
                Vector3 vectorInterior1 = Bodi.VectorRotate(direction, AnguloInterior-distanciaBigotesInteriores*i);
                Vector3 vectorInterior2 = Bodi.VectorRotate(direction, -AnguloInterior+distanciaBigotesInteriores*i);  
                
                Gizmos.DrawRay(from, vectorInterior1);
                Gizmos.DrawRay(from, vectorInterior2);

                // Dibujamos el angulo exterior
                Vector3 vectorExterior3 = Bodi.VectorRotate(direction, AnguloExterior-distanciaBigotesExteriores*i);
                Vector3 vectorExterior4 = Bodi.VectorRotate(direction, -AnguloExterior+distanciaBigotesExteriores*i); 
                
                Gizmos.color = Color.blue; 
                Gizmos.DrawRay(from, vectorExterior3);
                Gizmos.DrawRay(from, vectorExterior4);

            }

            // Dibujamos el circulo interior
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Position, RadioInterior);

            // Dibujamos el circulo exterior
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Position, RadioExterior);

            Gizmos.color = Color.black;
            Collider[] colliders = FindObjectsOfType<Collider>();

            foreach (Collider collider in colliders)
            {
                Gizmos.DrawWireCube(collider.transform.position, collider.bounds.size);
            }


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