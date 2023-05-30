using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Ruta : MonoBehaviour
{
    private const int IZQUIERDA = 1;
    private const int DERECHA = 2;
    private WayPoint[] caminoIzq;
    private WayPoint[] caminoDer;
    private int actual;
    private int sentido;
    private bool cambio = false;
    public Ruta(WayPoint[] a,WayPoint[] b){

        caminoIzq = a;
        caminoDer = b;
        actual = -1;
        int ran = UnityEngine.Random.Range(0,1);

        if(ran == 0)
            sentido = IZQUIERDA;
        else
            sentido = DERECHA;
    }

    public WayPoint getSiguiente(){

        actual++;
        switch (sentido)
        {
            case DERECHA:{
                
                if(actual == caminoDer.Length && !cambio){

                    actual = 0;
                    Array.Reverse(caminoDer);
                    cambio = true;
                    return caminoDer[actual]; 
                }else if(actual == caminoDer.Length && cambio){

                    sentido = IZQUIERDA;
                    actual = 0;
                    cambio = false;
                    Array.Reverse(caminoDer);
                    return caminoIzq[actual];
                }else{

                    return caminoDer[actual];
                }
                break;
            }
            case IZQUIERDA:{
                
                if(actual == caminoIzq.Length && !cambio){

                    actual = 0;
                    Array.Reverse(caminoIzq);
                    cambio = true;
                    return caminoIzq[actual]; 
                }else if(actual == caminoIzq.Length && cambio){

                    sentido = DERECHA;
                    actual = 0;
                    cambio = false;
                    Array.Reverse(caminoIzq);
                    return caminoDer[actual];
                }else{

                    return caminoIzq[actual];
                }
                break;
            
            }
            default:
                return null;
        }
    }
}
