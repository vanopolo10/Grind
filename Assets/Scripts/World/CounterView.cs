using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CounterView : MonoBehaviour
{
    private Counter _counter;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void SetCounter(Counter counter)
    {
        _counter = counter;
        _counter.CounterChanged += WriteCounter;
    }

    public void Unsubscribe()
    {
        _counter.CounterChanged -= WriteCounter;
    }

    private void WriteCounter(int counter)
    {
        print("Смена счетчика");
        _text.text = counter.ToString();
    }
}
