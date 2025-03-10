using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;           // Đối tượng người chơi
    public Vector3 offset = new Vector3(0, 2, -10);  // Khoảng cách giữa camera và nhân vật

    [Header("Camera Smoothing")]
    public float smoothSpeed = 0.125f; // Tốc độ mượt mà khi di chuyển camera

    [Header("Camera Bounds")]
    public bool useBounds = false;     // Bật/Tắt giới hạn camera
    public float minX, maxX, minY, maxY; // Các giới hạn của camera

    void Start()
    {
        // Tự động tìm đối tượng người chơi nếu không gán trước
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (player == null) return; // Nếu không tìm thấy Player, thoát hàm

        // Tính toán vị trí mục tiêu cho camera
        Vector3 targetPosition = player.position + offset;

        // Áp dụng giới hạn camera nếu bật tùy chọn
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }

        // Di chuyển camera đến vị trí mục tiêu với hiệu ứng mượt mà
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
