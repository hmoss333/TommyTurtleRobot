using System;
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

    //Not working at the moment; need the correct binary strings
    public static char EXPRESSION_COMMAND = 'L';
    public static string EXPRESSION_0 = "0001100010010010010010010001100";
    public static string EXPRESSION_1 = "0000100001100000100000100001110";
    public static string EXPRESSION_2 = "00001100010010000100001000011110";
    public static string EXPRESSION_3 = "00001100010010000100010010001100";
    public static string EXPRESSION_4 = "00010010010010011110000010000010";
    public static string EXPRESSION_5 = "00011110010000011100000010011100";
    public static string EXPRESSION_6 = "00000100001000011100010010001100";
    public static string EXPRESSION_7 = "00011110000010000100001000010000";
    public static string EXPRESSION_8 = "00001100010010001100010010001100";
    public static string EXPRESSION_9 = "00001100010010001110000010001110";
    public static string EXPRESSION_SMILE = "00000000100001010010001100000000";
    public static string EXPRESSION_HAPPY_OPEN = "00000000111111010010001100000000";
    public static string EXPRESSION_HAPPY_CLOSED = "00000000111111011110000000000000";
    public static string EXPRESSION_HEART = "00010010101101100001010010001100";
    public static string EXPRESSION_BIG_SURPRISE = "00001100010010100001010010001100";
    public static string EXPRESSION_SMALL_SURPRISE = "00000000000000001100001100000000";
    public static string EXPRESSION_TONGUE_OUT = "00111111001001001001000110000000";
    public static string EXPRESSION_VAMP1 = "00111111101101101101010010000000";
    public static string EXPRESSION_VAMP2 = "00111111101101010010000000000000";
    public static string EXPRESSION_LINE = "00000000000000111111000000000000";
    public static string EXPRESSION_CONFUSED = "00000000001000010101100010000000";
    public static string EXPRESSION_DIAGONAL = "00100000010000001000000100000010";
    public static string EXPRESSION_SAD = "00000000001100010010100001000000";
    public static string EXPRESSION_SAD_OPEN = "00000000001100010010111111000000";
    public static string EXPRESSION_SAD_CLOSED = "00000000001100011110110011000000";
    public static string EXPRESSION_OKMOUTH = "00000001000010010100001000000000";
    public static string EXPRESSION_XMOUTH = "00100001010010001100010010100001";
    public static string EXPRESSION_INTEROGATION = "00001100010010000100000100000100";
    public static string EXPRESSION_THUNDER = "00000100001000011100001000010000";
    public static string EXPRESSION_CULITO = "00000000100001101101010010000000";
    public static string EXPRESSION_ANGRY = "00000000011110100001100001000000";




    public static char ACK_COMMAND = 'A';
    public static char FINAL_ACK_COMMAND = 'F';

    public static char BATTERY_COMMAND = 'B';
    public static char PROGRAMID_COMMAND = 'I';



    //############### Get Face Commands #################//

    public static string getMouthShape(int number, int position)
    {
        string[] types = {EXPRESSION_0,EXPRESSION_1,EXPRESSION_2,EXPRESSION_3,EXPRESSION_4,EXPRESSION_5,EXPRESSION_6,EXPRESSION_7,EXPRESSION_8,
            EXPRESSION_9,EXPRESSION_SMILE,EXPRESSION_HAPPY_OPEN,EXPRESSION_HAPPY_CLOSED,EXPRESSION_HEART,EXPRESSION_BIG_SURPRISE,EXPRESSION_SMALL_SURPRISE,
            EXPRESSION_TONGUE_OUT,EXPRESSION_VAMP1,EXPRESSION_VAMP2,EXPRESSION_LINE,EXPRESSION_CONFUSED,EXPRESSION_DIAGONAL,EXPRESSION_SAD,EXPRESSION_SAD_OPEN,
            EXPRESSION_SAD_CLOSED,EXPRESSION_OKMOUTH,EXPRESSION_XMOUTH,EXPRESSION_INTEROGATION,EXPRESSION_THUNDER,EXPRESSION_CULITO,EXPRESSION_ANGRY };

        string tempString = "";
        for (int i = position; i < position + 4; i++)
        {
            //put a nested for loop in here to eliminate the need for the "position" variable
            try
            {
                tempString += types[number][i];
            }
            catch
            {
                Debug.Log(tempString);
                return tempString;
            }
        }

        return tempString;
    }
}
