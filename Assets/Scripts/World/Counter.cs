using System;
using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private int _timeForDoor;

    private int _oneSecond = 1;

    public event Action<int> CounterChanged; 
    public event Action TimeEnded;

    private void OnEnable()
    {
        _roomManager.RoomStarted += BeginCount;
    }

    private void OnDisable()
    {
        _roomManager.RoomStarted -= BeginCount;
        StopAllCoroutines();
    }

    private void BeginCount()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        int counter = _timeForDoor;
        
        while (counter > 0)
        {
            counter--;
            CounterChanged?.Invoke(counter);
            yield return new WaitForSeconds(_oneSecond);
        }
        
        TimeEnded?.Invoke();
    }
}
