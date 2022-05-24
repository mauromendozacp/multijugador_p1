using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class Room : MonoBehaviourPunCallbacks
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button leaveBtn = null;
    [SerializeField] private Button startBtn = null;
    [SerializeField] private RectTransform userListTransform = null;
    [SerializeField] private GameObject userPrefab = null;
    #endregion

    #region PRIVATE_FIELDS
    private List<UserList> userList = new List<UserList>();
    #endregion

    #region PUBLIC_METHODS
    public void EnterRoom()
    {
        GameObject user = PhotonNetwork.Instantiate(userPrefab.name, Vector3.zero, Quaternion.identity);
        user.transform.parent = userListTransform;

        user.GetComponent<UserList>().SetUserName(PhotonNetwork.LocalPlayer.NickName);
    }
    #endregion

    #region OVERRIDE_METHODS
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateListRoom(roomList);
    }
    #endregion

    #region PRIVATE_METHODS
    private void UpdateListRoom(List<RoomInfo> roomList)
    {
        for (int i = 0; i < userList.Count; i++)
        {
            Destroy(userList[i].gameObject);
        }
        userList.Clear();

        for (int i = 0; i < roomList.Count; i++)
        {
            UserList roomInfo = Instantiate(userPrefab, userListTransform).GetComponent<UserList>();
            roomInfo.SetUserName(roomInfo.name);
            userList.Add(roomInfo);
        }
    }
    #endregion
}
