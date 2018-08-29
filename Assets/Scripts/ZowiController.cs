using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

using TechTweaking.Bluetooth;

public class ZowiController : MonoBehaviour {

    public static ZowiController instance = null;

    public BluetoothDevice device;
    public static int time = 1000;

    public bool hasConnected;

    // Use this for initialization
    private void Awake () {
        hasConnected = false;

        BluetoothAdapter.enableBluetooth();//Force Enabling Bluetooth
                                           
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);


        device = new BluetoothDevice();
        //if (BluetoothAdapter.isBluetoothEnabled())
        //{
        //    connect();
        //}
        //else
        if (!BluetoothAdapter.isBluetoothEnabled())
        {
            BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
            //statusText.text = "Status : Please enable your Bluetooth";

            //BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
            BluetoothAdapter.listenToBluetoothState(); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

            BluetoothAdapter.askEnableBluetooth();//Ask user to enable Bluetooth
        }
    }

    public void connect()
    {
        //statusText.text = "Status : Searching...";

        device.Name = "Zowi"; //"HC-06";
        //device.MacAddress = "XX:XX:XX:XX:XX:XX";

        device.connect();
    }

    public void disconnect()
    {
        device.close();
    }

    //Default position; call to end the current animation
    public void home()
    {

        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.MOVE_STOP_OPTION +
                        ZowiProtocol.FINAL);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));

    }

    //############### Movement Commands #################//
    public void walk(int dir)//float steps, int time, int dir)
    {
        Debug.Log("Walking");

        char direction; // = ZowiProtocol.MOVE_STOP_OPTION; ;

        if (dir == -1)
        {
            direction = ZowiProtocol.MOVE_WALK_BACKWARD_OPTION;
        }
        else
        {
            direction = ZowiProtocol.MOVE_WALK_FORWARD_OPTION;
        }

        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        direction +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.FINAL, time);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));
    }

    public void turn(int dir)//float steps, int time, int dir) 
    {
        Debug.Log("Turning");

        char direction; //= ZowiProtocol.MOVE_STOP_OPTION;

        if (dir == -1)
        {
            direction = ZowiProtocol.MOVE_TURN_RIGHT_OPTION;           
        }
        else
        {
            direction = ZowiProtocol.MOVE_TURN_LEFT_OPTION;
        }

        //for (int i = 0; i < count; i++)
        //{
            String command = String.Format(
                    "" + ZowiProtocol.MOVE_COMMAND +
                            ZowiProtocol.SEPARATOR +
                            direction +
                            ZowiProtocol.SEPARATOR +
                            time +
                            ZowiProtocol.FINAL, time);

            device.send(System.Text.Encoding.UTF8.GetBytes(command));
        //}

    }

    public void updown()// float steps, int time, int h) 
    {
        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.MOVE_JUMP_OPTION +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.FINAL, time);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));

    }

    public void moonwalker(int dir)//float steps, int time, int h, int dir) 
    {
        char direction; // = ZowiProtocol.MOVE_STOP_OPTION;

        if (dir == -1)
        {
            direction = ZowiProtocol.MOVE_MOONWALKER_LEFT_OPTION;
        }
        else
        {
            direction = ZowiProtocol.MOVE_MOONWALKER_RIGHT_OPTION;
        }

        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        direction +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.SEPARATOR +
                        26 +
                        ZowiProtocol.FINAL, time, 26); //, h);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));

    }

    public void swing()//float steps, int time, int h) 
    {
        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.MOVE_SWING_OPTION +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.SEPARATOR +
                        25 +
                        ZowiProtocol.FINAL, time, 25);//, h);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));

    }

    public void crusaito(int dir)//float steps, int time, int h, int dir) 
    {
        String direction; //.MOVE_CRUSAITO_RIGHT_OPTION;

        if (dir == -1)
        {
            direction = ZowiProtocol.MOVE_CRUSAITO_LEFT_OPTION;
        }
        else
        {
            direction = ZowiProtocol.MOVE_CRUSAITO_RIGHT_OPTION;
        }

        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        direction +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.SEPARATOR +
                        25 +
                        ZowiProtocol.FINAL, time, 25);//, h);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));

    }

    public void jump()//float steps, int time) 
    {
        String command = String.Format(
                "" + ZowiProtocol.MOVE_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.MOVE_UPDOWN_OPTION +
                        ZowiProtocol.SEPARATOR +
                        time +
                        ZowiProtocol.SEPARATOR +
                        25 +
                        ZowiProtocol.FINAL, time, 25); //, h);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));
    }

    //############### Gesture Commands #################//

    public void testCommand()
    {
        //String command = String.Format(
        //        "" + ZowiProtocol.GESTURE_COMMAND +
        //                ZowiProtocol.SEPARATOR +
        //                ZowiProtocol.GESTURE_HAPPY +
        //                ZowiProtocol.SEPARATOR +
        //                ZowiProtocol.FINAL);

        //device.send(System.Text.Encoding.UTF8.GetBytes(command));
        testSound();
        jump();
        swing();
        testSound();
    }

    //############### Sound Commands #################//

    public void testSound()
    {
        String command = String.Format(
                "" + ZowiProtocol.SING_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.SING_HAPPY +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.FINAL);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));
    }

    //############### Face Commands #################//

    public void testFace(int val)
    {
        String command = String.Format(
                "" + ZowiProtocol.EXPRESSION_COMMAND +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.getMouthShape(val, 0) +
                        ZowiProtocol.getMouthShape(val, 4) +
                        ZowiProtocol.getMouthShape(val, 8) +
                        ZowiProtocol.getMouthShape(val, 12) +
                        ZowiProtocol.getMouthShape(val, 16) +
                        ZowiProtocol.getMouthShape(val, 20) +
                        ZowiProtocol.getMouthShape(val, 24) +
                        ZowiProtocol.getMouthShape(val, 28) +
                        ZowiProtocol.SEPARATOR +
                        ZowiProtocol.FINAL);

        device.send(System.Text.Encoding.UTF8.GetBytes(command));
    }




    public static void walkButton(int dir)
    {
        walkButton(dir);
    }
}
