using System.IO;
using UnityEngine;

namespace _Game.Scripts
{
    public class SaveLoad : MonoBehaviour
    {
        [field: SerializeField] 
        private UiManager uiManager;
        
        private string _applicationDataPath;
        
        public void Init()
        {  
            _applicationDataPath = Application.dataPath + "/save.txt";

            Load();
        }
        
        public void Save()
        {
            var saveData = new SaveData
            {
                HighScore = uiManager.HighScoreNumber
            };
            
            var json = JsonUtility.ToJson(saveData);

            File.WriteAllText(_applicationDataPath, json);
        }

        private void Load()
        {
            if (!File.Exists(_applicationDataPath)) return;
            
            var saveFileText = File.ReadAllText(_applicationDataPath);
                
            var saveData = JsonUtility.FromJson<SaveData>(saveFileText);

            uiManager.LoadScoreSetter(saveData.HighScore);
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
    
    public class SaveData
    {
        public int HighScore;
    }
}
