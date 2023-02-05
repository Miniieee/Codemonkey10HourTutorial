using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs: EventArgs {
        public ClearCounter selectedCounter;
    }


    public static Player Instance {get; private set;}

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayermask;

    private bool isWAlking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake() {
       
        if (Instance != null)
        {
            Debug.LogError("there is more than one Player instance");
        }

        Instance = this;
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter !=null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
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

    private void HandleInteractions()
    {
        Vector2 inputvector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputvector.x, 0f, inputvector.y);
        
        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayermask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //Has ClearCounter
                
                if (clearCounter != selectedCounter)
                {
                    selectedCounter = clearCounter;
                    SetSelectedCounter(clearCounter);
                }
                else
                    {
                        SetSelectedCounter(null);
                    }
            }
        } else
        {
            SetSelectedCounter(null);
        }

        Debug.Log(selectedCounter);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public bool IsWalking(){
        return isWAlking;
    }
}
