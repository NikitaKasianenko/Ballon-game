using UnityEngine;
using UnityEngine.UI;
public class AvatarPicker : MonoBehaviour
{
    public Image avatarPreview;

    public void OnPickAvatarButton()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 128, false);
                if (texture != null)
                {
                    string base64 = System.Convert.ToBase64String(texture.EncodeToPNG());
                    DataManager.Instance.SetPlayerAvatar(base64);
                    GameEvents.OnPlayerAvatarChange?.Invoke();
                }
            }
        }, "Select Image please :))", "image/*");
    }


}
