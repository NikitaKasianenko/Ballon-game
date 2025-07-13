using UnityEngine;

[CreateAssetMenu(fileName = "New Balloon Data", menuName = "Game/Balloon Data")]
public class BalloonData : ScriptableObject
{
    [Header("Shop Info")]
    public string balloonName = "Default Balloon";
    public string id;
    public Sprite balloonSprite;
    public int price = 0;
    public bool isUnlockedByDefault = false;

    [Header("Game features")]
    public int health = 1;
    public Ability ability;

    [Header("Type of movement")]
    public MovementStrategy movementStrategy;
}

