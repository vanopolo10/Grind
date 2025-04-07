using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FreeButton : MonoBehaviour
{
    [SerializeField] private Shop _shop;

    private Button _button;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() => _shop.SwitchCheat());
    }
}
