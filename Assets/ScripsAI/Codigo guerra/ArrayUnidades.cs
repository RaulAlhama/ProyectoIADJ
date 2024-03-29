using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUnidades
{
    private int[,] array;
    public const int LIBRE = 0;
    public const int ARQUEROROJO = 1;
    public const int UNIDADPESADAROJO = 2;
    public const int EXPLORADORROJO = 3;
    public const int PATRULLAROJO = 4;
    public const int ARQUEROAZUL = 5;
    public const int UNIDADPESADAAZUL = 6;
    public const int EXPLORADOAZUL = 7;
    public const int PATRULLAAZUL = 8;

    public ArrayUnidades(int a, int b){

        array = new int[a,b];

    }

    public void setUnidad(int i, int j, int unidad){

        array[i,j] = unidad;
    }

    public int getValorUnidad(int i,int j){

        return array[i,j];
    }
    public int[,] getArray(){

        return array;
    }
}
