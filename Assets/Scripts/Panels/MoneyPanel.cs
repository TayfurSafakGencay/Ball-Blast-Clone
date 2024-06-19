using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Panels
{
  public class MoneyPanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshPro _textObject;

    [SerializeField]
    private Transform _textContainer;

    private readonly Queue<TextMeshPro> _textObjectQueue = new();
    private void Start()
    {
      GameManager.Instance.CollectedMoneyWithCoefficient += CollectedMoney;
    }

    private void CollectedMoney(int value, Vector3 position)
    {
      if (_textObjectQueue.Count == 0)
      {
        TextMeshPro newText = Instantiate(_textObject, position, quaternion.identity, _textContainer);

        MoneyAnimation(value, position, newText);
      }
      else
      {
        TextMeshPro text = _textObjectQueue.Dequeue();
        MoneyAnimation(value, position, text);
      }
    }

    private void MoneyAnimation(int value, Vector3 position, TextMeshPro moneyText)
    {
      moneyText.gameObject.SetActive(true);
      moneyText.transform.position = position;
      
      moneyText.text = "+" + value + "$";
      
      moneyText.transform.DOMoveY(position.y + Random.Range(1.5f, 3f), 1.5f);
      moneyText.DOFade(0, 2f).OnComplete(() =>
      {
        moneyText.gameObject.SetActive(false);
        moneyText.DOFade(1, 0f);
        _textObjectQueue.Enqueue(moneyText);
      });
    }
  }
}