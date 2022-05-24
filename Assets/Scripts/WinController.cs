using System;
using UnityEngine;

public class WinController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private LayerMask chickenMask = default;
    #endregion

    #region PRIVATE_FIELDS
    private Action<string> onFinishGame = null;
    private bool finish = false;
    #endregion

    #region UNITY_CALLS
    private void OnTriggerEnter(Collider other)
    {
        if (!finish)
        {
            if (CheckLayerInMask(chickenMask, other.gameObject.layer))
            {
                finish = true;
                string chickenName = other.gameObject.GetComponent<ChickenController>().GetName();
                onFinishGame?.Invoke(chickenName);
            }
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(Action<string> onFinishGame)
    {
        this.onFinishGame = onFinishGame;
    }
    #endregion

    #region PRIVATE_METHODS
    public bool CheckLayerInMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    #endregion
}
