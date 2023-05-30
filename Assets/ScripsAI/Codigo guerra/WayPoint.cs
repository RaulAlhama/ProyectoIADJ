using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    // Start is called before the first frame update
    private bool disponible;
    private Posicion slot;

    public WayPoint(bool dis,Posicion b){

        disponible = dis;
        slot = b;
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
}
