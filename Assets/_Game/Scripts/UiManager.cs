using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class UiManager : MonoBehaviour
    {
        [field: SerializeField] 
        private int scoreAddition = 10;
        
        [field: SerializeField]
        private TextMeshProUGUI scoreNumber;
        
        [field: SerializeField]
        private TextMeshProUGUI highScoreNumber;

        private int _currentScore;
        private int _currentHighScore;

        public int CurrentHighScore
        {
            get => _currentHighScore;
            set => _currentHighScore = value;
        }        

        public void LoadScoreSetter(int score)
        {
            CurrentHighScore = score;
            highScoreNumber.text = CurrentHighScore.ToString();
        }

        public void SetScoreText()
        {
            _currentScore += scoreAddition;
            
            if (CurrentHighScore < _currentScore)
                CurrentHighScore = _currentScore;

            scoreNumber.text = _currentScore.ToString();
            highScoreNumber.text = CurrentHighScore.ToString();
        }
    }
}
