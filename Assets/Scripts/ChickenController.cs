using UnityEngine;
using Photon.Pun;

public class ChickenController : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float rotSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private int lives = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private CapsuleCollider capsuleCollider = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private LayerMask jumpleableMask = default;
    #endregion

    #region PRIVATE_FIELDS
    private float halfHeight = 0f;
    private bool isRun = false;
    private bool isDead = false;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        halfHeight = capsuleCollider.bounds.extents.y / 2;

        if (photonView.IsMine)
        {
            cameraController.OnStartFollowing();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            Run();
            Rotate();
            Jump();
        }
    }
    #endregion

    #region PUBLIC_METHODS

    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        float axisY = Input.GetAxis("Vertical");
        float axisAbs = Mathf.Abs(axisY);

        if (axisAbs > Mathf.Epsilon)
        {
            Vector3 moveVector = transform.forward * axisY;
            moveVector *= isRun ? runSpeed : walkSpeed;

            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

            animator.SetFloat("Speed", isRun ? axisAbs : axisAbs / 2);
        }
    }

    private void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles += Vector3.down * rotSpeed;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles += Vector3.up * rotSpeed;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, halfHeight + 0.05f, jumpleableMask);
    }
    #endregion
}
