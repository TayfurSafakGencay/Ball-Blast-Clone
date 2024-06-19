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
    private TextMeshProUGUI _titleText;

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
      
      InitialTitle();
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
      
      PanelTitle(success);
      UpdateBuyFeaturesPanel();
    }

    public void OnLeftButton()
    {
      _page--;
      if (_page < 1)
        _page = _skillTypeCount;

      UpdateBuyFeaturesPanel();
    }

    public void OnRightButton()
    {
      _page++;
      if (_page > _skillTypeCount)
        _page = 1;

      UpdateBuyFeaturesPanel();
    }

    private void InitialTitle()
    {
      int level = SaveSystemManager.LoadLevel();

      _titleText.text = "Level " + level;
      
      UpdateBuyFeaturesPanel();
    }

    private void PanelTitle(bool success)
    {
      int level = SaveSystemManager.LoadLevel();

      if (success)
      {
        _titleText.text = "Level " + level + " Cleared!";
      }
      else
      {
        _titleText.text = "Level " + level + " Failed!";
      }
    }

    private void UpdateBuyFeaturesPanel()
    {
      SkillType skillType = (SkillType)_page;
      SkillStat skillStat = GameManager.Instance.GetSkillStats(skillType);

      _skillNameText.text = skillType.ToString();

      if (GameManager.Instance.GetPlayerCoin() >= skillStat.Cost)
      {
        _priceText.text = "<sprite index=0> " + "<color=white>" + skillStat.Cost + "</color>";
        _buyButton.interactable = true;
      }
      else
      {
        _priceText.text = "<sprite index=0> " + "<color=red>" + skillStat.Cost + "</color>";
        _buyButton.interactable = false;
      }

      _upgradingStatText.text = skillType == SkillType.AttackSpeed
        ? skillStat.Stat.ToString("f2") + " -> " + "<color=green>" + (skillStat.Stat - 0.05f).ToString("f2") + "</color>"
        : skillStat.Stat + " -> " + "<color=green>" + (skillStat.Stat + 1) + "</color>";
    }

    public async void OnBuyFeature()
    {
      SkillType skillType = (SkillType)_page;

      GameManager.Instance.UpgradeSkillInvoke(skillType);

      await GameManager.Delay(0.05f);
      UpdateBuyFeaturesPanel();
    }
  }
}