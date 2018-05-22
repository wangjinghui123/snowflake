using UnityEngine;
using System.Collections;
using EasyARTool;
using UnityEngine.UI;
using DG.Tweening;
public class QrCodeConfig : MonoBehaviour
{
    public EasyConfig config;
    public RawImage qrCode;


    void OnEnable()
    {
      //  GetConfigStart();
    }
    void OnDisable()
    {
        //SetConfig();
    }
    private void SetConfig()
    {
        Vector2 size = Vector2.zero;
        size = qrCode.rectTransform.sizeDelta;
        config.SetInt("Qr_Width_int", (int)size.x);
        config.SetInt("Qr_Height_int", (int)size.y);
        Vector3 pos = Vector3.zero;
        pos = qrCode.rectTransform.anchoredPosition3D;
        config.SetInt("Qr_Position_x", (int)pos.x);
        config.SetInt("Qr_Position_y", (int)pos.y);
    }

    public void GetConfigStart()
    {
        Vector2 size = Vector2.zero;
        size.x = config.GetInt("Qr_Width_int");
        size.y = config.GetInt("Qr_Height_int");
        qrCode.rectTransform.sizeDelta = size;

        Vector3 pos = Vector3.zero;
        pos.x = config.GetInt("Qr_Position_x");
        pos.y = config.GetInt("Qr_Position_y");
        qrCode.rectTransform.anchoredPosition3D = pos;
    }


    private void SetConfigOnGame()
    {
        Vector2 size = Vector2.zero;
        size = qrCode.rectTransform.sizeDelta;
        config.SetInt("Qr_Width_int_game", (int)size.x);
        config.SetInt("Qr_Height_int_game", (int)size.y);
        Vector3 pos = Vector3.zero;
        pos = qrCode.rectTransform.anchoredPosition3D;
        config.SetInt("Qr_Position_x_game", (int)pos.x);
        config.SetInt("Qr_Position_y_game", (int)pos.y);
    }

    public void GetConfigOnGame()
    {



        Vector2 size = Vector2.zero;
        size.x = config.GetInt("Qr_Width_int_game");
        size.y = config.GetInt("Qr_Height_int_game");
        qrCode.rectTransform.sizeDelta = size;

        Vector3 pos = Vector3.zero;
        pos.x = config.GetInt("Qr_Position_x_game");
        pos.y = config.GetInt("Qr_Position_y_game");

        qrCode.rectTransform.DOAnchorPos3D(pos, 0.5f);//.anchoredPosition3D = pos;


    }



}
