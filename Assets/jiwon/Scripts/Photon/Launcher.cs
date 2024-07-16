using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private Transform playerListContent;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private TMP_Dropdown stageDropdown;

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //연결 끊기
        PhotonNetwork.Disconnect();

        // 서버 연결
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 연결 끊겼을 때 호출
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnecting to Server");
    }

    /// <summary>
    /// 서버 연결 콜백 함수
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // 로비 연결
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// 로비 연결 콜백 함수
    /// </summary>
    public override void OnJoinedLobby()
    {
        // 로딩 완료 후 title UI 표시
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    /// <summary>
    /// 방 만들기
    /// </summary>
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    /// <summary>
    /// 방 입장 콜백 함수
    /// </summary>
    public override void OnJoinedRoom()
    {
        // 방에 들어가면 room UI 표시
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerList();

        // 마스터 클라이언트만 게임 시작 버트 표시
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        stageDropdown.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// 플레이어 입장 시 콜백 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    /// <summary>
    /// 플레이어 퇴장 시 콜백 함수
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            // 플레이어 리스트 프리펩 생성 및 플레이어 정보 초기화
            Instantiate(playerListItemPrefab, playerListContent).GetComponentInChildren<PlayerListItem>().SetUp(players[i]);
        }
    }

    /// <summary>
    /// 마스터 클라이언트가 방을 나갔을 때, 마스터 클라이언트 교체
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        stageDropdown.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// 방 만들기 실패 콜백 함수
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 에러 UI 표시
        errorText.text = "Room Creation Failed: " + message;
        Debug.Log("Room Creation Failed: " + message);
        MenuManager.Instance.OpenMenu("error");
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        Hashtable customProperty = new Hashtable();
        customProperty["IsGameStarted"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperty);
        PhotonNetwork.LoadLevel(1);
        //PhotonNetwork.LoadLevel("Stage" + stageDropdown.value);
    }

    /// <summary>
    /// 방 나가기
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    /// <summary>
    /// 방 입장
    /// </summary>
    /// <param name="info"></param>
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    /// <summary>
    /// 방 떠나기 콜백 함수
    /// </summary>
    public override void OnLeftRoom()
    {
        // 방 나가면 title UI 표시
        MenuManager.Instance.OpenMenu("title");
        MenuManager.Instance.OpenMenu("loading");
    }

    /// <summary>
    /// 방 리스트 초기화 콜백 함수
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 방 초기화
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        // 방 목록 업데이트
        foreach (var info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }

        // 입장 가능한 방 표시
        foreach (var info in cachedRoomList.Values)
        {
            if (info.CustomProperties != null &&
                info.CustomProperties.ContainsKey("IsGameStarted") &&
                (bool)info.CustomProperties["IsGameStarted"])
            {
                Debug.Log("true");
                continue;
            }

            Debug.Log("false");
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(info);
        }
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
