using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    private bool isWAlking;

    private void Update() {
        Vector2 inputvector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputvector.x, 0f, inputvector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWAlking = moveDir != Vector3.zero;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
    }

    public bool IsWalking(){
        return isWAlking;
    }
}
