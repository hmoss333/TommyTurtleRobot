using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour {

    private AnimatePlayer player;
    private Sprite sprite;
    public Sprite[] values;

    public float yOff;
    float yPos;

    // Use this for initialization
    void Start()
    {
        Camera mainCam = Camera.main;
        //mainCam.transparencySortMode = TransparencySortMode.Orthographic;
        mainCam.opaqueSortMode = UnityEngine.Rendering.OpaqueSortMode.FrontToBack;
        player = GameObject.FindObjectOfType<AnimatePlayer>();
        transform.position = player.transform.position;

        yPos = player.transform.position.y - yOff;
    }

    private void Update()
    {
        if (!player)
            player = GameObject.FindObjectOfType<AnimatePlayer>();

        else
        {
            transform.position = new Vector3(player.transform.position.x, yPos, player.transform.position.z);
        }
    }
}
