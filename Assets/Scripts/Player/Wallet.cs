using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int _money;
    
    public event Action<int> MoneyChanged;
    
    public void SubscribeEnemy(Enemy enemy)
    {
        enemy.Died += GiveReward;
    }
    
    public bool TrySpendCoins(int amount)
    {
        if (HasEnoughCoins(amount))
        {
            _money -= amount;
            MoneyChanged?.Invoke(_money);
            
            return true;
        }

        return false;
    }
    
    private bool HasEnoughCoins(int amount)
    {
        return _money >= amount;
    }

    private void GiveReward(Enemy enemy, int reward)
    {
        if (reward > 0) 
            ChangeMoney(reward);
        
        enemy.Died -= GiveReward;
    }

    private void ChangeMoney(int money)
    {
        _money += money;
        MoneyChanged?.Invoke(_money);
    }
}
