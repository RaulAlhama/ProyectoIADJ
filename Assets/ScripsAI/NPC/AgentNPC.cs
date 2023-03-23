using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public Steering steer;
    public SteeringBehaviour[] listSteerings;
    private BlendedSteering arbitro;

    public GameObject indicadorPrefab = null;
    private GameObject indicador = null;
    public Agent virtualTarget = null;
    private GameObject objVirtual;

    void Awake()
    {
        //this.steer = new Steering();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Usa GetComponents<>()
        arbitro = new BlendedSteering(); //Inicializamos el árbitro
        listSteerings = GetComponents<SteeringBehaviour>(); //obtenemos los steerings
        
    }

    // Use this for initialization
    void Start()
    {
        Orientation = transform.eulerAngles.y;
        //Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        this.ApplySteering();
        this.ApplyTerreno();
        listSteerings = GetComponents<SteeringBehaviour>();

        if (Input.GetKeyDown(KeyCode.H))
            modoDebug = !modoDebug;
    }
    


    private void ApplySteering()
    {
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        // La actualización de las propiedades se puede hacer en LateUpdate()
        Velocity += this.steer.linear * Time.deltaTime;
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
                    break;
                case "Hielo":
                    return 2f;
                    break;
                case "Tierra":
                    return 0.5f;
                    break;
                default:
                    return 1f;
                    break;
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
        if(this.gameObject.GetComponent<Arrive>() == null){
            this.gameObject.AddComponent<Arrive>();
            //Destroy(this.gameObject.GetComponent<Wander>());
        }
        if(virtualTarget != null){
            Destroy(virtualTarget.gameObject);
        }
        Debug.Log("Asignado Target");
        objVirtual = new GameObject("NPCVirtual");
        virtualTarget = objVirtual.AddComponent<Agent>();
        virtualTarget.Position = virtualTargetPrefab.GetComponent<Agent>().Position;
        this.gameObject.GetComponent<Arrive>().target = virtualTarget;
        this.gameObject.GetComponent<Arrive>().weight = 0.5f;
       
        virtualTarget.Orientation = Bodi.PositionToAngle(virtualTarget.Position - this.Position);

        if(this.gameObject.GetComponent<Align>() == null){
            this.gameObject.AddComponent<Align>();
            //Destroy(this.gameObject.GetComponent<Wander>());
        }    
        this.gameObject.GetComponent<Align>().target = virtualTarget;
        this.gameObject.GetComponent<Align>().weight = 0.5f;
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

    void OnDrawGizmos()
    {
                     
        if (modoDebug){

            Vector3 from = transform.position; // Origen de la línea
            Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

            from = from + elevation;

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(from, Velocity);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(from, Acceleration);

            float distanciaBigotesExteriores = this.AnguloExterior/numBigotes;
            float distanciaBigotesInteriores = this.AnguloInterior/numBigotes; 
            
            for (int i=0;i<numBigotes;i++){

                // Mirando en la dirección de la orientación.
                Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;

                Gizmos.color = Color.red;  
                Vector3 vectorInterior1 = new Vector3 (Mathf.Cos((AnguloInterior-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((this.AnguloInterior-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((this.AnguloInterior-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((this.AnguloInterior-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z); 
                Vector3 vectorInterior2 = new Vector3 (Mathf.Cos((-AnguloInterior+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((-this.AnguloInterior+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((-this.AnguloInterior+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((-this.AnguloInterior+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z); 

                Gizmos.DrawRay(from, vectorInterior1);
                Gizmos.DrawRay(from, vectorInterior2);

                // Dibujamos el angulo exterior
                Vector3 vectorExterior3 = new Vector3 (Mathf.Cos((this.AnguloExterior-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((this.AnguloExterior-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((this.AnguloExterior-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((this.AnguloExterior-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z); 
                Vector3 vectorExterior4 = new Vector3 (Mathf.Cos((-this.AnguloExterior+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((-this.AnguloExterior+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((-this.AnguloExterior+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((-this.AnguloExterior+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z); 

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