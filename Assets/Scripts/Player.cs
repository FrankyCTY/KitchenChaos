using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDirection;

    // Everything here by default relate to each Frame, but:
    // 1. We use DeltaTime to be frame rate independent
    // 2. Accumulate changes to the attached game object by using += from each frame update
    private void Update()
    {
        this.HandleMovement();
        this.HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            // Cache old direction to ensure even just facing the object with physics collider can still trigger event (etc. clear counter interaction)
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2f;
        
        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                clearCounter.Interact();
            };
        } else
        {
            Debug.Log("-");
        }
    }

    private void HandleMovement()
    {
        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        

        // x, y, z -> only getting direction for current update exp: (x: 1f, y: 0, z: 0)
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        // How many distance moved in an unit of the deltaTime (frame rate independent)
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        var position = transform.position;

        var (canMoveToX, moveDirectionToX) =
            CheckCanMoveTowardsX(moveDirection, position, moveDistance, playerHeight, playerRadius);

        if (canMoveToX)
        {
            transform.position += moveDirectionToX * moveDistance;
        }

        var (canMoveToZ, moveDirectionToZ) =
            CheckCanMoveTowardsZ(moveDirection, position, moveDistance, playerHeight, playerRadius);

        if (canMoveToZ)
        {
            transform.position += moveDirectionToZ * moveDistance;
        }
        
        // Face direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * this.rotateSpeed);

        this.isWalking = moveDirection != Vector3.zero;
    }

    private static (bool, Vector3) CheckCanMoveTowardsX(Vector3 moveDirection, Vector3 position, float moveDistance,
        float playerHeight, float playerRadius)
    {
        // Try move towards X
        // As we are reassigning to a new vector3, the normalized y is gone, so we need to normalized it again
        Vector3 moveDirectionToX = (Vector3.right * moveDirection.x).normalized;
        bool canMoveToX = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
            moveDirectionToX, moveDistance);

        return (canMoveToX, moveDirectionToX);
    }

    private static (bool, Vector3) CheckCanMoveTowardsZ(
        Vector3 moveDirection, Vector3 position, float moveDistance,
        float playerHeight, float playerRadius
    )
    {
        // Try move towards Z
        Vector3 moveDirectionToZ = (Vector3.forward * moveDirection.z).normalized;
        bool canMoveToZ = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
            moveDirectionToZ, moveDistance);

        return (canMoveToZ, moveDirectionToZ);
    }

    public bool IsWalking()
    {
        return this.isWalking;
    }
}