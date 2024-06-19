using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class InnerGamePanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _coinText;

    [SerializeField]
    private TextMeshProUGUI _levelText;

    [SerializeField]
    private Slider _levelSlider;

    private void Start()
    {
      GameManager.Instance.TotalMoneyChanged += UpdateText;
      GameManager.GameStarted += GameStarted;
      GameManager.MeteorDestroyed += UpdateLevelBar;
      
      _levelText.text = "";
      
      UpdateText(GameManager.Instance.Money);
    }

    private void GameStarted()
    {
      _levelText.text = "Level " + GameManager.Instance.GetLevelStats().Level;

      _levelSlider.value = 0;
    }

    private void UpdateText(int money)
    {
      _coinText.text = "<sprite index=0> " + money;
    }

    private void UpdateLevelBar(int destroyedCount, int totalMeteor)
    {
      float targetValue = (float)destroyedCount / totalMeteor;
      
      _levelSlider.DOValue(targetValue, 0.25f).SetEase(Ease.InOutQuad);
    }
  }
}