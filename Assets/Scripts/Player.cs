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

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up* playerHeight, playerRadius, moveDir, moveDistance);
       
        if (!canMove)
        {   //Attempt X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up* playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {   //can move only in X
                moveDir = moveDirX;
            } else
                {   //Attempt Z movement
                    Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                    canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up* playerHeight, playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {   //can move only in z dir
                        moveDir = moveDirZ;
                    } else
                        {
                            //can not move
                        }

                }
        }
        if (canMove)
        {
           transform.position += moveDir * moveSpeed * Time.deltaTime; 
        }
        
        isWAlking = moveDir != Vector3.zero;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
    }

    public bool IsWalking(){
        return isWAlking;
    }
}
