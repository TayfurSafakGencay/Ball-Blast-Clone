using DG.Tweening;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private void Start()
    { 
      transform.DOLocalMoveX(5f, 12f)
            .SetEase(Ease.Linear).SetLoops(-1) 
            .OnComplete(() =>
            {
              transform.localPosition = new Vector3(-5, transform.localPosition.y);
            });
    }
}
