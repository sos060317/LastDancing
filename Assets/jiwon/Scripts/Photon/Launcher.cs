using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 서버 연결
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버 연결 콜백 함수
    public override void OnConnectedToMaster()
    {
        // 로비 연결
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 로비 연결 콜백 함수
    public override void OnJoinedLobby()
    {
        // 로딩 완료 후 title UI 표시
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000"); 
    }

    // 방 만들기
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    // 방 입장 콜백 함수
    public override void OnJoinedRoom()
    {
        // 방에 들어가면 room UI 표시
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            // 플레이어 리스트 프리펩 생성 및 플레이어 정보 초기화
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        // 마스터 클라이언트만 게임 시작 버트 표시
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    // 방 만들기 실패 콜백 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 에러 UI 표시
        errorText.text = "Room Creation Failed: " + message;
        Debug.Log("Room Creation Failed: " + message);
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    // 방 나가기
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    // 방 입장
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    // 방 떠나기 콜백 함수
    public override void OnLeftRoom()
    {
        // 방 나가면 title UI 표시
        MenuManager.Instance.OpenMenu("title");
        MenuManager.Instance.OpenMenu("loading");
    }

    // 방 리스트 초기화 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 방 초기화
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        // 입장 가능한 방 표시
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    // 플레이어 방 입장 콜백 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 플레이어 리스트 프리펩 생성 및 플레이어 정보 초기화
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
