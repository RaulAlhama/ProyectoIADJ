using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    // Start is called before the first frame update
    private bool disponible;
    private Posicion slot;
    private string nombre;
    public const string TORRE_VIGIA= "torre vigia";
    public const string ARMERIA= "armeria";
    public const string PUENTE_DERECHO_AZUL = "puente derecho azul";
    public const string PUENTE_IZQUIERDO_AZUL = "puente izquierdo azul";
    public const string PUENTE_DERECHO_ROJO = "puente derecho rojo";
    public const string PUENTE_IZQUIERDO_ROJO = "puente izquierdo rojo";
    public const string CON_ARMERIA = "dis armeria";
    public const string SANTUARIO = "santuario";
    public const string ESCUDERIA = "escuderia";

    public WayPoint(bool dis,Posicion b,string a="none"){

        disponible = dis;
        slot = b;
        nombre = a;
    }
    public int getX(){

        return slot.getI();
    }
    public int getY(){

        return slot.getJ();
    }
    public bool getDisponible(){

        return disponible;
    }
    public void setDisponible(bool val){

        disponible = val;
    }
    public string getNombre(){

        return nombre;
    }
}
