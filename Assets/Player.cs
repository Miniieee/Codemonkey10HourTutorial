using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;

    private bool isWAlking;

    private void Update() {
        Vector2 inputvector = new Vector2(0,0);


        if(Input.GetKey(KeyCode.W))
        {
            inputvector.y = +1;
        }

        if(Input.GetKey(KeyCode.S))
        {
            inputvector.y = -1;
        }

        if(Input.GetKey(KeyCode.A))
        {
            inputvector.x = -1;
        }

        if(Input.GetKey(KeyCode.D))
        {
            inputvector.x = +1;
        } 

        inputvector = inputvector.normalized;

        Vector3 moveDir = new Vector3(inputvector.x, 0f, inputvector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWAlking = moveDir != Vector3.zero;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
    }

    public bool IsWalking(){
        return isWAlking;
    }
}
