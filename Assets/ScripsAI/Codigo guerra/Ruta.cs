using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Ruta
{
    private const int IZQUIERDA = 1;
    private const int DERECHA = 2;
    private bool azul;
    private WayPoint[] caminoIzq;
    private WayPoint[] caminoDer;
    private WayPoint[] caminoIzqEnemigo;
    private WayPoint[] caminoDerEnemigo;
    private int actual;
    private int sentido;
    private bool cambio = false;
    private bool caminoEnemigo = false;
    public Ruta(WayPoint[] a,WayPoint[] b,WayPoint[] c,WayPoint[] d,bool equipo){

        caminoIzq = (WayPoint[])a.Clone();
        caminoDer = (WayPoint[])b.Clone();
        caminoIzqEnemigo = (WayPoint[])c.Clone();
        caminoDerEnemigo = (WayPoint[])d.Clone();
        
        actual = -1;
        int ran = UnityEngine.Random.Range(0,100);

        if(ran >= 50){

            sentido = IZQUIERDA;
            Array.Reverse(caminoDer);
            Array.Reverse(caminoDerEnemigo);
        } 
        else{

            sentido = DERECHA;
            Array.Reverse(caminoIzq);
            Array.Reverse(caminoIzqEnemigo);
        }
        azul = equipo;
    }

    public WayPoint getSiguiente(){

        actual++;
        if (caminoEnemigo)
        {
            switch (sentido)
            {
                case DERECHA:{
                    
                    if(actual == caminoIzqEnemigo.Length && !cambio){

                        int anterior = actual-1; 
                        actual = 0;
                        // puede ser que no se invierta
                        cambio = true;
                        if(caminoIzqEnemigo[actual].getDisponible() && caminoDer[anterior].getDisponible()){

                            caminoEnemigo = false;
                            return caminoDer[actual];
                        
                        }else{
                            
                            camnioDeSentido();
                            return caminoIzqEnemigo[actual];
                        }
                    }else if(actual == caminoIzqEnemigo.Length && cambio){

                        sentido = IZQUIERDA;
                        actual = 0;
                        cambio = false;
                        return caminoDerEnemigo[actual];
                    }else{

                        return caminoIzqEnemigo[actual];
                    }
                    
                }
                case IZQUIERDA:{
                    
                    if(actual == caminoDerEnemigo.Length && !cambio){

                        int anterior = actual-1; 
                        actual = 0;
                        cambio = true;
                        if(caminoDerEnemigo[actual].getDisponible() && caminoIzq[anterior].getDisponible()){

                            caminoEnemigo = false;
                            return caminoIzq[actual];

                        }else{
                            
                            camnioDeSentido();
                            return caminoDerEnemigo[actual];
                        }
                    }else if(actual == caminoDerEnemigo.Length && cambio){

                        sentido = DERECHA;
                        actual = 0;
                        cambio = false;
                        return caminoIzqEnemigo[actual];
                    }else{

                        return caminoDerEnemigo[actual];
                    }
                }
                default:
                    return null;
            }
        }else{

            switch (sentido)
            {
                case DERECHA:{
                    
                    if(actual == caminoDer.Length && !cambio){

                        int anterior = actual-1; 
                        actual = 0;
                        // puede ser que no se invierta
                        cambio = true;
                        if(caminoIzqEnemigo[actual].getDisponible() && caminoDer[anterior].getDisponible()){

                            caminoEnemigo = true;
                            return caminoIzqEnemigo[actual];

                        }else{
                            
                            camnioDeSentido();
                            return caminoDer[actual]; 
                        }
                        
                    }else if(actual == caminoDer.Length && cambio){

                        sentido = IZQUIERDA;
                        actual = 0;
                        cambio = false;
                        return caminoIzq[actual];
                    }else{

                        return caminoDer[actual];
                    }
                }
                case IZQUIERDA:{
                    
                    if(actual == caminoIzq.Length && !cambio){

                        int anterior = actual-1; 
                        actual = 0;
                        cambio = true;

                        if(caminoDerEnemigo[actual].getDisponible() && caminoIzq[anterior].getDisponible()){

                            caminoEnemigo = true;
                            return caminoDerEnemigo[actual];

                        }else{
                            
                            camnioDeSentido();
                            return caminoIzq[actual];
                        }

                    }else if(actual == caminoIzq.Length && cambio){

                        sentido = DERECHA;
                        actual = 0;
                        cambio = false;
                        return caminoDer[actual];
                    }else{

                        return caminoIzq[actual];
                    }
                
                }
                default:
                    return null;
            }
        }
        
    }
    private int WPAleatoreo(WayPoint[] wp){

        while(true){

            int i = UnityEngine.Random.Range(0, wp.Length-1);
            if (wp[i].getDisponible())
            {
                return i;
            }
        } 
    }
    public WayPoint getAleatoreo(){

        int st = UnityEngine.Random.Range(1, 2);
        if (st == 1)
        {
            if (caminoEnemigo)
            {
                int st2 = UnityEngine.Random.Range(1, 2);
                if (st2 == 1)
                {
                    int i = WPAleatoreo(caminoIzq);
                    return caminoIzq[i];
                }else{

                    int i = WPAleatoreo(caminoDerEnemigo);
                    return caminoDerEnemigo[i];
                    
                }
            }else{

                int i = WPAleatoreo(caminoIzq);
                return caminoIzq[i];
            }
            
        }else{

            if (caminoEnemigo)
            {
                int st2 = UnityEngine.Random.Range(1, 2);
                if (st2 == 1)
                {
                    int i = WPAleatoreo(caminoDer);
                    return caminoDer[i];
                }else{

                    int i = WPAleatoreo(caminoIzqEnemigo);
                    return caminoIzqEnemigo[i];
                }
            }else{

                int i = WPAleatoreo(caminoDer);
                return caminoDer[i];
            }
        }
    }
    
    private bool buscaDisponibilidad(WayPoint[] camino,string objetivo){

        foreach (WayPoint item in camino)
        {
            if (item.getNombre() == objetivo)
            {
                return item.getDisponible();
            }
        }
        return false;
    }
    public bool getDisponible(string obj){

        switch (sentido)
        {
            case IZQUIERDA:{

                if (buscaDisponibilidad(caminoDerEnemigo,obj))
                {
                    return true;
                }else if(buscaDisponibilidad(caminoIzq,obj)){

                    return true;
                }else{

                    return false;
                }
            }
            
            case DERECHA:{

                if (buscaDisponibilidad(caminoIzqEnemigo,obj))
                {
                    return true;
                }else if(buscaDisponibilidad(caminoDer,obj)){

                    return true;
                }else{

                    return false;
                }
            }
            
            default:
            return false;
        }
    }
    private void setDisponibilidad(WayPoint[] camino,string objetivo,bool valor){

        for (int i=0; i< camino.Length; i++)
        {
            if (camino[i].getNombre() == objetivo)
            {
                camino[i].setDisponible(valor);
            }
        }
    }
    public void setDisponible(string obj,bool value){

        bool band = false;

        if (obj == WayPoint.ARMERIA)
        {
            band = true;
        }

        if (azul)
        {
            if (obj == WayPoint.TORRE_VIGIA || obj == WayPoint.PUENTE_IZQUIERDO_AZUL)
            {

                setDisponibilidad(caminoIzq,obj,value);

            }else if(obj == WayPoint.ARMERIA || obj == WayPoint.PUENTE_DERECHO_AZUL){

                setDisponibilidad(caminoDer,obj,value);
                
                if(band){

                    setDisponibilidad(caminoDer,WayPoint.CON_ARMERIA,!value);
                }
                
            }else if(obj == WayPoint.SANTUARIO || obj == WayPoint.PUENTE_IZQUIERDO_ROJO){
            
                setDisponibilidad(caminoIzqEnemigo,obj,value);

            }else{

                setDisponibilidad(caminoDerEnemigo,obj,value);
            }
        }else{

            if (obj == WayPoint.TORRE_VIGIA || obj == WayPoint.PUENTE_IZQUIERDO_AZUL)
            {

                setDisponibilidad(caminoIzqEnemigo,obj,value);

            }else if(obj == WayPoint.ARMERIA || obj == WayPoint.PUENTE_DERECHO_AZUL){

                setDisponibilidad(caminoDerEnemigo,obj,value);

                if(band){

                    setDisponibilidad(caminoDerEnemigo,WayPoint.CON_ARMERIA,!value);
                }
                
            }else if(obj == WayPoint.SANTUARIO || obj == WayPoint.PUENTE_IZQUIERDO_ROJO){
            
                setDisponibilidad(caminoIzq,obj,value);

            }else{

                setDisponibilidad(caminoDer,obj,value);
            }
        }
        
    }
    private void camnioDeSentido(){

        Array.Reverse(caminoDer);
        Array.Reverse(caminoIzq);
        Array.Reverse(caminoDerEnemigo);
        Array.Reverse(caminoIzqEnemigo);
    }
}
