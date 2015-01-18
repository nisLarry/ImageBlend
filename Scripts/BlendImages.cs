//----------------------------------------------
//       (The MIT License)
//       Copyright ©2015 仙化似風(國同學)
//       功能簡介：負責處理圖片合成的功能
//       開發主頁：https://github.com/nisLarry/ImageBlend
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Drawing;
using System;

static public class BlendImages 
{

 static public Image MakeNewImages(string imgPath1, string imgPath2)
   {
     try
     {
         Image img1 = Image.FromFile(imgPath1);
         Image img2 = Image.FromFile(imgPath2);
         System.Drawing.Graphics _graphics = System.Drawing.Graphics.FromImage(img1);
         _graphics.DrawImage(img2, 0, 0);
         return img1;
     }
     catch(Exception e)
     {
         Debug.LogError("合成圖片時發生了錯誤："+e.Message);
         return null;
     }
        
   }
}
