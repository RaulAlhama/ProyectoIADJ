using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leyenda : MonoBehaviour
{

    [SerializeField]
    private GameObject debugInfo;
    [SerializeField]
    private GameObject guerraTotal;


    private Vector2 selectionOrigin;

    protected bool modoDebug = false;
    protected bool isGuerra = false;


    void Update(){

        if (Input.GetKeyDown(KeyCode.H)){
            modoDebug = !modoDebug;
            debugInfo.SetActive(modoDebug);
        }

        if (Input.GetKeyDown(KeyCode.G)){
            isGuerra = !isGuerra;
            guerraTotal.SetActive(isGuerra);
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
