using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UINicknameView nicknameView;
    public UILoading uiLoading;
    public UIScrollView uiScrollView;

    public Button createRoomButton;
    public Button leaveRoomButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiScrollView.Init();
        AddEventListeners();
        
        ConnectToMasterServer();
       
        createRoomButton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.CreateRoom();
        });

        leaveRoomButton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.LeaveRoom();
        });

        
        nicknameView.onClickSubmit = (nickname) =>
        {
            if (string.IsNullOrEmpty(nickname))
            {
                Debug.Log("nickname is empty");
            }
            else
            {
                Debug.Log($"nickname : {nickname}");
                Pun2Manager.instance.SetNickname(nickname);
                uiScrollView.Show();
                createRoomButton.gameObject.SetActive(true);
                nicknameView.gameObject.SetActive(false);
                
            }
        };
    }
    
    private void ConnectToMasterServer()
    {
        uiLoading.Show();
        Pun2Manager.instance.Init();
        
    }
    private void AddEventListeners()
    {
        
        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnConnectedToMaster, (eventType) =>
        {
            Debug.Log($"AddEventListeners:{(EventEnums.EventType)eventType}");
            uiLoading.Hide();
            nicknameView.gameObject.SetActive(string.IsNullOrEmpty(PhotonNetwork.NickName));
        });
        
        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnJoinedLobby, (eventType) =>
        {
            Debug.Log($"AddEventListeners:{(EventEnums.EventType)eventType}");
            uiLoading.Hide();
            
            if(!string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                uiScrollView.Show();
                createRoomButton.gameObject.SetActive(true);
                leaveRoomButton.gameObject.SetActive(false);
            }
            
        });
        
        EventDispatcher.instance.AddEventHandler((int)EventEnums.EventType.OnJoinedRoom, (eventType) =>
        {
            Debug.Log($"AddEventListeners:{(EventEnums.EventType)eventType}");
            leaveRoomButton.gameObject.SetActive(true);
            
            uiScrollView.Hide();
            createRoomButton.gameObject.SetActive(false);
        });
        

    }
    
    

}
