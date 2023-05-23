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
    public const int NPCAZUL = 4;
    public const int NPCROJO = 5;
    public const int ARMERIA = 6;
    public const int SANTUARIO = 7;
    public const int VIGIA = 8;
    public const int ESTATUAAZUL = 9;
    public const int ESTATUAROJA = 10;
    private bool Man,Chev,Euc;

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
    public void setDistancia(int val){

        if(val == 1){

            Man = true;
            Chev = false;
            Euc = false;
        }else if(val == 2){

            Man = false;
            Chev = true;
            Euc = false;

        }else if(val == 3){

            Man = false;
            Chev = false;
            Euc = true;
        }
    }
    public Vector3 getPosicionReal(int x, int y){

        return new Vector3(x,0,y) * tam; //+ new Vector3(tam/2,0,tam/2);
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
    public void setValor(int i, int j, int value){

        gridArray[i,j] = value;
    }
    public int getValor(int i, int j){

        return gridArray[i,j];
    }
    public void setObstaculos(GameObject[] list){
        //Debug.Log(gridArray[15,10]);
        foreach(GameObject obs in list){
            //Debug.Log(obs.transform.tag);
            setValor(obs.transform.position, OBSTACULO);
        }

        
    }
    public double[,] getGrafo(int iObjetivo, int jObjetivo){ /// Vector3 posTarget

        double[,] grafoMovimiento = new double[ancho,largo];

        for(int i = 0; i < grafoMovimiento.GetLength(0); i++){
            for(int j = 0; j < grafoMovimiento.GetLength(1); j++){
                
                if(gridArray[i,j] == OBSTACULO)
                    grafoMovimiento[i,j] = Double.PositiveInfinity;
                else{

                    if(Man){

                        grafoMovimiento[i,j] = Mathf.Abs(i-iObjetivo)+Mathf.Abs(j-jObjetivo);

                    }else if(Chev){

                        grafoMovimiento[i,j] = Mathf.Max(Mathf.Abs(i-iObjetivo),Mathf.Abs(j-jObjetivo));

                    }else if(Euc){

                        grafoMovimiento[i,j] = Math.Sqrt(Math.Pow(iObjetivo-i,2)+Mathf.Pow(jObjetivo-j,2));
                    }
                    // //Manhattan 2:10:25
                    // //Chebyshev 2:49:28
                     //Euclidea 0:57:65
                }
                    
                //if(grafoMovimiento[i,j] != Double.PositiveInfinity)
                    //Debug.Log("(" + i + "," + j + ") : " +grafoMovimiento[i,j]);
            }
        }
        return grafoMovimiento;
    }
    public bool Posible(int i, int j){

        if(gridArray[i,j] != GridFinal.OBSTACULO){
            return true;
        }
        else{
            return false;
        }
    }
    

}