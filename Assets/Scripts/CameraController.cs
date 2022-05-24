using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Vector2 distance = Vector2.zero;
    #endregion

    #region PRIVATE_FIELDS
    private Transform camTransform = null;
    #endregion

    #region PROPERTIES
    public Transform CamTransform { get => camTransform; }
    public bool Follow { get; set; } = false;
    #endregion

    #region UNITY_CALLS
    private void LateUpdate()
    {
        if (Follow)
        {
            Vector3 distancePos = transform.position - new Vector3(0, distance.y, distance.x);
            camTransform.position = distancePos;
            camTransform.LookAt(transform.position);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void OnStartFollowing()
    {
        camTransform = Camera.main.transform;
        Follow = true;
    }
    #endregion
}