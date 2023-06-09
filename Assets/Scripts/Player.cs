using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickUp;
    public event EventHandler<HandleSelectedCounterChangedEventArgs> HandleSelectedCounterChanged;

    public class HandleSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldingPoint;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance is not null)
        {
            // Debug.LogError("There is more than one Player instance!");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.HandleInteractAction += GameInput_HandleInteraction;
        gameInput.HandlerInteractAlternateAction += GameInput_HandleInteractionAlternate;
    }

    private void GameInput_HandleInteractionAlternate(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (this.selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_HandleInteraction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (this.selectedCounter != null)
        {
            this.selectedCounter.Interact(this);
        }
    }

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
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            // Cache old direction to ensure even just facing the object with physics collider can still trigger event (etc. clear counter interaction)
            lastInteractDirection = moveDirection;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance,
                countersLayerMask))
        {
            // Set selected counter for visual effect on the selected counter
            // Debug.Log($"Detected counter direction {clearCounter.name}");
            this.SetSelectedCounter(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)
                ? baseCounter
                : null);
        }
        else
        {
            this.SetSelectedCounter(null);
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

        this.UpdatePosition(moveDirection, position, moveDistance, playerHeight, playerRadius);

        // Face direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * this.rotateSpeed);
        this.isWalking = moveDirection != Vector3.zero;
    }

    private void UpdatePosition(Vector3 moveDirection, Vector3 position, float moveDistance, float playerHeight,
        float playerRadius)
    {
        bool canMoveToDesiredDirection =
            CheckCanMoveTowardsDesiredDirection(moveDirection, position, moveDistance, playerHeight, playerRadius);

        if (canMoveToDesiredDirection)
        {
            transform.position += moveDirection * moveDistance;
            return;
        }
        
        this.tryMovingUserToAdjustedPosition(moveDirection, position, moveDistance, playerHeight, playerRadius);
    }

    private void tryMovingUserToAdjustedPosition(Vector3 moveDirection, Vector3 position, float moveDistance, float playerHeight, float playerRadius)
    {
        var (canMoveToX, moveDirectionToX) =
            CheckCanMoveTowardsX(moveDirection, position, moveDistance, playerHeight, playerRadius);

        if (canMoveToX)
        {
            transform.position += moveDirectionToX * moveDistance;
            return;
        }

        var (canMoveToZ, moveDirectionToZ) =
            CheckCanMoveTowardsZ(moveDirection, position, moveDistance, playerHeight, playerRadius);

        if (canMoveToZ)
        {
            transform.position += moveDirectionToZ * moveDistance;
        }
    }

    private static bool CheckCanMoveTowardsDesiredDirection(Vector3 moveDirection, Vector3 position, float moveDistance,
        float playerHeight, float playerRadius)
    {
        bool canMoveToDesiredDirection = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirection,
            moveDistance);
        return canMoveToDesiredDirection;
    }

    private static (bool, Vector3) CheckCanMoveTowardsX(Vector3 moveDirection, Vector3 position, float moveDistance,
        float playerHeight, float playerRadius)
    {
        // Try move towards X
        // As we are reassigning to a new vector3, the normalized y is gone, so we need to normalized it again
        Vector3 moveDirectionToX = (Vector3.right * moveDirection.x).normalized;
        bool isMovingForward = moveDirectionToX.x < -.5f || moveDirectionToX.x > +.5f;
        bool canMoveToX = isMovingForward && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
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
        bool isMovingSideway = moveDirectionToZ.z < -.5f || moveDirectionToZ.x> +.5f;
        bool canMoveToZ = isMovingSideway && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
            moveDirectionToZ, moveDistance);

        return (canMoveToZ, moveDirectionToZ);
    }

    public bool IsWalking()
    {
        return this.isWalking;
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this.selectedCounter = baseCounter;
        this.HandleSelectedCounterChanged?.Invoke(this, new HandleSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }

    public Transform GetKitchenObjectHoldingPointTransform()
    {
        return this.kitchenObjectHoldingPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject is not null)
        {
            OnPickUp?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void clearKitchenObject()
    {
        this.kitchenObject = null;
        // Clear the rendered kitchen object on the counter top point
        // Potentially move to a dedicated counterTopPoint script
        this.ClearChildrenObjectsInCounterTopPoint();
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
    
    private void ClearChildrenObjectsInCounterTopPoint()
    {
        for (int i = 0; i < this.kitchenObjectHoldingPoint.childCount; i++)
        {
            // Get the child at index i
            Transform child = this.kitchenObjectHoldingPoint.GetChild(i);

            // Destroy the child object
            GameObject.Destroy(child.gameObject);
        }
    }
}