using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordenada
{
    // Start is called before the first frame update
    private int x;
    private int y;

    public Coordenada(int a, int b){

        x = a;
        y = b;
    }

    public int getX(){

        return x;
    }
    public int getY(){

        return y;
    }
    public void setNueva(int xn,int yn){

        x = xn;
        y = yn;
    }
    
}
