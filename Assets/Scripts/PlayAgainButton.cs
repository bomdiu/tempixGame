using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    public void OnPlayAgainButtonClick()
    {
         Debug.Log("Play Again Button Clicked!"); // Thông báo trong Console
        Time.timeScale = 1f; // Khôi phục thời gian
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại scene hiện tại
    }
}
