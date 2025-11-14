using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class MasterServerPun2Manager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public static MasterServerPun2Manager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        PhotonNetwork.AutomaticallySyncScene = true; // 자동으로 씬을 동기화
        
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 접속 했습니다.");
        
    }
}
