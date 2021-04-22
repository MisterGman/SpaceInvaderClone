using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class StartGame : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
