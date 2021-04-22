using System;
using System.IO;
using UnityEngine;

namespace _Game.Scripts
{
    public class SaveLoad 
    {
        private readonly string _applicationDataPath;
        
        public SaveLoad()
        {  
            _applicationDataPath = Application.dataPath + "/save.txt";
        }
        
        public void Save(int score)
        {
            var saveData = new SaveData
            {
                HighScore = score 
            };
            
            var json = JsonUtility.ToJson(saveData);

            File.WriteAllText(_applicationDataPath, json);
        }

        public void Load(UiManager uiManager)
        {
            if (!File.Exists(_applicationDataPath)) return;

            var saveFileText = File.ReadAllText(_applicationDataPath);

            SaveData saveData;

            try
            {
                saveData = JsonUtility.FromJson<SaveData>(saveFileText);
            }
            catch (Exception e)
            {
                File.WriteAllText(_applicationDataPath,  JsonUtility.ToJson(""));
                throw;
            }
            
            uiManager.LoadScoreSetter(saveData.HighScore);
        }
    }
    
    public class SaveData
    {
        public int HighScore;
    }
}
