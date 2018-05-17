using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class ShoeAnimation : MonoBehaviour
{
    //Image _ShoeImage;
    //public float time = 1f;

    //void OnEnable()
    //{
    //    //_ShoeImage = this.GetComponent<Image>();
    //    //InistializeSprites();
    //}

    //void OnDisable()
    //{
    //    mytween.Complete();
    //    StopCoroutine("_ChangeTheSprite");
    //}

    //Tween mytween;
    //private string _path = "Texture/Shoe";
    //private Sprite[] _sprites;
    //private int _maxCount;
    //private float _deltaTime;

    //private void InistializeSprites()
    //{
    //    Debug.Log("Function InistializeSprites(): _path is ->" + _path);
    //    _sprites = Resources.LoadAll<Sprite>(_path);
    //    _maxCount = _sprites.Length;
    //    _deltaTime = time / _maxCount;

    //    _ShoeImage.color = new Color(1, 1, 1, 0);
    //    mytween = _ShoeImage.DOFade(1, 2f);
    //    StartCoroutine("_ChangeTheSprite");

    //}


    //IEnumerator _ChangeTheSprite()
    //{
    //    for (int i = 0; i < _maxCount; ++i)
    //    {
    //        if (i == _maxCount - 1)
    //            i = 0;
    //        _ShoeImage.sprite = _sprites[i];
    //        yield return new WaitForSeconds(_deltaTime);
    //    }
    //}
}
