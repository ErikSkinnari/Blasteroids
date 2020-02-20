using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class HighscoreHandler : MonoBehaviour
{
    private readonly string url = @"https://localhost:44343/api/Highscores";

    public void SendHighscore(Highscore highscore)
    {
        StartCoroutine(Upload(highscore));
    }

    IEnumerator Upload(Highscore highscore)
    {
        var postData = JsonUtility.ToJson(highscore);
        Debug.Log(postData);
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("POST Completed: " + uwr.downloadHandler.text);
        }
    }

}