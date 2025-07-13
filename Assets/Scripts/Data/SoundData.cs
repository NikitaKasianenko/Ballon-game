using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Data", menuName = "Game/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("SFX Clips")]
    public AudioClip inGame;
    public AudioClip inMainMenu;
    public AudioClip gameWin;
    public AudioClip gameOver;
    public AudioClip hit;
    public AudioClip ability;
    public AudioClip enemeSpawn;
}