using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private CounterView _counterView;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Character _player;
    [SerializeField] private List<RoomManager> _rooms;

    private int _activeRoomID = 0;
    private RoomManager _activeRoom;

    private void Start()
    {
        SetActiveRoom();
        LaunchRoom();
    }

    private void LaunchRoom()
    {
        _counterView.SetCounter(_activeRoom.GetCounter());

        _activeRoom.enabled = true;
        _activeRoom.RoomEnded += ChangeRoom;
        _activeRoom.Play(_player);
        
        (Vector2 leftTopCorner, Vector2 rightBottomCorner) = _activeRoom.GetCorners();
        _camera.SetCorners(leftTopCorner, rightBottomCorner);
    }

    private void ChangeRoom()
    {
        _counterView.Unsubscribe();
        
        _activeRoom.RoomEnded -= ChangeRoom;
        _activeRoom.enabled = false;
        
        if (_activeRoomID < _rooms.Count)
            _activeRoomID++;
        else
            Application.Quit();
        
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
