using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    // Everything here by default relate to each Frame, but:
    // 1. We use DeltaTime to be frame rate independent
    // 2. Accumulate changes to the attached game object by using += from each frame update
    private void Update()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();

        // x, y, z -> only getting direction for current update exp: (x: 1f, y: 0, z: 0)
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        // How many distance moved in an unit of the deltaTime (frame rate independent)
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        var position = transform.position;
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirection,
            moveDistance);

        if (!canMove)
        {
            // Try move towards X
            // As we are reassigning to a new vector3, the normalized y is gone, so we need to normalized it again
            Vector3 moveDirectionToX = (Vector3.right * moveDirection.x).normalized;
            bool canMoveToX = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
                moveDirectionToX, moveDistance);

            if (canMoveToX)
            {
                canMove = true;
                moveDirection = moveDirectionToX;
            }
            else
            {
                // Try move towards Z
                Vector3 moveDirectionToZ = (Vector3.forward * moveDirection.z).normalized;
                bool canMoveToZ = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
                    moveDirectionToZ, moveDistance);

                if (canMoveToZ)
                {
                    canMove = true;
                    moveDirection = moveDirectionToZ;
                }
            }
        }

        if (canMove)
        {
            // Moving direction
            transform.position += moveDirection * moveDistance;
        }

        // Face direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * this.rotateSpeed);

        this.isWalking = moveDirection != Vector3.zero;
    }

    public bool IsWalking()
    {
        return this.isWalking;
    }
}