using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

namespace WJH
{
    public class CreateNode : MonoBehaviour
    {
        //标准二维码生成
        #region

        public RawImage codeRawImage;
        private Texture2D Qrcode;
        //private bool isCreated = false;
        //public void ShowQrCode()
        //{
        //    if (isCreated)
        //        codeRawImage.gameObject.SetActive(true);
        //    else
        //        Debug.LogWarning("二维码未创建，数据返回有延迟！！！");
        //}


        public void GameQrCodePosOnGame()
        {
            codeRawImage.transform.GetComponent<QrCodeConfig>().GetConfigOnGame();
        }



        public void CreatCode(string url)
        {
            Texture2D texture2d = Resources.Load("Texture/QrCodeBg") as Texture2D;
            Color32[] testColors = texture2d.GetPixels32();

            Qrcode = new Texture2D(256, 256);
            if (!string.IsNullOrEmpty(url))
            {
                Color32[] color32 = CreatQrCode(url, Qrcode.width, Qrcode.height);
                for (int i = 0; i < color32.Length; i++)
                {
                    if (color32[i].r == 0)
                    {
                        color32[i] = testColors[i];
                        color32[i] = new Color32(0, 0, 0, 255);
                        //color32[i].a = 0;
                    }
                    else
                    {
                        color32[i] = new Color32(255, 255, 255, 255);
                        //   color32[i] = new Color32(212, 140, 31, 200);
                    }
                }
                Qrcode.SetPixels32(color32);
                Qrcode.Apply();
            }
            codeRawImage.texture = Qrcode;
            //isCreated = true;
            codeRawImage.gameObject.SetActive(true);
        }

        private static Color32[] CreatQrCode(string textForEncoding, int width, int height)
        {

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                }
            };
            writer.Options.Margin = 1;


            return writer.Write(textForEncoding);
        }

        #endregion

    }

}