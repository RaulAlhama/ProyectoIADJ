using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTerreno
{
    private int[,] array;
    public const int SUELO = 0;
    public const int TIERRA = 1;
    public const int CESPED = 2;

    public ArrayTerreno(int a, int b){

        array = new int[a,b];
    }

    public void setTerreno(int i, int j, int unidad){

        array[i,j] = unidad;
    }

    public int getValorTerreno(int i,int j){

        return array[i,j];
    }
    public int[,] getArray(){

        return array;
    }
}
