using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : Seek
{
    public Path path;
    public float predictTime;
    public int currentIndexOnPath = -1;
    private AgentPlayer player;

    public PathFollowing(AgentPlayer jugador, Path camino){
            
        player = jugador;
        path = camino;
    }

    public Vector3 getSiguienteObjetivo(){

        int nearestPosition;

        if(currentIndexOnPath == -1){
            
            nearestPosition = path.GetNearestLine(player.Position);
        }else{

            nearestPosition = path.nextPoint(currentIndexOnPath);
        }

        currentIndexOnPath = nearestPosition;

        Vector3 targetPosition = path.GetMappedPositionOnPath(currentIndexOnPath);
        Debug.Log(currentIndexOnPath);
        return targetPosition;
    }
}
