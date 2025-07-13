using System;

public static class GameEvents
{
    // Game State
    public static Action<int, int> OnPlayerWin;
    public static Action<int, int> OnPlayerLose;

    public static Action OnWavesEnd;

    // UI
    public static Action OnPlayerNameChange;
    public static Action OnPlayerAvatarChange;

    public static Action<float> OnMusicVolumeChange;
    public static Action<float> OnSoundVolumeChange;

    public static Action UpdatePlayerName;

    //Shop
    public static Action<BalloonData> OnShopItemClicked;
    public static Action OnPlayerDataUpdate;

    //Init
    public static Action OnGameInit;

}
