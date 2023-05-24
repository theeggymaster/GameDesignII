using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
namespace MyGame
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Constants
        //store the player name in the player Prefs
        const string playerNamePrefKey = "PlayerName";
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = this.GetComponent<InputField>();
            if (inputField is null) return;
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                //if we have played before and save our name, make it default
                //to that name
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            }
            PhotonNetwork.NickName = defaultName;
        }
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.Log("Player name is null or empty");
                return;
            }
            //set the name in the network and player prefs
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}