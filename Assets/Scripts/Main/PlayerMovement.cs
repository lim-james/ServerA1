using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector3 direction { get; private set; }

    private void Update()
    {
        Vector3 newDirection = new Vector3(0.0f, 0.0f);

        if (Input.GetKeyDown(KeyCode.W))
            newDirection = new Vector3(0, 1);

        if (Input.GetKeyDown(KeyCode.S))
            newDirection = new Vector3(0, -1);

        if (Input.GetKeyDown(KeyCode.A))
            newDirection = new Vector3(-1, 0);

        if (Input.GetKeyDown(KeyCode.D))
            newDirection = new Vector3(1, 0);

        if (newDirection.x != 0.0f || newDirection.y != 0.0f)
        {
            direction = newDirection;
            transform.Translate(direction);
        }
    }
}
