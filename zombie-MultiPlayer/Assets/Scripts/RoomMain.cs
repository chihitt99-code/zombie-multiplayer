using System;
using Photon.Pun;
using UnityEngine;

public class RoomMain : MonoBehaviour
{
    public UIPlayerList uiPlayerList;

    private void Awake() 
    {
 
    }

    private void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer);
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        
        //룸에 대한 정보를 받아올수는 없을까 ?
    }
 
}
