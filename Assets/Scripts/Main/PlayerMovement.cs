using System;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun
{
    public class PlayerMovement : MonoBehaviour
    {

        public Vector3 direction { get; private set; }
        private string username;

        private void Awake()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        private void Start()
        {
            // TODO - Update player position in db
            // SELECT x, y from Main.Positions WHERE username='username';
            username = AccountManager.Instance().username;
            PhotonNetwork.RaiseEvent((int)Events.GET_POSITION, username, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private void OnEvent(EventData obj)
        {
            Events e = (Events)obj.Code;

            if (e == Events.GET_POSITION)
            {
                GetPositionHandler((object[])obj.CustomData);
            }
        }

        private void GetPositionHandler(object[] data)
        {
            int x = (int)data[0];
            int y = (int)data[1];
            transform.position = new Vector3((float)x, (float)y);
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
                
                object[] data = new object[] {
                    username,
                    (int)transform.position.x,
                    (int)transform.position.y
                };

                PhotonNetwork.RaiseEvent((int)Events.UPDATE_POSITION, data, RaiseEventOptions.Default, SendOptions.SendReliable);
            }
        }
    }
}
