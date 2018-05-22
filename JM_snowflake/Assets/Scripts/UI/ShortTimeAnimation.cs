using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

/// <summary>
/// 游戏正计时类
/// </summary>
public class ShortTimeAnimation : MonoBehaviour
{
    public EventHandler readyTimeDoneEvent;
    public GameObject runTime;
    public int readyTime = 10;

    [SerializeField]
    private Image _number;
    [SerializeField]
    private Image _circle;
    [SerializeField]
    private Transform _tishiyu;
    [SerializeField]
    private Transform _maskblack;
    [SerializeField]
    private List<Sprite> _lastSpriteList = new List<Sprite>();

    private Tween _myTween;

    // Use this for initialization
    void OnEnable()
    {
        _ChangeTheNumber();
    }
    private void _ChangeTheNumber()
    {
        _number.enabled = true;
        _circle.enabled = true;
        _tishiyu.gameObject.SetActive(true);
        StartCoroutine(_circlechange());
    }

    private IEnumerator _circlechange()
    {
        int i = readyTime;
        if (i > 10)
            i = 10;
        while (i >= 0 && i <= _lastSpriteList.Count)
        {
            if (i >= 1)
            {
                _number.rectTransform.sizeDelta = new Vector2(-100f, -100f);
                _number.rectTransform.DOSizeDelta(new Vector2(0, 0), 0.3f);
                _number.sprite = _lastSpriteList[i];
                _circle.fillAmount = 1f;
                _circle.DOFillAmount(0, 1f);
                yield return new WaitForSeconds(1f);
                if (i == 1)
                {
                    _tishiyu.gameObject.SetActive(false);
                    _maskblack.gameObject.SetActive(false);
                }

            }
            else
            {
                _circle.enabled = false;
                _number.sprite = _lastSpriteList[i];
                _number.rectTransform.sizeDelta = new Vector2(550f, 550f);
                _number.rectTransform.DOSizeDelta(new Vector2(50, 50), 0.3f);
                yield return new WaitForSeconds(1f);
                _number.enabled = false;
            }
            --i;
            if (i==0)
            {
                runTime.SetActive(true);
            }
            if (i == 0 && readyTimeDoneEvent != null)
            {
                readyTimeDoneEvent(this, null);
               
            }
               
        }

    }

}
