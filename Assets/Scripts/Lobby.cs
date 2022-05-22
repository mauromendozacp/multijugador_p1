using UnityEngine;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject controlPanel;
	[SerializeField] private TMP_Text feedbackText;
	[SerializeField] private byte maxPlayersPerRoom = 4;
    [SerializeField] private LoaderAnime loaderAnime;
    #endregion

    #region PRIVATE_FIELDS
    private bool isConnecting = false;
    private bool roomCreated = false;
    private string gameVersion = "1";
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
	#endregion

	#region PUBLIC_METHODS
	public void CreateRoom()
	{
		LoadingRoom();

		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = gameVersion;
		roomCreated = true;
	}

	public void JoinRandomRoom()
	{
		LoadingRoom();

		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = gameVersion;

		roomCreated = false;
	}
	#endregion

	#region PRIVATE_METHODS
	private void LogFeedback(string message)
	{
		if (feedbackText == null)
		{
			return;
		}

		feedbackText.text += System.Environment.NewLine + message;
	}

	private void LoadingRoom()
	{
		feedbackText.text = "";
		isConnecting = true;
		controlPanel.SetActive(false);
		if (loaderAnime != null)
		{
			loaderAnime.StartLoaderAnimation();
		}
	}
	#endregion

	#region OVERRIDE_METHODS
	public override void OnConnectedToMaster()
	{
		if (isConnecting)
		{
			LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");

			if (roomCreated)
			{
				PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
			}
			else
			{
				PhotonNetwork.JoinRandomRoom();
			}
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");

		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);

		loaderAnime.StopLoaderAnimation();

		isConnecting = false;
		controlPanel.SetActive(true);
	}

	public override void OnJoinedRoom()
	{
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");

		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("Joined Room");

			PhotonNetwork.LoadLevel("Room_2");
		}
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Created Room");
	}
	#endregion
}
