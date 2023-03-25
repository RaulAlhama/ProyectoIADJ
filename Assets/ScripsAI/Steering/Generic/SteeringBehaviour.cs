using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentNPC))]
[System.Serializable]
public class SteeringBehaviour : MonoBehaviour
{

    protected string nameSteering = "no steering";

    protected Vector3 explTargetDirection;
    protected float explTargetRotation;
    protected bool isExplicitTarget;



    public Agent target;
   [SerializeField] [Range(0.0f, 1.0f)] public float weight = 1; //peso del steering para el movimiento compuesto

    public string NameSteering
    {
        set { nameSteering = value; }
        get { return nameSteering; }
    }


    /// <summary>
    /// Cada SteerinBehaviour retornará un Steering=(vector, escalar)
    /// acorde a su propósito: llegar, huir, pasear, ...
    /// Sobreescribie siempre este método en todas las clases hijas.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public virtual Steering GetSteering(AgentNPC agent)
    {
        return null;
    }


    protected virtual void OnGUI()
    {
        // Para la depuración te puede interesar que se muestre el nombre
        // del steeringbehaviour sobre el personaje.
        // Te puede ser util Rect() y GUI.TextField()
        // https://docs.unity3d.com/ScriptReference/GUI.TextField.html
    }
}
