using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameplayManager : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private Transform[] spawns = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");

            return;
        }

        InstancePlayer();
    }
    #endregion

    #region PRIVATE_METHODS
    private void InstancePlayer()
    {
        int index = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.Instantiate(prefab.name, spawns[index].position, Quaternion.identity, 0);
    }
    #endregion

    #region OVERRIDE_METHODS

    #endregion
}
