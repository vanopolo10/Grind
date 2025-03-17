using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Character _character;
    [SerializeField] private int _maxUpgradeLevel;
    
    [SerializeField] private Button _buttonAttackBuff;
    [SerializeField] private Button _buttonHealthBuff;
    [SerializeField] private Button _buttonAttackSpeedBuff;

    private Upgrade _attackUpgrade = new(Upgrades.Attack);
    private Upgrade _healthUpgrade = new(Upgrades.Health);
    private Upgrade _attackSpeedUpgrade = new(Upgrades.AttackSpeed);

    private void Start()
    {
        _buttonAttackBuff.onClick.AddListener(() => BuyBuff(_attackUpgrade));
        _buttonHealthBuff.onClick.AddListener(() => BuyBuff(_healthUpgrade));
        _buttonAttackSpeedBuff.onClick.AddListener(() => BuyBuff(_attackSpeedUpgrade));
    }

    private void BuyBuff(Upgrade upgrade)
    {
        if (upgrade.Level < _maxUpgradeLevel && _wallet.TrySpendCoins(upgrade.Level + 1))
        {
            upgrade.IncreaseLevel();
            _character.IncreaseStat(upgrade.Name);
        }
    }
}

public class Upgrade
{
    public Upgrade(Upgrades name)
    {
        Name = name;
    }
    
    public Upgrades Name { get; }
    public int Level { get; private set; } 

    public void IncreaseLevel()
    {
        Level++;
    }
}