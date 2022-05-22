using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private float walkSpeed = 0f;
    [SerializeField] private float rotSpeed = 0f;
    [SerializeField] private float runSpeed = 0f;
    [SerializeField] private int lives = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private CameraController cameraController = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool isRun = false;
    private bool isDead = false;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        if (photonView.IsMine)
        {
            cameraController.OnStartFollowing();
        }
    }

    private void Update()
    {
        Move();
        Run();
        Rotate();
        Jump();
    }
    #endregion

    #region PUBLIC_METHODS

    #endregion

    #region PRIVATE_METHODS
    private void Move()
    {
        float axisY = Input.GetAxis("Vertical");

        if (Mathf.Abs(axisY) > Mathf.Epsilon)
        {
            Vector3 moveVector = transform.forward * axisY;
            moveVector *= isRun ? runSpeed : walkSpeed;

            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

            animator.SetFloat("Speed", isRun ? axisY: axisY / 2);
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
        float axisX = Input.GetAxis("Horizontal");

        if (Mathf.Abs(axisX) > Mathf.Epsilon)
        {
            Vector3 euler = Vector3.zero;
            euler.y = axisX * rotSpeed;

            transform.eulerAngles += euler;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion
}
