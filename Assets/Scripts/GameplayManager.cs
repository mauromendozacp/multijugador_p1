using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Collections;

public class GameplayManager : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private WinController winController = null;
    [SerializeField] private Transform[] spawns = null;
    [SerializeField] private GameObject winPanel = null;
    [SerializeField] private TMP_Text chickenNameWinner = null;
    #endregion

    #region PRIVATE_FIELDS
    private bool isEnd = false;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");

            return;
        }

        InstancePlayer();
        winController.Init((name) =>
        {
            photonView.RPC("EndGame", RpcTarget.All, name);
        });
    }
    #endregion

    #region PUBLIC_METHODS
    public void LeftLobby()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    #endregion

    #region PRIVATE_METHODS
    private void InstancePlayer()
    {
        PhotonNetwork.Instantiate(prefab.name, GetRandomPosition(), Quaternion.identity, 0);
    }

    private Vector3 GetRandomPosition()
    {
        int index = Random.Range(0, spawns.Length);
        return spawns[index].position;
    }
    
    [PunRPC]
    private void EndGame(string chickenName)
    {
        winPanel.SetActive(true);
        chickenNameWinner.text = chickenName;
    }
    #endregion
}
