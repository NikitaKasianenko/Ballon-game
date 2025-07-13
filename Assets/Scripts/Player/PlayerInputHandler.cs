using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, IInputProvider
{
    public Vector2 targetPosition { get; private set; }
    public bool abilityRequested { get; private set; }
    public bool holding { get; private set; }

    [SerializeField] private RectTransform targetArea;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            holding = true;
            targetPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            holding = false;
        }

        //abilityRequested = Input.GetMouseButtonDown(0);
    }
}
