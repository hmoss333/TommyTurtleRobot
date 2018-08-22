using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZowiProtocol : MonoBehaviour {
    public static char SEPARATOR = ' ';
    public static string FINAL = "\n\r";

    public static char MOVE_COMMAND = 'M';
    public static char MOVE_STOP_OPTION = '0';
    public static char MOVE_WALK_FORWARD_OPTION = '1';
    public static char MOVE_WALK_BACKWARD_OPTION = '2';
    public static char MOVE_TURN_LEFT_OPTION = '3';
    public static char MOVE_TURN_RIGHT_OPTION = '4';
    public static char MOVE_UPDOWN_OPTION = '5';
    public static char MOVE_MOONWALKER_LEFT_OPTION = '6';
    public static char MOVE_MOONWALKER_RIGHT_OPTION = '7';
    public static char MOVE_SWING_OPTION = '8';
    public static string MOVE_CRUSAITO_LEFT_OPTION = "9";
    public static string MOVE_CRUSAITO_RIGHT_OPTION = "10";
    public static string MOVE_JUMP_OPTION = "11";

    public static char GESTURE_COMMAND = 'H';
    public static char GESTURE_HAPPY = '1';
    public static char GESTURE_SUPER_HAPPY = '2';
    public static char GESTURE_SAD = '3';
    public static char GESTURE_SLEEPING = '4';
    public static char GESTURE_FART = '5';
    public static char GESTURE_CONFUSED = '6';
    public static char GESTURE_LOVE = '7';
    public static char GESTURE_ANGRY = '8';
    public static char GESTURE_FRETFUL = '9';
    public static string GESTURE_MAGIC = "10";
    public static string GESTURE_WAVE = "11";
    public static string GESTURE_VICTORY = "12";
    public static string GESTURE_FAIL = "13";

    public static char SING_COMMAND = 'K';
    public static char SING_CONNECT = '1';
    public static char SING_DISCONNECT = '2';
    public static char SING_SURPRISE = '3';
    public static char SING_OhOoh = '4';
    public static char SING_OhOoh_2 = '5';
    public static char SING_CUDDLY = '6';
    public static char SING_SLEEPING = '7';
    public static char SING_HAPPY = '8';
    public static char SING_SUPER_HAPPY = '9';
    public static string SING_HAPPY_SHORT = "10";
    public static string SING_SAD = "11";
    public static string SING_CONFUSED = "12";
    public static string SING_FART_1 = "13";
    public static string SING_FART_2 = "14";
    public static string SING_FART_3 = "15";
    public static string SING_MODEL_1 = "16";
    public static string SING_MODEL_2 = "17";
    public static string SING_MODEL_3 = "18";
    public static string SING_BUTTON_PUSHED = "19";

    public static char ACK_COMMAND = 'A';
    public static char FINAL_ACK_COMMAND = 'F';

    public static char BATTERY_COMMAND = 'B';
    public static char PROGRAMID_COMMAND = 'I';
}
