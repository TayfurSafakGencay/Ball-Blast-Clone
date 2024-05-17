using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class InnerGamePanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _coinText;

    private int _playerPrefMoney;

    private void Start()
    {
      GameManager.Instance.CollectedMoneyForThisTurn += UpdateText;
      GameManager.GameFinished += UpdatePlayerPrefMoney;
      GameManager.GameStarted += UpdatePlayerPrefMoney;
      
      _playerPrefMoney = GameManager.Instance.GetPlayerCoin();
      UpdateText(0);
    }

    private void UpdatePlayerPrefMoney(bool success)
    {
      _playerPrefMoney = GameManager.Instance.GetPlayerCoin();
    }

    private void UpdatePlayerPrefMoney()
    {
      _playerPrefMoney = GameManager.Instance.GetPlayerCoin();
    }

    private void UpdateText(int value)
    {
      int money = value + _playerPrefMoney;
      
      _coinText.text = "<sprite index=0> " + money;
    }
  }
}