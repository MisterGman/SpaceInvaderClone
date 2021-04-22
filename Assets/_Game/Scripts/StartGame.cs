using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class StartGame : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
