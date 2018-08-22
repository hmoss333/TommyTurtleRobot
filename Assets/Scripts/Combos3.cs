﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combos3 : MonoBehaviour
{
    public GameObject player;
    public Text move;
    public Text showMoves;
    List<string> movement = new List<string>();
    List<string> movementListCreation = new List<string>();
    string size;
    public AudioSource narration;
    public AudioSource incorrectVoiceOver;

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
    public AudioClip winSound;

    int shrinkCount;
    public Text help;
    public GameObject canvas;
    public GameObject winCanvas;

    int spinCount;
    int jumpCount;

    bool singBetween;

    public Button[] myButtons;
    int buttonCount;

    ZowiController zowiController;
    public GameObject transmittingBackground;
    float zowiCommandWaitTime;

    // Use this for initialization
    void Start()
    {
        GameObject moveBackground = GameObject.Find("MoveBackground");
        GameObject helpBackground = GameObject.Find("HelpBackground");
        switch(PlayerPrefs.GetInt("fontSizeIndex"))
        {
            case 0:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.0f);
                helpBackground.transform.localScale = new Vector2(1.0f, 1.0f);
                help.GetComponent<RectTransform>().sizeDelta = new Vector2(200.0f, 145);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(375, 145);
                break;
            case 1:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.2f);
                helpBackground.transform.localScale = new Vector2(1.3f, 1.2f);
                help.GetComponent<RectTransform>().sizeDelta = new Vector2(250.0f, 190);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(375, 190);
                break;
            case 2:
                moveBackground.transform.localScale = new Vector2(1.0f, 1.5f);
                helpBackground.transform.localScale = new Vector2(1.5f, 1.5f);
                help.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 210);
                showMoves.GetComponent<RectTransform>().sizeDelta = new Vector2(375, 210);
                break;
        }
        showMoves.fontSize = PlayerPrefs.GetInt("fontSize");
        help.fontSize = PlayerPrefs.GetInt("fontSize")-5;
        singBetween = false;
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
        shrinkCount = 0;

        spinCount = 0;
        jumpCount = 0;

        buttonCount = 0;
        if (PlayerPrefs.GetInt("Scan") == 1) { StartCoroutine(scanner()); }
        if (PlayerPrefs.GetInt("Voice") == 1) { narration.Play(); }
        GameStatusEventHandler.gameWasStarted("challenge");

        zowiController = GameObject.FindObjectOfType<ZowiController>();
        if (zowiController.device.IsConnected)
            zowiController.home();
        transmittingBackground.SetActive(false);

        switch (ZowiController.time)
        {
            case 1500:
                zowiCommandWaitTime = 3f;
                break;
            case 1000:
                zowiCommandWaitTime = 2f;
                break;
            case 500:
                zowiCommandWaitTime = 1f;
                break;
            default:
                zowiCommandWaitTime = 2f;
                Debug.Log("Can't get control time");
                break;
        }
    }

    void narrationVoiceOverStop()
    {
        if (PlayerPrefs.GetInt("Voice") == 1)
        {
            if (incorrectVoiceOver.isPlaying)
                incorrectVoiceOver.Stop();
            if (narration.isPlaying)
                narration.Stop();
        }
    }

    IEnumerator scanner()
    {
        myButtons[buttonCount].GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(PlayerPrefs.GetFloat("scanSpeed"));
        myButtons[buttonCount].GetComponent<Image>().color = new Color(0.258f, 0.941f, 0.090f, 1);

        if (buttonCount == myButtons.Length - 1) { buttonCount = 0; } else { buttonCount++; }
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
        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) return;

            if ((PlayerPrefs.GetInt("Scan") == 1))
            {
                checkScanPosition();
            }
        }

        if (!zowiController.device.IsConnected)
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

    public void addSpin() { movement.Add("Spin"); showMoves.text = showMoves.text + "Spin..."; lineSkip(4); playSound(11); spinCount++; }
    public void addGrow() { movement.Add("Grow"); showMoves.text = showMoves.text + "Grow..."; lineSkip(4); playSound(5); }
    public void addShrink() { movement.Add("Shrink"); showMoves.text = showMoves.text + "Shrink..."; lineSkip(6); playSound(9); shrinkCount += 1; }
    public void addTurn() { movement.Add("Turn"); showMoves.text = showMoves.text + "Turn..."; lineSkip(4); playSound(13); }
    public void addJump() { movement.Add("Jump"); showMoves.text = showMoves.text + "Jump..."; lineSkip(4); playSound(6); jumpCount++; }
    public void addWalkForward() { movement.Add("Forward"); showMoves.text = showMoves.text + "Forward..."; lineSkip(7); playSound(4); }
    public void addWalkBackward() { movement.Add("Backward"); showMoves.text = showMoves.text + "Backward..."; lineSkip(8); playSound(0); }
    public void addSing() { movement.Add("Sing"); showMoves.text = showMoves.text + "Sing..."; lineSkip(4); playSound(10); }

    public void erase()
    {
        if (movement.Count >= 1)
        {
            playSound(14);
            if (movement[movement.Count - 1] == "Begin Loop")
                loopState = false;
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
        if (countTillLineSkip >= 60)
        {
            showMoves.text = showMoves.text + "\n";
            countTillLineSkip = 0;
        }
    }

    IEnumerator playNarration()
    {
        yield return new WaitForSeconds(1.0f);
        if (PlayerPrefs.GetInt("Voice") == 1)
            narration.Play();
    }

    public void clearList()
    {
        spinCount = 0;
        jumpCount = 0;
        shrinkCount = 0;
        help.text = "<b><color=yellow>Turn</color></b> towards Cat and show him you can <b><color=yellow>Grow</color></b> too! <b><color=yellow>Jump</color></b> for joy after you do!";
        playSound(2);
        movementLengthCollection = 0;
        movement.Clear();
        move.text = "List Of Moves Cleared";
        showMoves.text = "";
        player.transform.localScale = new Vector3(2, 2, 2);
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.transform.position = new Vector3(3f, -3.72f, 0f);
        loopState = false;
        StartCoroutine(playNarration());
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
            player.transform.position = new Vector3(3f, -3.72f, 0f);
            if (zowiController.device.IsConnected)
                StartCoroutine(sendToZowi());
            else
                StartCoroutine(playingMovement());
        }
        else { move.text = "Must Close All Loops To Play"; }
    }

    public void slider()
    {
        loopsFromSlider = myLoops.value;
        howManyTimesToLoop.text = "Times To Loop : " + loopsFromSlider;
    }

    void insertFormat(int i)
    {
        for (int h = 0; h < movement.Count; h++)
        {
            if (movement[h].Contains(startFormat))
            {
                if (movement[h].Contains("Right"))
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

            if (movement[i].Contains("Begin Loop")) { /*i++;*/ saveStartLocation = i; }

            if (movement[i].Contains("End Loop")) { countLoops++; if (countLoops < loopsFromSlider) { i = saveStartLocation; } else { countLoops = 0; } }

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
                if (player.transform.position.x >= 4.6f && player.transform.position.x <= 6.3) { singBetween = true; }
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
            else if (facingRight && turned)
            {
                player.transform.eulerAngles = new Vector3(0, 270, 0);
                facingRight = false;
            }

            growthSwitch = true;
            shrinkSwitch = true;
            turned = false;
        }

        move.text = "Done Moving";
        checkCorrect();
    }

    IEnumerator sendToZowi()
    {
        transmittingBackground.SetActive(true);

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

                yield return new WaitForSeconds(7f);
            }

            if (movement[i].Contains("Spin"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.turn(1);
                }

                yield return new WaitForSeconds(14f);
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

            if (movement[i].Contains("Grow") || movement[i].Contains("Shrink"))
            {
                if (zowiController.device.IsConnected)
                {
                    zowiController.updown();
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
        checkCorrect();
    }

    void checkCorrect()
    {
        try
        {
            if (movement[0].Equals("Turn") && movement[1].Equals("Grow") && movement[2].Equals(startFormat + "Jump" + endFormat))
            {
                displayWinScreen();
            }
            else
            {
                displayErrorMessage();
            }
        } catch(ArgumentOutOfRangeException ex)
        {
            displayErrorMessage();
        }

        transmittingBackground.SetActive(false);
    }
     void displayWinScreen()
    {
        canvas.SetActive(false);
        winCanvas.SetActive(true);
        GetComponent<AudioSource>().clip = winSound;
        GetComponent<AudioSource>().Play();
        player.transform.eulerAngles = new Vector3(0, 180, 0);
        AnimatePlayer.win = true;
        AnimateFriend.win = true;
        PointHandler.completedChallenges += 1.0f;

        if (zowiController.device.IsConnected)
        {
            zowiController.testSound();
            zowiController.swing();
            zowiController.home();
        }
    }

    void displayErrorMessage()
    {
            help.text = "Good try! Press Clear To Try Again";
            if (PlayerPrefs.GetInt("Voice") == 1)
            {
                incorrectVoiceOver.Play();
                if (narration.isPlaying)
                    narration.Stop();
            }
            PointHandler.incorrect += 1.0f;
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
            narrationVoiceOverStop();
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
        LoadManager.level = "Title";
        SceneManager.LoadScene("LoadLevel");
        //Application.LoadLevel("Title");
    }

    public void nextLevel()
    {
        GameStatusEventHandler.gameWasStopped();
        StartCoroutine(mainMenuStart());
    }

}