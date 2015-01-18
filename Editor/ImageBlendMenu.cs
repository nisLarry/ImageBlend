//----------------------------------------------
//       (The MIT License)
//       工具名稱：ImageBlend 0.0.1(beta)
//       Copyright ©2015 仙化似風(國同學)
//       功能簡介：將兩個資料夾的png圖片依照規則檔設定合成新圖片
//                 並輸出至指定資料夾中
//       依賴類：1.BlendImages.cs 負責處理圖片合成的功能
//               2.GetRuleData.cs 負責將規則來源的字串取出
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public class ImageBlendMenu : EditorWindow {
    [MenuItem("ImageBlend/兩資料夾合成")]
    static void AddWindow()
    {
        //創建視窗
        Rect wr = new Rect(0, 0, 500, 500);
        ImageBlendMenu window = (ImageBlendMenu)EditorWindow.GetWindowWithRect(typeof(ImageBlendMenu), wr, true, "兩資料夾合成圖片");
        window.Show();

    }

    //資料夾的路徑
    private string sourceFolder1Path1;
    private string sourceFolder1Path2;
    private string sourceFolder1Path3;

    //規則設定檔的路徑
    private string ruleFilePath1;
    private string ruleFilePath2;

    //圖檔處理的狀態
    private string makeState = "未處理…";

    //繪制視窗元件
    void OnGUI()
    {

        if (GUILayout.Button("選擇來源資料夾", GUILayout.Width(200)))
        {
            //選擇來源資料夾1
            GetFolderPath(1);
        }

        sourceFolder1Path1 = EditorGUILayout.TextField("來源資料夾:", sourceFolder1Path1);

        if (GUILayout.Button("選擇合成資料夾", GUILayout.Width(200)))
        {
            //選擇合成資料夾
            GetFolderPath(2);
        }

        sourceFolder1Path2 = EditorGUILayout.TextField("合成資料夾:", sourceFolder1Path2);

         if (GUILayout.Button("選擇輸出資料夾", GUILayout.Width(200)))
         {
             //選擇輸出資料夾
             GetFolderPath(3);
         }
          
        sourceFolder1Path3 = EditorGUILayout.TextField("輸出資料夾:", sourceFolder1Path3);

        if (GUILayout.Button("選擇規則檔1", GUILayout.Width(200)))
        {
            //選擇輸出資料夾
            GetFilePath(1);
        }

        ruleFilePath1 = EditorGUILayout.TextField("規則檔1:", ruleFilePath1);

        if (GUILayout.Button("選擇規則檔2", GUILayout.Width(200)))
        {
            //選擇輸出資料夾
            GetFilePath(2);
        }

        ruleFilePath2 = EditorGUILayout.TextField("規則檔2:", ruleFilePath2);

        if (GUILayout.Button("產生圖片", GUILayout.Width(200)))
        {
            MakePNG();
        }

        EditorGUILayout.LabelField("處理狀態 ", makeState);
        this.Repaint();


        if (GUILayout.Button("關閉視窗", GUILayout.Width(200)))
        {
            this.Close();
        }
    }


    void OnInspectorUpdate()
    {
       this.Repaint();
    }


    /// <summary>
    /// 取得資料夾路徑
    /// </summary>
    /// <param name="pathNum"> 路徑編號</param>
    void GetFolderPath(int pathNum)
    {
       switch (pathNum)
       {
           case 1:
               sourceFolder1Path1 = EditorUtility.OpenFolderPanel("從資料夾載入原始png圖片", "", "");
               break;
           case 2:
               sourceFolder1Path2 = EditorUtility.OpenFolderPanel("從資料夾載入要合併的png圖片", "", "");
               break;
           case 3:
               sourceFolder1Path3 = EditorUtility.OpenFolderPanel("選擇要輸出的資料夾", "", "");
               break;
       }
    }
    /// <summary>
    /// 取得檔案路徑
    /// </summary>
    /// <param name="pathNum">路徑編號</param>
    void GetFilePath(int pathNum)
    {
        switch (pathNum)
        {
            case 1:
                ruleFilePath1 = EditorUtility.OpenFilePanel("選擇規則文字檔1","","txt");;
                break;
            case 2:
                ruleFilePath2 = EditorUtility.OpenFilePanel("選擇規則文字檔2","","txt");;
                break;
        }
    }
    /// <summary>
    /// 制作合成圖片
    /// </summary>
    void MakePNG()
    {
        this.ShowNotification(new GUIContent("圖片合成中…請稍候"));
        string[] files1 = GetFilesPath(sourceFolder1Path1);
        Dictionary<string, string> ruleDictionary1 = GetRuleDictionary(ruleFilePath1);
        Dictionary<string, string> ruleDictionary2 = GetRuleDictionary(ruleFilePath2);
        for (int i = 0; i < files1.Length; i++ )
        {
            string fileName = Path.GetFileNameWithoutExtension(files1[i]);
            string file2Name = ruleDictionary2[ruleDictionary1[fileName]];
            Image newImage = BlendImages.MakeNewImages(files1[i], sourceFolder1Path2 + "/" + file2Name + ".png");
            newImage.Save(sourceFolder1Path3 + "/" + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }
        this.RemoveNotification();
        makeState = "處理完成";
        Debug.Log("圖片處理完成！請刷新資料夾！");
    }
    /// <summary>
    /// 將規則檔輸入成dictionary物件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    Dictionary<string, string> GetRuleDictionary(string filePath)
    {
       Dictionary<string, string> ruleDictionary = new Dictionary<string, string>();
       string fileText = GetRuleData.GetText(filePath);
       string[] ruleLines = fileText.Split(';');
       foreach(string ruleLine in ruleLines)
       {
           string[] rules = ruleLine.Trim().Split('=');
           ruleDictionary.Add(rules[0],rules[1]);
       }
       return ruleDictionary;
    }
    /// <summary>
    /// 取得檔案路徑陣列
    /// </summary>
    /// <param name="sourceFolder1Path">資料夾路徑</param>
    /// <returns></returns>
    string[] GetFilesPath(string sourceFolder1Path)
    {
       string[] files = Directory.GetFiles(sourceFolder1Path,"*.png");
       return files;
    }

}
