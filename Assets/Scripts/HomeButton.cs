using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public void OnHomeButtonClick()
    {
        Time.timeScale = 1f; // Khôi phục thời gian
        SceneManager.LoadScene("HomeScene"); // Thay "HomeScene" bằng tên scene chính của bạn
    }
}
