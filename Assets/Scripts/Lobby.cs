using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
	[Header("General"), Space]
	[SerializeField] private byte maxPlayersPerRoom = 4;
	[SerializeField] private TMP_Text feedbackText = null;
	[SerializeField] private LoaderAnime loaderAnime = null;

	[Header("Lobby"), Space]
	[SerializeField] private GameObject lobbyGO = null;
	[SerializeField] private GameObject userListPrefab = null;
	[SerializeField] private TMP_InputField nickname = null;
	[SerializeField] private RectTransform userListTransform = null;
	[SerializeField] private Button createRoomBtn = null;
	[SerializeField] private Button JoinRandomRoomBtn = null;

	[Header("Room"), Space]
	[SerializeField] private RectTransform userRoomTransform = null;
	[SerializeField] private GameObject userRoomPrefab = null;
	[SerializeField] private GameObject roomGO = null;
	[SerializeField] private Button leaveBtn = null;
	[SerializeField] private Button startBtn = null;
	#endregion

	#region PRIVATE_FIELDS
	private List<UserList> userList = new List<UserList>();
	private bool isConnecting = false;
    private bool inRoom = false;
    private string gameVersion = "1";
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
		nickname.text = "Player " + Random.Range(1000, 9999);
		Connecting();
	}

    private void Update()
    {
        if (inRoom)
        {
			startBtn.interactable = PhotonNetwork.CurrentRoom.PlayerCount > 1;
		}
    }
    #endregion

    #region PUBLIC_METHODS
    public void CreateRoom()
	{
		LoadingRoom();
		PhotonNetwork.NickName = nickname.text;
		PhotonNetwork.CreateRoom(nickname.text);
	}

	public void JoinRandomRoom()
	{
		LoadingRoom();
		PhotonNetwork.NickName = nickname.text;
		PhotonNetwork.JoinRandomRoom();
	}

	public void LeaveRoom()
    {
		PhotonNetwork.LeaveRoom();
	}

	public void StartGame()
    {
		PhotonNetwork.LoadLevel("Room_2");
    }
	#endregion

	#region PRIVATE_METHODS
	private void Connecting()
    {
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = gameVersion;
	}

	private void LogFeedback(string message)
	{
		if (feedbackText == null)
		{
			return;
		}

		feedbackText.text += System.Environment.NewLine + message;
	}

	private void SwitchLobby(bool goLobby)
	{
		lobbyGO.SetActive(goLobby);
		roomGO.SetActive(!goLobby);
		inRoom = !goLobby;
	}

	private void LoadingRoom()
	{
		feedbackText.text = "";
		isConnecting = true;
		lobbyGO.SetActive(false);

		if (loaderAnime != null)
		{
			loaderAnime.StartLoaderAnimation();
		}
	}

	private void JoinRoom(string roomName)
    {
		PhotonNetwork.NickName = nickname.text;
		PhotonNetwork.JoinRoom(roomName);
	}
	#endregion

	#region OVERRIDE_METHODS
	public override void OnConnectedToMaster()
	{
		LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");

		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;

		createRoomBtn.interactable = true;
		JoinRandomRoomBtn.interactable = true;
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("OnJoinRandomFailed: Next -> Create a new Room");

		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);

		loaderAnime.StopLoaderAnimation();

		isConnecting = false;
		lobbyGO.SetActive(true);
	}

	public override void OnJoinedRoom()
	{
		LogFeedback("OnJoinedRoom with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		loaderAnime.StopLoaderAnimation();

		Player[] players = PhotonNetwork.PlayerList;

		foreach (Transform child in userRoomTransform)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < players.Length; i++)
		{
			Instantiate(userRoomPrefab, userRoomTransform).GetComponent<UserList>().SetUserName(players[i].NickName);
		}

		SwitchLobby(false);
		startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Lobby created");
	}

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (!inRoom)
        {
			for (int i = 0; i < userList.Count; i++)
			{
				Destroy(userList[i].gameObject);
			}
			userList.Clear();

			for (int i = 0; i < roomList.Count; i++)
			{
				if (roomList[i].RemovedFromList)
					continue;

				UserList roomInfo = Instantiate(userListPrefab, userListTransform).GetComponent<UserList>();
				roomInfo.Init(JoinRoom);
				roomInfo.SetUserName(roomList[i].Name);
				userList.Add(roomInfo);
			}
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(userRoomPrefab, userRoomTransform).GetComponent<UserList>().SetUserName(newPlayer.NickName);
	}

	public override void OnLeftRoom()
	{
		SwitchLobby(true);
	}
	#endregion
}
