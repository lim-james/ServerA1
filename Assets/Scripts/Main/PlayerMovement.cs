using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector3 direction { get; private set; }
    private string username;

    private void Start() 
    {
        // TODO - Update player position in db
        // SELECT x, y from Main.Positions WHERE username='username';
        username = AccountManager.Instance().username;
        String sqlCmd = String.Format("SELECT x, y from Main.Positions WHERE username='{0}';", username);
        Debug.Log(sqlCmd);
    }

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

            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            // TODO - Update player position in db
            // UPDATE Main.Positions 
            // SET x=x, y=y
            // WHERE username='username';        
            String sqlCmd = String.Format("UPDATE Main.Positions SET x={0}, y={1} WHERE username='{2}';", x, y, username);
            Debug.Log(sqlCmd);
        }
    }
}
