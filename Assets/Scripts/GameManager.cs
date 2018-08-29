﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public Text move;
    public Text showMoves;
    public static string movesString;
    public static List<string> movement = new List<string>();
    public static bool loadedCode = false;
    string size;
    GameObject moveBackground;

    bool growthSwitch;
    bool shrinkSwitch;

    bool facingRight;
    bool turned;

    bool spinOrRoll;

    bool jumpSwitch;
    bool jumpReset;

    public static int index;
    int countTillLineSkip;

    Vector3 resetAfterSpinOrRoll;
    Vector3 resetAfterJump;

    int saveStartLocation;
    int countLoops;
    public Text howManyTimesToLoop;
    float loopsFromSlider;
    public Slider myLoops;

    string startFormat;
    string endFormat;
    int movementLengthCollection;
    bool loopState;

    public AudioClip[] mySounds;

    public Button[] myButtons;
    int buttonCount;

    ZowiController zowiController;
    private bool transmittingToZowi;
    public GameObject transmittingBackground;
    public GameObject zowiControlsMenu;
    public GameObject zowiControlsMenuButton;
    public GameObject zowiFaceMenu;
    public GameObject zowiFaceMenuButton;
    public GameObject defaultMenu;
    public GameObject transmitCanvas;

    Text growText;
    Text shrinkText;

    // Use this for initialization
    void Start()
    {
        moveBackground = GameObject.Find("MoveBackground");
        switch (PlayerPrefs.GetInt("fontSizeIndex"))
        {
            case 0:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.0f);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(504, 145);
                break;
            case 1:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.2f);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(504, 190);
                break;
            case 2:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.5f);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(504, 210);
                break;
        }
        showMoves.fontSize = PlayerPrefs.GetInt("fontSize");
        loopState = false;
        loopsFromSlider = 2;
        howManyTimesToLoop.text = "Times To Loop : " + loopsFromSlider;
        countLoops = 0;
        saveStartLocation = -1;
        countTillLineSkip = 0;
        growthSwitch = true;
        shrinkSwitch = true;

        turned = false;
        facingRight = true;

        spinOrRoll = false;

        jumpSwitch = true;
        jumpReset = false;

        startFormat = "<b><color=#00ff00ff>";
        endFormat = "</color></b>";
        movementLengthCollection = 0;

        buttonCount = 0;
        if (PlayerPrefs.GetInt("Scan") == 1) { StartCoroutine(scanner()); }
        GameStatusEventHandler.gameWasStarted("freeplay");

        growText = GameObject.Find("Grow").GetComponentInChildren<Text>();
        shrinkText = GameObject.Find("Shrink").GetComponentInChildren<Text>();
        zowiController = GameObject.FindObjectOfType<ZowiController>();
        if (zowiController && zowiController.device.IsConnected)
        {           
            zowiController.home();
            growText.text = "Dance";
            shrinkText.text = "Swing";
        }
        else
        {
            zowiFaceMenuButton.SetActive(false);
            zowiControlsMenuButton.SetActive(false);
            growText.text = "Grow";
            shrinkText.text = "Shrink";
        }
        transmittingBackground.SetActive(false);
        transmittingToZowi = false;
    }

    public void zowiWalk(int dir)
    {
        AnimatePlayer.run = true;

        if (zowiController.device.IsConnected)
            zowiController.walk(dir);
    }
    public void zowiTurn(int dir)
    {
        AnimatePlayer.run = true;

        if (zowiController.device.IsConnected)
            zowiController.turn(dir);
    }
    public void zowiReset()
    {
        AnimatePlayer.run = false;
        AnimatePlayer.sing = false;
        AnimatePlayer.win = false;
        AnimatePlayer.jump = false;

        if (zowiController.device.IsConnected)
            zowiController.home();
    }

    public void OpenZowiMenu()
    {
        zowiControlsMenu.SetActive(true);
        defaultMenu.SetActive(false);
    }
    public void OpenDefaultMenu()
    {
        zowiControlsMenu.SetActive(false);
        defaultMenu.SetActive(true);
    }
    public void OpenZowiFaceMenu()
    {
        zowiFaceMenu.SetActive(true);
    }
    public void CloseZowiFaceMenu()
    {
        zowiFaceMenu.SetActive(false);
    }

    IEnumerator scanner()
    {
        myButtons[buttonCount].GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(PlayerPrefs.GetFloat("scanSpeed"));
        myButtons[buttonCount].GetComponent<Image>().color = new Color(0.258f, 0.941f, 0.090f, 1);

        if(buttonCount == myButtons.Length -1) { buttonCount = 0; } else { buttonCount++; }
        StartCoroutine(scanner());
    }

    void checkScanPosition()
    {
        if (buttonCount == 0) { addGrow(); }
        else if (buttonCount == 1) { addShrink(); }
        else if (buttonCount == 2) { addTurn(); }
        else if (buttonCount == 3) { addSpin(); }
        else if (buttonCount == 4) { addJump(); }
        else if (buttonCount == 5) { addSing(); }
        else if (buttonCount == 6) { addWalkForward(); }
        else if (buttonCount == 7) { addWalkBackward(); }
        else if (buttonCount == 8) { startLoop(); }
        else if (buttonCount == 9) { endLoop(); }
        else if (buttonCount == 10) { play(); }
        else if (buttonCount == 11) { clearList(); }
        else if (buttonCount == 12) { erase(); }
        else if (buttonCount == 13) { mainMenu(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (showMoves.text != movesString)
        {
            movesString = showMoves.text;
        }

        if (loadedCode)
        {

            Debug.Log("Made it here: " + movement.Count);
            clearList();
            foreach (string currMove in movement)
            {
                Debug.Log("CurrentMove: " + currMove);
                if (currMove != "")
                {
                    showMoves.text += currMove + "...";
                    lineSkip(4);
                }
            }
            loadedCode = false;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) return;

            if ((PlayerPrefs.GetInt("Scan") == 1))
            {
                checkScanPosition();
            }
        }

        if (!transmittingToZowi)
        {
            if (move.text.Contains("Forward"))
            {
                if (facingRight)
                {
                    if (player.transform.position.x < 6.7)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x + 2, player.transform.position.y, 0), Time.deltaTime * 1);
                    }
                }
                else
                {
                    if (player.transform.position.x > -7.4)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x - 2, player.transform.position.y, 0), Time.deltaTime * 1);
                    }
                }
            }

            if (move.text.Contains("Backward"))
            {
                if (facingRight)
                {
                    if (player.transform.position.x > -7.4)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x - 2, player.transform.position.y, 0), Time.deltaTime * 1);
                    }
                }
                else
                {
                    if (player.transform.position.x < 6.7)
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x + 2, player.transform.position.y, 0), Time.deltaTime * 1);
                    }
                }
            }


            if (move.text.Contains("Spin")) { spinOrRoll = true; player.transform.Rotate(0, Time.deltaTime * 370, 0); }

            if (move.text.Contains("Grow") && growthSwitch)
            {
                if (player.transform.localScale.x < 3)
                {
                    player.transform.localScale = new Vector3(player.transform.localScale.x + .5f, player.transform.localScale.y + .5f, player.transform.localScale.z + .5f);
                }
                growthSwitch = false;
            }

            if (move.text.Contains("Shrink") && shrinkSwitch)
            {
                if (player.transform.localScale.x > .5f)
                {
                    player.transform.localScale = new Vector3(player.transform.localScale.x - .5f, player.transform.localScale.y - .5f, player.transform.localScale.z - .5f);
                }
                shrinkSwitch = false;
            }

            if (move.text.Contains("Jump"))
            {
                if (jumpSwitch)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 2, 0), Time.deltaTime * 4);
                }
                else
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, player.transform.position.y - 2, 0), Time.deltaTime * 4);
                }
            }

            if (move.text.Contains("Turn"))
            {
                turned = true;
                player.transform.Rotate(0, Time.deltaTime * 180, 0);
            }
        }
    }

    public void addSpin() {  movement.Add("Spin"); showMoves.text = showMoves.text + "Spin..."; lineSkip(4); playSound(11); } 
    public void addGrow()
    {
        if (zowiController.device.IsConnected)
        {
            movement.Add("Dance");
            showMoves.text = showMoves.text + "Dance...";
        }
        else
        {
            movement.Add("Grow");
            showMoves.text = showMoves.text + "Grow...";
        }
        lineSkip(4);
        playSound(5);
    } 
    public void addShrink()
    {
        if (zowiController.device.IsConnected)
        {
            movement.Add("Swing");
            showMoves.text = showMoves.text + "Swing...";
        }
        else
        {
            movement.Add("Shrink");
            showMoves.text = showMoves.text + "Shrink...";
        }
        lineSkip(6);
        playSound(9);
    } 
    public void addTurn() { movement.Add("Turn"); showMoves.text = showMoves.text + "Turn..."; lineSkip(4); playSound(13); } 
    public void addJump() { movement.Add("Jump"); showMoves.text = showMoves.text + "Jump..."; lineSkip(4); playSound(6);  }
    public void addWalkForward() {  movement.Add("Forward"); showMoves.text = showMoves.text + "Forward..."; lineSkip(7); playSound(4);  }
    public void addWalkBackward() { movement.Add("Backward"); showMoves.text = showMoves.text + "Backward..."; lineSkip(8); playSound(0); }
    public void addSing() {  movement.Add("Sing"); showMoves.text = showMoves.text + "Sing..."; lineSkip(4); playSound(10); }

    public void erase() 
    {
        if (movement.Count >= 1)
        {
            playSound(14);
            movement.RemoveAt(movement.Count - 1);

            showMoves.text = "";
            for (int i = 0; i < movement.Count; i++)
            {
                showMoves.text += movement[i] + "...";
            }
        }
    }

    public void startLoop()
    {
            if (!loopState)
            {
                movement.Add("Begin Loop"); showMoves.text = showMoves.text + "Begin Loop..."; lineSkip(10);
                loopState = true;
                playSound(1);
            }        
    }

    public void endLoop()
    {        
            if (loopState)
            {
                movement.Add("End Loop"); showMoves.text = showMoves.text + "End Loop..."; lineSkip(8);
                loopState = false;
                playSound(3);
            }        
    }

    void lineSkip(int counter)
    {
        countTillLineSkip += counter;
        if(countTillLineSkip >= 60)
        {
            showMoves.text = showMoves.text + "\n";
            countTillLineSkip = 0;
        }
    }

    public void clearList()
    {
        loopState = false;
        if (!loadedCode)
        {
            playSound(2);
            movement.Clear();
            move.text = "List Of Moves Cleared";
        }
        movementLengthCollection = 0;
        showMoves.text = "";
        player.transform.localScale = new Vector3(2, 2, 2);
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.transform.position = new Vector3(-2.64f,-3.72f,0.28f); 
    }
    public void play()
    {
        StartCoroutine(playStart());
    }
    public IEnumerator playStart()
    {
        playSound(8);
        yield return new WaitForSeconds(1);
        facingRight = true;
        if (!loopState)
        {
            movementLengthCollection = 0;
            player.transform.localScale = new Vector3(2, 2, 2);
            player.transform.rotation = Quaternion.Euler(0, 90, 0);
            player.transform.position = new Vector3(-2.64f, -3.72f, 0.28f);
            //if (zowiController.device.IsConnected)
            //    StartCoroutine(sendToZowi());
            //else
                StartCoroutine(playingMovement());
        }
        else { move.text = "Must Close All Loops To Play"; }
    }

    public void slider()
    {
        loopsFromSlider = myLoops.value;
        howManyTimesToLoop.text = "Times To Loop : " + loopsFromSlider;
    }

    //Inserts rich text
    void insertFormat(int i)
    {
        for (int h = 0; h < movement.Count; h++)
        {
            if (movement[h].Contains(startFormat))
            {
                if (movement[h].Contains("Right") || movement[h].Contains("Dance") || movement[h].Contains("Swing"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 5);
                }
                else if (movement[h].Contains("Left") || movement[h].Contains("Turn") || movement[h].Contains("Spin") || movement[h].Contains("Jump") || movement[h].Contains("Grow") || movement[h].Contains("Sing"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 4);
                }
                else if (movement[h].Contains("Shrink"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 6);
                }
                else if (movement[h].Contains("Begin Loop"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 10);
                }
                else if (movement[h].Contains("End Loop"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 8);
                }
                else if (movement[h].Contains("Forward"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 7);
                }
                else if (movement[h].Contains("Backward"))
                {
                    movement[h] = movement[h].Substring(startFormat.Length, 8);
                }
            }
        }

        showMoves.text = "";

        movement[i] = (startFormat + movement[i] + endFormat);

        for (int j = 0; j < movement.Count; j++)
        {
            showMoves.text += movement[j] + "...";
        }
    }

    void playMoveName(string move)
    {
        if (PlayerPrefs.GetInt("Voice") == 1)
        {
            if (move.Contains("Grow")) { GetComponent<AudioSource>().clip = mySounds[5]; }
            if (move.Contains("Spin")) { GetComponent<AudioSource>().clip = mySounds[11]; }
            if (move.Contains("Turn")) { GetComponent<AudioSource>().clip = mySounds[13]; }
            if (move.Contains("Jump")) { GetComponent<AudioSource>().clip = mySounds[6]; }
            if (move.Contains("Sing")) { GetComponent<AudioSource>().clip = mySounds[10]; }
            if (move.Contains("Shrink")) { GetComponent<AudioSource>().clip = mySounds[9]; }
            if (move.Contains("Forward")) { GetComponent<AudioSource>().clip = mySounds[4]; }
            if (move.Contains("Backward")) { GetComponent<AudioSource>().clip = mySounds[0]; }
            if (move.Contains("Begin")) { GetComponent<AudioSource>().clip = mySounds[1]; }
            if (move.Contains("End")) { GetComponent<AudioSource>().clip = mySounds[3]; }

            GetComponent<AudioSource>().Play();
        }
    }
  
    IEnumerator playingMovement()
    {
        for (int i = 0; i < movement.Count; i++)
        {
            if(movement[i].Contains("Begin Loop")) { 
               /*i++;*/ saveStartLocation = i;
            }
            Debug.Log("startLocation: " + saveStartLocation);
          
            if (movement[i].Contains("End Loop")) {
                countLoops++;
                if (countLoops < loopsFromSlider) {
                    i = saveStartLocation;
                } else {
                    countLoops = 0;
                }
            }

            if (movement[i].Contains("Roll") || movement[i].Contains("Spin"))
            {
                resetAfterSpinOrRoll = player.transform.eulerAngles;
                AnimatePlayer.run = true;
            }

            if (movement[i].Contains("Right") || movement[i].Contains("Left") || movement[i].Contains("Turn") || movement[i].Contains("Backward") || movement[i].Contains("Forward"))
            {
                AnimatePlayer.run = true;
            }

            if (movement[i].Contains("Sing"))
            {
                AnimatePlayer.sing = true;                               
            }
          
            if (movement[i].Contains("Jump"))
            {
                jumpReset = true; resetAfterJump = player.transform.position;
                AnimatePlayer.jump = true;
                StartCoroutine(jump());
            }
            insertFormat(i);
            
            move.text = movement[i];
            playMoveName(move.text);

            yield return new WaitForSeconds(1);
            AnimatePlayer.run = false;
            AnimatePlayer.jump = false;
            AnimatePlayer.sing = false;
            AnimatePlayer.playOnce = true;
            jumpSwitch = true;

            if (jumpReset)
            {
                player.transform.position = resetAfterJump;
                jumpReset = false;
            }

            if (spinOrRoll)
            {
                player.transform.eulerAngles = resetAfterSpinOrRoll;
                spinOrRoll = false;
            }

            if (!facingRight && turned)
            {
                player.transform.eulerAngles = new Vector3(0, 90, 0);
                facingRight = true;
            }
            else if(facingRight && turned)
            {
                player.transform.eulerAngles = new Vector3(0, 270, 0);
                facingRight = false;
            }

            growthSwitch = true;
            shrinkSwitch = true;
            turned = false;         
        }
      
        move.text = "Done Moving";
        if (zowiController.device.IsConnected && zowiController.sendToZowi == 1)
        {
            defaultMenu.SetActive(false);
            transmitCanvas.SetActive(true);
        }
    }

    IEnumerator sendToZowi()
    {
        transmittingBackground.SetActive(true);
        transmittingToZowi = true;

        yield return new WaitForSeconds(1);

        for (int i = 0; i < movement.Count; i++)
        {
            insertFormat(i);

            move.text = movement[i];

            //playMoveName(move.text);

            if (movement[i].Contains("Begin Loop")) { /*i++;*/ saveStartLocation = i; }

            if (movement[i].Contains("End Loop")) { countLoops++; if (countLoops < loopsFromSlider) { i = saveStartLocation; } else { countLoops = 0; } }

            if (movement[i].Contains("Forward"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.walk(1);
                }
            }
            if (movement[i].Contains("Backward"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.walk(-1);
                }
            }

            if (movement[i].Contains("Turn"))// || movement[i].Contains("Spin"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.turn(1);
                }

                yield return new WaitForSeconds(5.5f);
            }

            if (movement[i].Contains("Spin"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.turn(1);
                }

                yield return new WaitForSeconds(11f);
            }

            if (movement[i].Contains("Sing"))
            {
                //AnimatePlayer.sing = true;

                if (zowiController.device.IsConnected)
                {
                    zowiController.testSound();
                }
            }

            if (movement[i].Contains("Jump"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.jump();
                }
            }

            if (movement[i].Contains("Swing"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.swing();
                }
            }

            if (movement[i].Contains("Dance"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.crusaito(1);
                }
            }

            if (zowiController.device.IsConnected)
            {
                zowiController.home();
            }

            yield return new WaitForSeconds(2f); //slow = 3f, medium = 2f, fast = 1f
        }

        //if (zowiController.device.IsConnected)
        //{
        //    zowiController.home();
        //}
        move.text = "Done Moving";
        //checkCorrect();
        transmittingBackground.SetActive(false);
        defaultMenu.SetActive(true);
        transmittingToZowi = false;
    }

    public void SendCodeToZowi()
    {
        transmitCanvas.SetActive(false);
        defaultMenu.SetActive(true);
        StartCoroutine(sendToZowi());
    }
    public void DontSendCodeToZowi()
    {
        transmitCanvas.SetActive(false);
        defaultMenu.SetActive(true);
    }

    public void UpdateZowiFace(int val)
    {
        if (zowiController)
            zowiController.updateFace(val);
    }

    IEnumerator jump()
    {
        yield return new WaitForSeconds(.5f);
        jumpSwitch = false;       
    }

    void playSound(int num)
    {
        if (PlayerPrefs.GetInt("Voice") == 1)
        {
            GetComponent<AudioSource>().clip = mySounds[num];
            GetComponent<AudioSource>().Play();
        }
    }
   
    public void mainMenu()
    {
        GameStatusEventHandler.gameWasStopped();
        StartCoroutine(mainMenuStart());
    }
    IEnumerator mainMenuStart()
    {
        playSound(7);
        yield return new WaitForSeconds(1.5f);
        clearList();
        LoadManager.level = "Title";
        SceneManager.LoadScene("LoadLevel");
    }
}
