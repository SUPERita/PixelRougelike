using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EmailSender2 : MonoBehaviour
{
    public string recipient = "kingkriegfeedback@gmail.com";
    public string subject = "feedback";
    public string body = "hello i wanted to tell you that you can improve the game if you will:";

    [Button]
    public void SendEmail()
    {
        string emailUrl = "mailto:" + recipient + "?subject=" + EscapeURL(subject) + "&body=" + EscapeURL(body);
        Application.OpenURL(emailUrl);
    }

    string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
