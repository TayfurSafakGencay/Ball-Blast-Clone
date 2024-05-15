using Enum;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vo;

namespace Panels
{
  public class EndGamePanel : MonoBehaviour
  {
    private int _page;

    private int _skillTypeCount;

    [SerializeField]
    private TextMeshProUGUI _skillNameText;

    [SerializeField]
    private TextMeshProUGUI _upgradingStatText;

    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private Button _buyButton;

    private void Awake()
    {
      _skillTypeCount = System.Enum.GetNames(typeof(SkillType)).Length;
    }

    private void Start()
    {
      GameManager.GameFinished += OpenPanel;
      GameManager.GameStarted += ClosePanel;
    }

    public void OnStartGame()
    {
      GameManager.Instance.StartGame();
    }

    private void OnEnable()
    {
      _page = 1;
    }

    private void ClosePanel()
    {
      gameObject.SetActive(false);
    }
    
    private void OpenPanel(bool success)
    {
      gameObject.SetActive(true);
    }

    public void OnLeftButton()
    {
      _page--;
      if (_page < 1)
        _page = _skillTypeCount;

      OpenPage();
    }

    public void OnRightButton()
    {
      _page++;
      if (_page > _skillTypeCount)
        _page = 1;

      OpenPage();
    }

    private void OpenPage()
    {
      SkillType skillType = (SkillType)_page;
      SkillStat skillStat = GameManager.Instance.GetSkillStats(skillType);

      _skillNameText.text = skillType.ToString();

      if (GameManager.Instance.GetPlayerCoin() >= skillStat.Cost)
      {
        _priceText.text = "<color=white>" + skillStat.Cost + "</color>";
        _buyButton.interactable = true;
      }
      else
      {
        _priceText.text = "<color=red>" + skillStat.Cost + "</color>";
        _buyButton.interactable = false;
      }

      _upgradingStatText.text = skillType == SkillType.AttackSpeed
        ? skillStat.Stat + " -> " + "<color=green>" + (skillStat.Stat - 0.01f) + "</color>"
        : skillStat.Stat + " -> " + "<color=green>" + (skillStat.Stat + 1) + "</color>";
    }
  }
}