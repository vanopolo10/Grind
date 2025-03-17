using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WalletView : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _wallet.MoneyChanged += WriteWallet;
    }
    
    private void OnDisable()
    {
        _wallet.MoneyChanged -= WriteWallet;
    }

    private void WriteWallet(int money)
    {
        _text.text = money.ToString();
    }
}
