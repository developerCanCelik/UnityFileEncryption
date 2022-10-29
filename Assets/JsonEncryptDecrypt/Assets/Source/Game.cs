using System.IO;
using Source.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class Game : MonoBehaviour
    {
        private PlayerData playerData;
        private string jsonPath;
        public Button SetNameButton;
        public Button UpdateScoreBtn;
        
        private void Awake()
        {
            jsonPath = Application.persistentDataPath + "/playerdata.json";
            
            SetNameButton.onClick.AddListener(ChangeName);
            UpdateScoreBtn.onClick.AddListener(UpdateScoreBtnOnClick);
            
            CreateAndLoadPlayerData();
        }

        private void UpdateScoreBtnOnClick()
        {
            playerData.HighestScore += 100;
            UpdatePlayerData();
            Debug.Log("NEW HighestScore : "+playerData.HighestScore);
        }

        private void ChangeName()
        {
            playerData.PlayerName = "Ahmet";

            UpdatePlayerData();
        }

        private void UpdatePlayerData()
        {
            string json = JsonUtility.ToJson(playerData);
            
            json = StringCipher.Encrypt(json, "qwe123**");
            
            File.WriteAllText(jsonPath, json);
        }

        private void CreateAndLoadPlayerData()
        {
            if (!File.Exists(jsonPath))
            {
                FileStream fs = File.Create(jsonPath);
                fs.Close();

                playerData = new PlayerData();
                playerData.PlayerName = "";
                playerData.HighestScore = 0;
                playerData.IsSoundOff = false;

                UpdatePlayerData();
            }
            else
            {
                string json = File.ReadAllText(jsonPath);

                json = StringCipher.Decrypt(json, "qwe123**");
                
                playerData = JsonUtility.FromJson<PlayerData>(json);
                
                Debug.Log("Read HighestScore : "+playerData.HighestScore);
            }
            
            
        }
    }
}