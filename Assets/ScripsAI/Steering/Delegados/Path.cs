using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Path: MonoBehaviour
{
    public Node[] nodes;

    private void Awake(){
        
        for (int i = 0; i < nodes.Length; i++){
            
            nodes[i].index = i; //Inicializamos los nodos
        }
    }

    public int GetNearestLine(Vector3 currentPosition){ //posicion del agente
        
        int nearestPosition = 0;
        Vector3 min = GetMappedPositionOnPath(0) - currentPosition; //vector con dirección nodo 0
        
        for (int i = 1; i < nodes.Length; i++){
            
            Vector3 result = GetMappedPositionOnPath(i) - currentPosition;
            
            if(result.magnitude < min.magnitude){  //Si hay un nodo más cerca que el nodo min, elegimos ese como objetivo
                
                min = result;
                nearestPosition = i;
            }
        }

        return nearestPosition; //Devolvemos el indice del nodo más cercano
    }
    //A través del índice del nodo, devolvemos su posición
    public Vector3 GetMappedPositionOnPath(int nodePosition){

        return nodes[nodePosition].transform.position; //Devuelve la posición del nodo
    }

    public int nextPoint(int currentIndexOnPath){

        if(currentIndexOnPath == nodes.Length - 1){ //Si el indice actual, es el del último nodo, invertimos el array para recorer el camino hacia atrás.
            Array.Reverse(nodes);
            return 1; //Devolvemos 1 porque al invertir el array, el penultimo pasa a ser el segundo.
        }

        return currentIndexOnPath + 1; //Si no es el último, devolvemos el siguiente
    }
}
