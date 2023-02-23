using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path: MonoBehaviour
{
    public Node[] nodes;

    private void Awake(){
        
        for (int i = 0; i < nodes.Length; i++){
            
            nodes[i].index = i;
        }
    }

    public int GetNearestLine(Vector3 currentPosition){
        
        int nearestPosition = 0;
        Vector3 min = GetMappedPositionOnPath(0) - currentPosition;
        
        for (int i = 1; i < nodes.Length; i++){
            
            Vector3 result = GetMappedPositionOnPath(i) - currentPosition;
            
            if(result.magnitude < min.magnitude){
                
                min = result;
                nearestPosition = i;
            }
        }

        return nearestPosition;
    }

    public Vector3 GetMappedPositionOnPath(int nodePosition){
        
        if(nodePosition == nodes.Length - 1){
            
            return nodes[nodePosition].transform.position;
        }

        return nodes[nodePosition].transform.position;
    }
    public int nextPoint(int currentIndexOnPath){

        if(currentIndexOnPath == nodes.Length - 1){
            
            return 0;
        }

        return currentIndexOnPath + 1;
    }
}
