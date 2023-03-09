using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing
{
    public Path path;
    public float predictTime;
    public int currentIndexOnPath = -1; //Indice del nodo, Al principio -1 porque vamos a obtener el más cercano
    private PlayerPathFollowing player;

    public PathFollowing(PlayerPathFollowing jugador, Path camino){
            
        player = jugador;
        path = camino;
    }

    public Vector3 getSiguienteObjetivo(){

        //int nearestPosition; //nodo objetivo (esta variable se puede obviar)

        if(currentIndexOnPath == -1){
            
            currentIndexOnPath = path.GetNearestLine(player.Position); // Calculamos el nodo más cercano la primera vez
        }else{

            currentIndexOnPath = path.nextPoint(currentIndexOnPath); //obtenemos el siguiente nodo
        }


        Vector3 targetPosition = path.GetMappedPositionOnPath(currentIndexOnPath); //A través del índice del nodo, obtenemos su posición
        //Debug.Log("PathFollowing.cs: " + currentIndexOnPath);
        return targetPosition; //La devolvemos
    }
}
