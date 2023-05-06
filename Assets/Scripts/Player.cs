using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private void Update()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Moving direction
        transform.position += moveDirection * (Time.deltaTime * this.moveSpeed);
        // Face direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * this.rotateSpeed);
        
        this.isWalking = moveDirection != Vector3.zero;
    }

    public bool IsWalking()
    {
        return this.isWalking;
    }
}
