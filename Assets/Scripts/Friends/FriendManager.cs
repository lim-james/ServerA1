using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun
{
    public class FriendManager : MonoBehaviour
    {
        [SerializeField]
        private InputField addField;
        [SerializeField]
        private Text addErrrorLabel;

        [SerializeField]
        private FriendTableView friendsList;

        private void Start()
        {
            addField.gameObject.SetActive(false);
            friendsList.SetFriends(AccountManager.Instance().friends);
        }

        // handlers

        public void AddHandler()
        {
            string error;
            if (AccountManager.Instance().AddUser(addField.text, out error))
            {
                addField.gameObject.SetActive(false);
                addField.text = "";
                friendsList.ReloadData();
            }

            addErrrorLabel.text = error;
        }

        // button handlers     

        public void AddButtonHandler()
        {
            addField.gameObject.SetActive(true);
        }
    }
}
