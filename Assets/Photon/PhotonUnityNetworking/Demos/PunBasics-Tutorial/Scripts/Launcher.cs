using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649

	public class Launcher : MonoBehaviourPunCallbacks
    {
		#region Private Serializable Fields
		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		[SerializeField]
		private GameObject controlPanel;

		[Tooltip("The Ui Text to inform the user about the connection progress")]
		[SerializeField]
		private Text feedbackText;

		[Tooltip("The maximum number of players per room")]
		[SerializeField]
		private byte maxPlayersPerRoom = 4;

		[Tooltip("The UI Loader Anime")]
		[SerializeField]
		private LoaderAnime loaderAnime;
		#endregion

		#region Private Fields
		private bool isConnecting = false;
		private bool roomCreated = false;
		private string gameVersion = "1";
		#endregion

		#region MonoBehaviour CallBacks
		void Awake()
		{
			if (loaderAnime == null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.",this);
			}

			PhotonNetwork.AutomaticallySyncScene = true;
		}
		#endregion

		#region Public Methods
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

		private void LogFeedback(string message)
		{
			if (feedbackText == null) {
				return;
			}

			feedbackText.text += System.Environment.NewLine+message;
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

        #region MonoBehaviourPunCallbacks CallBacks
        public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
				Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

                if (roomCreated)
                {
					PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
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
			Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			LogFeedback("<Color=Red>OnDisconnected</Color> "+cause);
			Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

			loaderAnime.StopLoaderAnimation();

			isConnecting = false;
			controlPanel.SetActive(true);
		}

		public override void OnJoinedRoom()
		{
			LogFeedback("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)");
			Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("We load the 'Room for 1' ");

				PhotonNetwork.LoadLevel("Room");
			}
		}

        public override void OnCreatedRoom()
        {
			Debug.Log("We load the 'Room'");
		}
        #endregion

    }
}