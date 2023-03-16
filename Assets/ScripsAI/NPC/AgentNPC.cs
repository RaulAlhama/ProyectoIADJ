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
    private Agent virtualTarget = null;


    void Awake()
    {
        this.steer = new Steering();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Usa GetComponents<>()
        arbitro = new BlendedSteering();
        listSteerings = GetComponents<SteeringBehaviour>();
        
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
        listSteerings = GetComponents<SteeringBehaviour>();
        
        if (Input.GetKeyDown(KeyCode.H)){

            Vector3 from = transform.position; // Origen de la línea
            Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

            from = from + elevation;

            Vector3 velocity = from + Velocity;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(from, velocity);
              
        }

    }


    private void ApplySteering()
    {
        // Actualizar las propiedades para Time.deltaTime según NewtonEuler
        // La actualización de las propiedades se puede hacer en LateUpdate()
        Velocity += this.steer.linear * Time.deltaTime;
        Rotation += this.steer.angular * Time.deltaTime;
        Position += Velocity * Time.deltaTime;
        Orientation += Rotation * Time.deltaTime;

        // Aplicar las actualizaciones a la componente Transform
        transform.rotation = new Quaternion(); //Quaternion.identity;
        transform.Rotate(Vector3.up, Orientation);
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
        virtualTarget = Instantiate(virtualTargetPrefab);
        this.gameObject.GetComponent<Arrive>().target = virtualTarget;
        this.gameObject.GetComponent<Arrive>().weight = 0.5f;
       
        virtualTargetPrefab.Orientation = Bodi.PositionToAngle(virtualTargetPrefab.Position - this.Position);

        if(this.gameObject.GetComponent<Align>() == null){
            this.gameObject.AddComponent<Align>();
            //Destroy(this.gameObject.GetComponent<Wander>());
        }    
        this.gameObject.GetComponent<Align>().target = virtualTargetPrefab;
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

    public virtual void LateUpdate()
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