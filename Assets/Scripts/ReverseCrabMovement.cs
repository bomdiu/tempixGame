using System.Collections;
using UnityEngine;

public class ReverseCrabMovement : MonoBehaviour
{
    public float moveDistance = 1f;       // Khoảng cách di chuyển mỗi lần (từng ô)
    public float moveSpeed = 3f;          // Tốc độ di chuyển đến ô tiếp theo
    public float jumpHeight = 0.3f;       // Chiều cao nhảy khi di chuyển
    public float waitTime = 0.5f;         // Thời gian dừng lại giữa mỗi lần di chuyển
    private bool isMoving = false;        // Kiểm tra nếu cua đang di chuyển
    private Vector2 direction = Vector2.left; // Hướng di chuyển (ngược lại với cua gốc)

    private void Start()
    {
        StartCoroutine(MoveWithDelay());
    }

    private IEnumerator MoveWithDelay()
    {
        while (true)
        {
            yield return StartCoroutine(MoveWithJump());
            yield return new WaitForSeconds(waitTime); // Dừng lại một nhịp sau khi di chuyển
        }
    }

    private IEnumerator MoveWithJump()
    {
        if (isMoving) yield break;

        isMoving = true;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * moveDistance;

        float elapsedTime = 0f;
        float moveDuration = moveDistance / moveSpeed;

        // Di chuyển cua từ vị trí bắt đầu đến vị trí đích với hiệu ứng nhảy
        while (elapsedTime < moveDuration)
        {
            float horizontalPosition = Mathf.Lerp(startPosition.x, endPosition.x, elapsedTime / moveDuration);
            float verticalPosition = Mathf.Lerp(startPosition.y, endPosition.y, elapsedTime / moveDuration);
            float jumpOffset = Mathf.Sin(Mathf.PI * (elapsedTime / moveDuration)) * jumpHeight;
            transform.position = new Vector2(horizontalPosition, verticalPosition + jumpOffset);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Đặt vị trí cua tới ô cuối cùng
        isMoving = false; // Cho phép di chuyển tiếp
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Kiểm tra va chạm với người chơi
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die(); // Xử lý khi cua va chạm với người chơi
            }
        }
    }
}
