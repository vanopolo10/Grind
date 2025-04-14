using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Character _player;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private List<RoomManager> _rooms;

    [SerializeField] private Button _endBattleButton;
    
    private int _activeRoomID;
    private RoomManager _activeRoom;

    private void Start()
    {
        SetActiveRoom();
        LaunchRoom();
        _endBattleButton.onClick.AddListener(EndBattle);
    }
    
    private void LaunchRoom()
    {
        _activeRoom.enabled = true;
        _activeRoom.RoomEnded += ChangeRoom;
        _activeRoom.Play(_player, _wallet);
    }

    private void EndBattle()
    {
        _activeRoom.EndBattle();
    }
    
    private void ChangeRoom()
    {
        _activeRoom.RoomEnded -= ChangeRoom;
        _activeRoom.enabled = false;
        
        if (_activeRoomID < _rooms.Count - 1)
            _activeRoomID++;
        else
            return;
        
        _camera.IncreaseTopEdge();
        
        SetActiveRoom();
        LaunchRoom();
    }

    private void SetActiveRoom()
    {
        foreach (var room in _rooms)
        {
            if (_rooms.IndexOf(room) == _activeRoomID)
                _activeRoom = room; 
            else
                room.enabled = false;
        }
    }
}
