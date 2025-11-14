using UnityEngine;

public class LobbyMainTest : MonoBehaviour
{
    
    void Start()
    {
        ConnectToMasterServer();
    }

    private void ConnectToMasterServer()
    {
        MasterServerPun2Manager.instance.Init();
    }
}
