using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCamara : MonoBehaviour
{

    private Vector3 limitesInferiores = new Vector3(35,30,0);
    private Vector3 limitesSuperiores = new Vector3(415,100,365);
    private Vector3 inicio = new Vector3(250,62,11);

    [SerializeField]
    private float speedCamera = 50;

    private int LFinput, FBinput;
    private float UPinput;

    void Start()
    {

        transform.position = inicio;
    }

    // Update is called once per frame
    void Update()
    {
        LFinput = 0; //Left & Right input
        FBinput = 0; //Foward & Back input
        UPinput = 0; //Up & down input

        if (Input.GetKey(KeyCode.A))
            LFinput = -1;
        else if (Input.GetKey(KeyCode.D))
            LFinput = 1;

        if (Input.GetKey(KeyCode.W))
            FBinput = 1;
        else if(Input.GetKey(KeyCode.S))
            FBinput = -1;

        if (Input.GetKey(KeyCode.Q))
            UPinput = 1;
        else if (Input.GetKey(KeyCode.E))
            UPinput = -1;
    }

    void FixedUpdate()
    {
        Vector3 move = Vector3.zero;
        if (LFinput < 0 && transform.position.z - Time.fixedDeltaTime * speedCamera >= limitesInferiores.z)
        {
            move += Vector3.back;
        }else if (LFinput > 0 && transform.position.z + Time.fixedDeltaTime * speedCamera <= limitesSuperiores.z)
        {
            move += Vector3.forward;
        }
        if (FBinput < 0 && transform.position.x - Time.fixedDeltaTime * speedCamera <= limitesSuperiores.x)
        {
            move += Vector3.right;
        }
        else if (FBinput > 0 && transform.position.x + Time.fixedDeltaTime * speedCamera >= limitesInferiores.x)
        {
            move += Vector3.left;
        }
        if (UPinput < 0 && transform.position.y + Time.fixedDeltaTime * speedCamera <= limitesSuperiores.y )
        {
            move += Vector3.up;
        }
        else if (UPinput > 0 && transform.position.y - Time.fixedDeltaTime * speedCamera >= limitesInferiores.y)
        {
            move += Vector3.down;
        }
        transform.position = transform.position + move.normalized * Time.fixedDeltaTime * speedCamera;
    }
}
