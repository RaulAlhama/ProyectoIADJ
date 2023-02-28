using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[AddComponentMenu("Steering/InteractiveObject/Agent")]
public class Agent : Bodi
{

    /*protected virtual void Awake(){

   }*/

   public bool drawBigotes = false;
   public bool drawSpheres = false;
   public int numBigotes = 1;
   
    
    [Tooltip("Radio interior de la IA")]
    [SerializeField] protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")]
    [SerializeField] protected float _arrivalRadius = 3f;

    [Tooltip("Ángulo interior de la IA")]
    [SerializeField] protected float _interiorAngle = 3.0f; // ángulo sexagesimal.

    [Tooltip("Ángulo exterior de la IA")]
    [SerializeField] protected float _exteriorAngle = 8.0f; // ángulo sexagesimal.


    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.
    public float RadioInterior
    {
        get {return _interiorRadius;}
        set {_interiorRadius = value;}
    }
    
    public float RadioExterior
    {
        get {return _arrivalRadius;}
        set {_arrivalRadius = value;}
    }

    public float AnguloInterior
    {
        get {return _interiorAngle;}
        set {_interiorAngle = value;}
    }

    public float AnguloExterior
    {
        get {return _exteriorAngle;}
        set {_exteriorAngle = value;}
    }
    // AÑADIR MÉTODS FÁBRICA, SI LO VES NECESARIO.
    // En algún momento te puede interesar crear Agentes con tengan una posición
    // y unos radios: por ejemplo, crar un punto de llegada para un auténtico
    // Agente Inteligente. Este punto de llegada no tienen que ser inteligente,
    // solo tienen que ser "sensible" - si fuera necesario - a que lo tocan.
    // Planteate la posibilidad de crear aquí métodos fábrica (estáticos) para
    // crear esos agentes. Para ello crea un GameObject y usa:
    // .AddComponent<BoxCollider>();
    // .GetComponent<Collider>().isTrigger = true;
    // .AddComponent<Agent>();
    // Establece los valores del Bodi y radios/ángulos a los valores adecuados.
    // Esta es solo una de las muchas posiblidades para resolver este problema.



    // AÑADIR LO NECESARIO PARA MOSTRAR LA DEPURACIÓN. Te puede interesar los siguientes enlaces.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
    // https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html
    // https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireSphere.html
    // https://docs.unity3d.com/ScriptReference/Gizmos-color.html

    void OnDrawGizmos()
    {
        float distanciaBigotesExteriores = _exteriorAngle/numBigotes;
        float distanciaBigotesInteriores = _interiorAngle/numBigotes;

        if (drawBigotes)
        {   
            
           for (int i=0;i<numBigotes;i++){

                // Dibujamos el angulo interior
                Vector3 from = transform.position; // Origen de la línea
                Vector3 elevation = new Vector3(0, 1, 0); // Elevación para no tocar el suelo

                from = from + elevation;

                // Mirando en la dirección de la orientación.
                Gizmos.color = Color.green;  
                Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
                Gizmos.DrawRay(from, direction);

                Gizmos.color = Color.red;  
                Vector3 vectorInterior1 = new Vector3 (Mathf.Cos((_interiorAngle-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((_interiorAngle-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((_interiorAngle-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((_interiorAngle-distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z); 
                Vector3 vectorInterior2 = new Vector3 (Mathf.Cos((-_interiorAngle+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((-_interiorAngle+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((-_interiorAngle+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((-_interiorAngle+distanciaBigotesInteriores*i) * Mathf.Deg2Rad) * direction.z); 

                Gizmos.DrawRay(from, vectorInterior1);
                Gizmos.DrawRay(from, vectorInterior2);

                // Dibujamos el angulo exterior
                Vector3 vectorExterior3 = new Vector3 (Mathf.Cos((_exteriorAngle-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((_exteriorAngle-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((_exteriorAngle-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((_exteriorAngle-distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z); 
                Vector3 vectorExterior4 = new Vector3 (Mathf.Cos((-_exteriorAngle+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Sin((-_exteriorAngle+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z,direction.y,-Mathf.Sin((-_exteriorAngle+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.x + Mathf.Cos((-_exteriorAngle+distanciaBigotesExteriores*i) * Mathf.Deg2Rad) * direction.z); 

                Gizmos.color = Color.blue; 
                Gizmos.DrawRay(from, vectorExterior3);
                Gizmos.DrawRay(from, vectorExterior4);

            }

        }

        if (drawSpheres)
        {
            // Dibujamos el circulo interior
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Position, _interiorRadius);

            // Dibujamos el circulo exterior
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Position, _arrivalRadius);
        }
    }


}
