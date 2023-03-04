using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridFinal 
{
    private int ancho;
    private int largo;
    private int[,] gridArray;
    private float tam;
    public const int PLAYER = 3;
    public const int NPC = 2;
    public const int LIBRE = 0;
    public const int OBSTACULO = 1;

    public GridFinal(int ancho, int largo, float tamCasilla){

        this.ancho = ancho;
        this.largo = largo;
        this.tam = tamCasilla;

        gridArray = new int[ancho,largo];
        

        for(int i = 0; i < gridArray.GetLength(0); i++){
            for(int j = 0; j < gridArray.GetLength(1); j++){

                Debug.DrawLine(getPosicionReal(i,j), getPosicionReal(i,j+1), Color.blue, 100f);
                Debug.DrawLine(getPosicionReal(i,j), getPosicionReal(i+1,j), Color.blue, 100f);

            }
        }
        Debug.DrawLine(getPosicionReal(0,largo), getPosicionReal(ancho,largo), Color.blue, 100f);
        Debug.DrawLine(getPosicionReal(ancho,0), getPosicionReal(ancho,largo), Color.blue, 100f);

        
    }
    public Vector3 getPosicionReal(int x, int y){

        return new Vector3(x,0,y) * tam;
    }
    public void getCoordenadas(Vector3 posicionReal, out int i, out int j){

        i = Mathf.FloorToInt(posicionReal.x / tam);
        j = Mathf.FloorToInt(posicionReal.z / tam);
    }
    public void setValor(Vector3 pos, int value){

        int x;
        int y;
        getCoordenadas(pos, out x,out y);
        if((x < ancho && x >=0) && (y < largo  && y >=0))
            gridArray[x,y] = value;
    }
    private void setValor(int i, int j, int value){

        gridArray[i,j] = value;
    }
    public void setObstaculos(GameObject[] list){
        //Debug.Log(gridArray[15,10]);
        foreach(GameObject obs in list){
            //Debug.Log(obs.transform.tag);
            setValor(obs.transform.position, OBSTACULO);
        }
        Debug.Log(gridArray[15,10]);
        
    }
    public double[,] getGrafo(){ /// Vector3 posTarget

        int iObjetivo = 15; //getCoordenadas
        int jObjetivo = 10;
        double[,] grafoMovimiento = new double[ancho,largo];

        for(int i = 0; i < grafoMovimiento.GetLength(0); i++){
            for(int j = 0; j < grafoMovimiento.GetLength(1); j++){
                
                if(gridArray[i,j] == OBSTACULO)
                    grafoMovimiento[i,j] = Double.PositiveInfinity;
                else
                    grafoMovimiento[i,j] = Mathf.Abs(i-iObjetivo)+Mathf.Abs(j-jObjetivo);
                
                //if(grafoMovimiento[i,j] != Double.PositiveInfinity)
                    //Debug.Log("(" + i + "," + j + ") : " +grafoMovimiento[i,j]);
            }
        }
        return grafoMovimiento;
    }
    

}

