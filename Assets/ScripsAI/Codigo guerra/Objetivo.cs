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

}