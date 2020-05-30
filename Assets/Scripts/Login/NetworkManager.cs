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
            PhotonNetwork.JoinLobby();
            Debug.Log("On connected");            
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("On joined");
            RoomOptions defaultOption = new RoomOptions();
            defaultOption.MaxPlayers = 4;
           
            PhotonNetwork.JoinOrCreateRoom("testRoom", defaultOption,TypedLobby.Default);
        }
    }
}
