using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Camera mainCam;
    private CircleCollider2D boxCol;

    void Start()
    {
        mainCam = Camera.main;
        boxCol = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        }

        moveInput = moveInput.normalized;
        transform.position += (Vector3) moveInput * moveSpeed * Time.deltaTime;

        ClampToCameraBounds();
    }

    void ClampToCameraBounds()
    {
        Vector3 pos = transform.position;

        float halfWidth = boxCol.bounds.extents.x;
        float halfHeight = boxCol.bounds.extents.y;

        Vector3 min = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, min.y + halfHeight, max.y - halfHeight);

        transform.position = pos;
    }
}
