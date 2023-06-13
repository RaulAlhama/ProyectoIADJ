using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leyenda : MonoBehaviour
{

    [SerializeField]
    private GameObject debugInfo;


    private Vector2 selectionOrigin;

    protected bool modoDebug = false;


    void Update(){

        if (Input.GetKeyDown(KeyCode.H)){
            modoDebug = !modoDebug;
            debugInfo.SetActive(modoDebug);
        }
            
    }

    /*void FixedUpdate()
    {
         if (modoDebug){
            
         } else{
            debugInfo.SetActive(false);
         }
    }*/




    

}
