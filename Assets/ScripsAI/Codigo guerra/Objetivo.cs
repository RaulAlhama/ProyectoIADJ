using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Objetivo
{
    public const int NEUTRAL = 0;
    public const int AZUL = 1;
    public const int ROJO = 2;

    public const string TORRE_VIGIA = "torre vigia";
    public const string ARMERIA = "armeria";
    public const string PUENTE_IZQUIERDO_AZUL = "puente izquierdo azul";
    public const string PUENTE_DERECHO_AZUL = "puente derecho azul";
    public const string SANTUARIO = "santuario";
    public const string ESCUDERIA = "escuderia";
    public const string PUENTE_IZQUIERDO_ROJO = "puente izquierdo rojo";
    public const string PUENTE_DERECHO_ROJO = "puente derecho rojo";

    private int prioridad;
    private Coordenada[] cordes;
    private int propiedad;
    private string nombre;

    public Objetivo(int prio, Coordenada[] cor, string nom){

        prioridad = prio;
        cordes = cor;
        propiedad = Objetivo.NEUTRAL;
        nombre = nom;
    }
    public Coordenada[] getSlots(){

        return cordes;
    }
    public int getPropiedad(){

        return propiedad;
    }
    public void setPropiedad(int val){

        propiedad = val;
    }
    public string getNombre(){

        return nombre;
    }
    public int getPrioridad(){

        return prioridad;
    }

    public int getXInicial(){
        int min=900;
        for (int i=0; i<cordes.Length;i++){
            if (cordes[i].getX()<=min)
                min = cordes[i].getX();
        }
        return min;
        
    }

    public int getXFinal(){
        int max=0;
        for (int i=0; i<cordes.Length;i++){
            if (cordes[i].getX()>=max)
                max = cordes[i].getX();
        }
        return max;
        
    }

    public int getYInicial(){
        int min=900;
        for (int i=0; i<cordes.Length;i++){
            if (cordes[i].getY()<=min)
                min = cordes[i].getY();
        }
        return min;
        
    }

    public int getYFinal(){
        int max=0;
        for (int i=0; i<cordes.Length;i++){
            if (cordes[i].getY()>=max)
                max = cordes[i].getY();
        }
        return max;
        
    }

}
