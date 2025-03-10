using UnityEngine;
using TMPro;  // Nếu sử dụng TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 60f;  // Thời gian bắt đầu đếm ngược (ví dụ 60 giây)
    public bool timerIsRunning = false; // Kiểm tra xem bộ đếm có đang chạy không
    public TextMeshProUGUI timerText;   // Tham chiếu tới TextMeshPro trong UI

    void Start()
    {
        // Đảm bảo timer bắt đầu chạy
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            // Kiểm tra nếu còn thời gian
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;  // Giảm thời gian mỗi frame
                DisplayTime(timeRemaining);  // Hiển thị thời gian còn lại
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;  // Dừng bộ đếm khi hết thời gian
                Debug.Log("Time's up!");  // Có thể thay bằng hành động khác khi hết thời gian
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Tính toán phút và giây còn lại
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  // Lấy phút
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // Lấy giây

        // Hiển thị dưới định dạng "phút:giây" (ví dụ "02:30")
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
