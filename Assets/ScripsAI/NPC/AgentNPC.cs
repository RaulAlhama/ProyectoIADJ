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
    private bool objetivo=false;

    public enum TIPO_NPC
    {
        LANCERO,
        ARQUERO,
        MAGO,
        JINETE
    }

    public bool getLLegada(){

        return objetivo;
    }
    public void setLLegada(bool valor){

        objetivo = valor;
    }

    [SerializeField]
    protected internal TIPO_NPC tipo;

    void Awake()
    {
        //this.steer = new Steering();

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
        //Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < 0.01f && !inFormacion){
            listSteerings = steeringsIniciales;
            setStatus(NPC);
        }

        if(virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < 0.01f && inFormacion && isLider){
            tiempo -= Time.deltaTime;
        }

        if(virtualTarget != null && (virtualTarget.Position - this.Position).magnitude < 0.01f && inFormacion)
            setStatus(ENFORMACION);

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


    
    public override Agent getTarget()
    {
        if (this.GetComponent<SteeringBehaviour>() != null){
            return this.GetComponent<SteeringBehaviour>().target;
        }

        return null;

    }



    public override void setTarget(Agent virtualTargetPrefab){
        if(virtualTarget != null){
            Destroy(virtualTarget.gameObject);
        }
        setStatus(MOVING);
        objVirtual = new GameObject("NPCVirtual");
        virtualTarget = objVirtual.AddComponent<Agent>();
        virtualTarget.Position = virtualTargetPrefab.GetComponent<Agent>().Position;      
        virtualTarget.Orientation = Bodi.PositionToAngle(virtualTarget.Position - this.Position);
        if(firstTime){
            foreach (SteeringBehaviour behavior in listSteerings) //Eliminamos Steering del NPC
                DestroyImmediate(gameObject.GetComponent<SteeringBehaviour>());
        
            //DestroyImmediate(gameObject.GetComponent<Wander>());
            

            this.gameObject.AddComponent<Arrive>();
            this.gameObject.GetComponent<Arrive>().target = virtualTarget;
            this.gameObject.GetComponent<Arrive>().weight = 0.5f;

            this.gameObject.AddComponent<Align>();
            this.gameObject.GetComponent<Align>().target = virtualTarget;
            this.gameObject.GetComponent<Align>().weight = 0.5f;
            firstTime = false;
        } else{
            this.gameObject.GetComponent<Arrive>().target = virtualTarget;
            this.gameObject.GetComponent<Align>().target = virtualTarget;
        }
        steeringsTargets = GetComponents<SteeringBehaviour>();
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

        // Reseteamos el steering final.
        this.steer = new Steering();
        //Si únicamente se aplica un movimiento, no aplicamos árbitro
        if(listSteerings.Length == 1){
            this.steer = listSteerings[0].GetSteering(this);
        } else{
            //Debug.Log("Aplicando árbitro");
            this.steer = arbitro.GetSteering(this,listSteerings);
        }

       

        // Recorremos cada steering
        //foreach (SteeringBehaviour behavior in listSteerings)
        //    GetSteering(behavior);
        

    }

    

    
    void OnMouseDown(){
        
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