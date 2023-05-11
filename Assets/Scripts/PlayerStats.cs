using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Flesh;
    public int startFlesh = 400;

    public static int Bones;
    public int startBones = 400;

    public static int Souls;
    public int startSouls = 400;

    public static int Lives;
    public int startLives = 20;

    public static int Rounds;

    private void Start()
    {
        Flesh = startFlesh;
        Bones = startBones;
        Souls = startSouls;
        Lives = startLives;
        Rounds = 0;
    }
}
