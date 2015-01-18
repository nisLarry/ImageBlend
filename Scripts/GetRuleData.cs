//----------------------------------------------
//       (The MIT License)
//       Copyright ©2015 仙化似風(國同學)
//       功能簡介：負責將規則來源的字串取出
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.IO;

static public class GetRuleData 
{
    static public string GetText(string rulefile)
    {
        string ruleText = "";
        try
        {
            using (StreamReader sr = new StreamReader(rulefile))
            {
                ruleText = sr.ReadToEnd();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("讀取文字檔發生錯誤：" + e.Message);
            
        }
        return ruleText;
    }
   

	
}
