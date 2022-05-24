using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserList : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private TMP_Text userTxt = null;
    [SerializeField] private Button joinRoomBtn = null;
    #endregion

    #region PUBLIC_METHODS
    public void Init(Action<string> onJoinRoom)
    {
        joinRoomBtn?.onClick.AddListener(() => { onJoinRoom?.Invoke(userTxt.text); });
    }

    public void SetUserName(string name)
    {
        userTxt.text = name;
    }
    #endregion
}
