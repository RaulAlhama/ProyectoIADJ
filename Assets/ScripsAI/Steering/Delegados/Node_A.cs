using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node_A
{
    public AEstrella.Coordenadas corde;
    public Node_A padre { get; set; }
    public double h { get; set; }
    public double g { get; set; }
    public double f { get; set; }

    public Node_A(AEstrella.Coordenadas actual, Node_A pa, int iObjetivo, int jObjetivo){
        corde = actual;
        padre = pa;
        //h =  Math.Abs(corde.x - iObjetivo) + Math.Abs(corde.y - jObjetivo);  // Calcular distancia
        h = 0.0f;

        /*
        if (padre is null){
            g = 0.0f;
        }
        else{
            g = Math.Abs(corde.x - pa.corde.x) + Math.Abs(corde.y - pa.corde.y); ;
        }*/

        g = 0.0f;
        f = 0.0f;
        //f = g + h;
    }
}
