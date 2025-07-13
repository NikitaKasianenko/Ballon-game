using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string description;
    //public Sprite icon;
    public float cooldown = 5f;

    public abstract void Activate(GameObject owner);
}
