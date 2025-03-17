using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HealthView : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = _character.Health.ToString();
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

        _text.text = health.ToString();
    }
}