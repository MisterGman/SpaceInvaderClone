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

        public int HighScoreNumber { get; private set; }

        public void LoadScoreSetter(int score)
        {
            HighScoreNumber = score;
            highScoreNumber.text = HighScoreNumber.ToString();
        }

        public void SetScoreText()
        {
            _currentScore += scoreAddition;
            
            if (HighScoreNumber < _currentScore)
                HighScoreNumber = _currentScore;

            scoreNumber.text = _currentScore.ToString();
            highScoreNumber.text = HighScoreNumber.ToString();
        }

        //Game over 
        //time scale
        //yepta
    }
}
