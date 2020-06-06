// Manual https://docs.google.com/document/d/1s9Rj8qpaVihMYYPUzgfTG2DK7ZnapthX8sTVnGAga5A/

using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class RemoteLogger : MonoBehaviour
{
    public static RemoteLogger instance;

    static bool isEnable = false;

    const string deviceId = "?deviceId=";
    //const string p0 = "&p0=";
    const string p = "&p";
    const string equal = "=";

    int uniqueid;
    int tabid;
    string tabName;
    string [] tabNames = new string[] { "gameStats0", "gameStats1", "gameStats2", "gameStats3", "gameStats4", "gameStats5", "gameStats6", "gameStats7", "gameStats8","gameStats9", "gameStats10", "gameStats11", "gameStats12" };

    //List<string> tabNames = Instantiate("gameStats0", "gameStats1", "gameStats2"); 

    int m_card;
    float m_time;


    public static void Enable()
    {
        if (isEnable) return;

        isEnable = true;

        PlayerPrefs.SetInt("remoteLogger", 1);

        Debug.Log("Remote Logger - Enabled");

#if UNITY_5
        Application.logMessageReceived += instance.HandleLog;
#else
        Application.RegisterLogCallback(instance.HandleLog);
#endif

        instance.StartCoroutine(instance.WaitForRequest());
    }

    public static void Disable()
    {
        isEnable = false;

        PlayerPrefs.SetInt("remoteLogger", 0);

#if UNITY_5
        Application.logMessageReceived -= instance.HandleLog;
#else
        Application.RegisterLogCallback(null);
#endif

        instance.StopAllCoroutines();
        Debug.Log("Remote Logger - Disabled");
    }

    public string googleScriptUrl = "https://script.google.com/macros/s/AKfycbyOyb5RTbnMXd3Yijsij3bg3DPyLlrlk9CY2VYz2PmHRwz6bqag/exec";//"https://script.google.com/macros/s/AKfycbyGmLruI2TSRFI_D2ndEhV_KVRzbe0Rg5g3-PhX01depOGH7Nw1/exec";

    Queue<string> logs = new Queue<string>();
    //const int maxParams = 10;

    void Awake()
    {
        instance = this;

        if (string.IsNullOrEmpty(googleScriptUrl)) Debug.LogError("Remote Logger - Error: Google Script Url is empty");

        // if (Debug.isDebugBuild) RemoteLogger.Enable();

        // if (DateTime.Now.Year == 1985) RemoteLogger.Enable();
        // else if (DateTime.Now.Year == 1990) RemoteLogger.Disable();

        if (PlayerPrefs.GetInt("remoteLogger", 0) == 1) RemoteLogger.Enable();
        if (Application.isEditor)
            uniqueid = 0;
        else
            uniqueid = (int)UnityEngine.Random.Range(1f, 100000f);
        tabid = (int)UnityEngine.Random.Range(1f, 10f);
        tabName = tabNames[tabid]; 
    }

    const string warning = "Warning: ";
    const string error = "Error: ";
    const string exception = "Exception: ";
    const string n = "\n";

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log) logs.Enqueue(logString);
        else if (type == LogType.Warning) logs.Enqueue(warning + logString);
        else if (type == LogType.Error) logs.Enqueue(error + logString + n + stackTrace);
        else if (type == LogType.Exception) logs.Enqueue(exception + logString + n + stackTrace);
    }

    IEnumerator WaitForRequest()
    {
        while (true)
        {
            if (logs.Count > 0)
            {
                int count = logs.Count; //> maxParams ? maxParams : logs.Count;

                var url = new StringBuilder(googleScriptUrl).Append(deviceId).Append(tabName);//SystemInfo.deviceUniqueIdentifier);
                //for (int i = 0; i < count; ++i)
                //{
                //    url.Append(p).Append(i).Append(equal).Append(UrlEncode(logs.Dequeue()));
                //}
                url.Append(p).Append(0).Append(equal).Append(UrlEncode(logs.Dequeue()));

                var www = new WWW(url.ToString());
                yield return www;

                www.Dispose();
            }

            yield return null;
        }
    }

    public void SetCardData(string phase, int round, int card, float time)
    {
        Enable();
        Debug.Log(uniqueid.ToString() + ",Game" + phase + "," + round + "," + card.ToString() + "," + time.ToString());
    }

    public void SetQuestionnaireData(int qNum, int choice, float time)
    {
        Enable();
        Debug.Log(uniqueid.ToString() + ",Q," + qNum + "," + choice + "," + time.ToString());
    }

    public void SetFeedbackData(int qNum, int choice, float time)
    {
        Enable();
        Debug.Log(uniqueid.ToString() + ",F," + qNum + "," + choice + "," + time.ToString());
    }

    public void SetEndQuestionnaireData(float time)
    {
        Enable();
        Debug.Log(uniqueid.ToString() + ",Q," + time.ToString());
    }

    string UrlEncode2(string instring)
    {
        return "id: " + 2;
    }

    string UrlEncode(string instring)
    {
        return instring;

        //return "card:" + m_card + ", time:" + m_time + ", ID:" + uniqueid;

        StringReader strRdr = new StringReader(instring);
        StringWriter strWtr = new StringWriter();
        int charValue = strRdr.Read();
        while (charValue != -1)
        {
            if (((charValue >= 48) && (charValue <= 57)) || ((charValue >= 65) && (charValue <= 90)) || ((charValue >= 97) && (charValue <= 122)))
                strWtr.Write((char)charValue);
            else if (charValue == 32)
                strWtr.Write("+");
            else
                strWtr.Write("%{0:x2}", charValue);

            charValue = strRdr.Read();
        }
        return strWtr.ToString();
    }
}