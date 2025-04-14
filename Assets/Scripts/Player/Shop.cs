using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Character _character;
    [SerializeField] private int _maxUpgradeLevel = 20;
    [SerializeField] private TMP_Text _dpsText;
    [SerializeField] private List<Upgrade> _upgrades;

    private bool _isFree = false;
    private int _secondInMinute = 60;
    
    public void SwitchCheat()
    {
        _isFree = !_isFree;
    }
    
    private void Start()
    {
        foreach (var upgrade in _upgrades)
        {
            upgrade.Initialize(_character);
            upgrade.Button.onClick.AddListener(() => BuyBuff(upgrade));
            upgrade.UpdateUI();
        }
    }

    private void BuyBuff(Upgrade upgrade)
    {
        if (upgrade.Level < _maxUpgradeLevel)
        {
            if (_isFree || _wallet.TrySpendCoins(upgrade.Level * upgrade.CostCoefficient))
            {
                _character.IncreaseStat(upgrade.Name);
                _dpsText.text = "DPM: " + Convert.ToString(_secondInMinute / _character.AttackSpeed * _character.Damage);
                upgrade.IncreaseLevel();
            }
        }
    }
}

[Serializable]
public class Upgrade
{
    [SerializeField] private Upgrades _name;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _statText;
    [SerializeField] private TMP_Text _costText;
    
    private Character _character;
    private int _costCoefficient = 3;
    private Dictionary<Upgrades, Func<string>> _statFormatters;
    
    public Upgrades Name => _name;
    public Button Button => _button;
    public int CostCoefficient => _costCoefficient;
    public int Level { get; private set; } = 1;

    private Action<Upgrade> _onClick;
    
    public void Initialize(Character character)
    {
        _character = character;
        
        _statFormatters = new Dictionary<Upgrades, Func<string>>
        {
            { Upgrades.Damage,      () => $"Damage: {_character.Damage}" },
            { Upgrades.Health,      () => $"Health: {_character.MaxHealth}" },
            { Upgrades.AttackSpeed, () => $"Attack Speed: {_character.AttackSpeed}" }
        };
    }
    
    public void IncreaseLevel()
    {
        Level++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _levelText.text = $"LVL {Level}";
        _statText.text = _statFormatters[_name]();
        _costText.text = $"Increase\n {Level * _costCoefficient}";
    }
}