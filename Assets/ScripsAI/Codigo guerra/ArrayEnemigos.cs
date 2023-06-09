using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrayEnemigos : MonoBehaviour
{
    private int[,] array;
    private const int A_SALVO = 0;
    private const int PELIGRO_ALTO = 3;
    private const int  PELIGRO_MEDIO = 2;
    private const int  PELIGRO_BAJO = 1;
    private const int RANGO_INTERNO = 2;
    private const int RANGO_MEDIO = 4;
    private const int RANGO_EXTERNO = 5;


    public ArrayEnemigos(int a, int b){

        array = new int[a,b];
    }
    private bool esPeligroAlto(int x,int y,int i,int j){

        if (Math.Abs(x-i) <= RANGO_INTERNO && Math.Abs(y-j) <= RANGO_INTERNO)
        {
            return true;
        }else{

            return false;
        }
    }
    private bool esPeligroBajo(int x,int y,int i,int j){

        if (Math.Abs(x-i) > RANGO_MEDIO || Math.Abs(y-j) > RANGO_MEDIO)
        {
            return true;
        }else{

            return false;
        }
    }
    public void setPeligro(int i, int j){

        int[] lim = new int[4];

        
        lim = setLimites(i,j,RANGO_EXTERNO);
        for (int x = lim[0]; x <= lim[1]; x++)
        {
            for (int y = lim[2]; y <= lim[3]; y++)
            {
                if (esPeligroAlto(x,y,i,j))
                {
                    array[x,y] = PELIGRO_ALTO;
                }else if (esPeligroBajo(x,y,i,j) && array[x,y] < PELIGRO_BAJO)
                {
                    array[x,y] = PELIGRO_BAJO;
                }else if(array[x,y] < PELIGRO_MEDIO)
                {
                    array[x,y] = PELIGRO_MEDIO;
                }
                
            }
        }
        
    }
    public void resetArea(){

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                array[i, j] = 0;
            }
        }
    }
    private int[] setLimites(int i,int j,int val){

        int[] limites = new int[4];
        if (i-val < 0)
        {
            limites[0] = 0;
        }else{

            limites[0] = i-val;
        }
        if (i+val > 99)
        {
            limites[1] = 99;
        }else
        {
            limites[1] = i+val;
        }
        if (j-val < 0)
        {
            limites[2] = 0;
        }else
        {
            limites[2] = j-val;
        }
        if (j+val > 99)
        {
            limites[3] = 99;
        }else
        {
            limites[3] = j+val;
        }
        return limites;
    }
    public int getValorTerreno(int i,int j){

        return array[i,j];
    }
    public int[,] getArray(){

        return array;
    }
}
