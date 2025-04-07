using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HealthView : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private RectTransform _bar;
    
    private TMP_Text _text;
    private float _barWidth;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = _character.Health.ToString();
        _barWidth = _bar.sizeDelta.x;
    }

    private void OnEnable()
    {
        _character.HealthChanged += WriteHealth;
    }
    
    private void OnDisable()
    {
        _character.HealthChanged -= WriteHealth;
    }

    private void WriteHealth(int health)
    {
        if (health < 0)
            health = 0;

        _bar.sizeDelta = new Vector2(_barWidth / _character.MaxHealth * _character.Health, _bar.sizeDelta.y);
        _text.text = health.ToString();
    }   
}