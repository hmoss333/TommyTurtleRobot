using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class eve : MonoBehaviour {

    public GameObject test;
    SpeechToTextReq sttr;

    public AudioClip audioFile;
    
    // Use this for initialization
	void Start () {
        sttr = test.GetComponent<SpeechToTextReq>();
        //Debug.Log(AssetDatabase.GetAssetPath(audioFile));
        //sttr.GetVoiceText(AssetDatabase.GetAssetPath(audioFile), (text) => { Debug.Log(text); });
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
