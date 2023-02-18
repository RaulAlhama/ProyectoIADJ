using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public Steering steer;
    public SteeringBehaviour[] listSteerings;
    private Quaternion rotacionIni;

    void Awake()
    {
        this.steer = new Steering();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Usa GetComponents<>()
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



    public virtual void LateUpdate()
    {
        // Reseteamos el steering final.
        this.steer = new Steering();

        // Recorremos cada steering
        foreach (SteeringBehaviour behavior in listSteerings)
            GetSteering(behavior);
    }




    private void GetSteering(SteeringBehaviour behavior)
    {
        // Calcula el steeringbehaviour
        Steering kinematic = behavior.GetSteering(this);

        // Usar kinematic con el árbitro desesado para combinar todos los SteeringBehaviour.
        // Llamaremos kinematicFinal a la aceleraciones finales.
        Steering kinematicFinal = kinematic; // Si solo hay un SteeringBehaviour

        // El resultado final se guarda para ser aplicado en el siguiente frame.
        this.steer = kinematicFinal;
    }
}