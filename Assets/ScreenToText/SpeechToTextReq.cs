using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;

/*  
 *  Uses the Microsoft Speech To Text Rest API
 *  https://docs.microsoft.com/en-us/azure/cognitive-services/speech/home
 *
 *  Documentation:
 *  https://docs.microsoft.com/en-us/azure/cognitive-services/speech/getstarted/getstarted
 *  
 *  Configs:
 * 
 *  Subscription Key:   f7adac941d3040db90841da00ed72b10
 *  Recognition URL:    https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1?language=en-us&format=simple
 *  Token URI:          https://api.cognitive.microsoft.com/sts/v1.0
 *
 *       //GENERAL USAGE
 *      
 *       public GameObject speechToText;
 *       SpeechToTextReq speechConvert = speechToText.GetComponent<SpeechToTextReq>();
 *       
 *       //Pass in the wav file and callback functions as arguments      
 *       speechConvert.GetVoiceText("/Page10.wav", (text) =>
 *       {
 *           Debug.Log(text);
 *       });
 *
 * */

public class SpeechToTextReq : MonoBehaviour {

    public string subscriptionKey;
    public string recognitionURL;
    public string FetchTokenUri;
    private string token;
    private string fileName;

    //Main Calling Method
    public void GetVoiceText(string fileName, Action<string> returnData)
    {
        this.fileName = fileName;
        SetAuthentication(returnData);
    }

    private void SetAuthentication(Action<string> returnData)
    {
        StartCoroutine(FetchToken(returnData));
    }

    private string GetAccessToken()
    {
        return token;
    }

    /* HTTP TASKS */
    /*Gets the security token from MicroSoft*/
    IEnumerator FetchToken(Action<string> returnData)
    {
        string fetchUri = FetchTokenUri;
        fetchUri += "/issueToken";
        using (UnityWebRequest req = UnityWebRequest.Post(fetchUri, ""))
        {
            req.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            yield return req.Send();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            token = System.Text.Encoding.Default.GetString(result);
            StartCoroutine(ConvertSpeechToText(fileName, returnData));
        }
    }

    //Sends the Audio file and returns the speech text
    //The file must be in the assets folder, if it's not, you will have to specify your own path.
    IEnumerator ConvertSpeechToText(string wavFile, Action<string> returnData) {
        /*validation to find file*/
         //read in file to bytes
         byte[] audio = File.ReadAllBytes(/*Application.dataPath + */wavFile);

        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audio, "name.wav", "audio/wav");

        // Upload to a cgi script
        using (var www = UnityWebRequest.Post(recognitionURL, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + GetAccessToken());
            yield return www.Send();
            if (www.isError) {
                print(www.isError);
                returnData(www.isError.ToString());
            }
            else {
                try
                {
                    var data = JSON.Parse(www.downloadHandler.text);
                    if (data["RecognitionStatus"].Value == "Success")
                    {
                        returnData(data["DisplayText"].Value);
                    }
                    else
                    {
                        returnData("Error Occurred");
                    }
                } catch
                {
                        returnData("Error Occurred");
                }
            }
        }
    }
}
