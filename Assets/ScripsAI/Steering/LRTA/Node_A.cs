using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Clase que representa un nodo del algoritmo A*
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
        h = 0.0f;
        g = 0.0f;
        f = 0.0f;
    }
}
