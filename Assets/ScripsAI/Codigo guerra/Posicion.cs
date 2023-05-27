using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posicion
{
    // Start is called before the first frame update
    private int i;
    private int j;

    public Posicion(int a, int b){

        i = a;
        j = b;
    }

    public int getI(){

        return i;
    }
    public int getJ(){

        return j;
    }
    public void setNueva(int iN,int jN){

        i = iN;
        j = jN;
    }
}
