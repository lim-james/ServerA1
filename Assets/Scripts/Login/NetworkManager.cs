using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("On connected");
            PhotonNetwork.JoinRandomRoom();
        }

        //public override void OnJoinedLobby()
        //{
        //    Debug.Log("On joined");
        //    PhotonNetwork.JoinOrCreateRoom("testRoom");
        //}
    }
}
