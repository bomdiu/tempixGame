using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps; // Nhớ thêm thư viện này để làm việc với Tilemap

public class CrabMovement : MonoBehaviour
{
    public Tilemap tilemap;              // Tham chiếu đến tilemap
    public float moveDistance = 1f;      // Khoảng cách di chuyển mỗi lần
    public float moveSpeed = 3f;         // Tốc độ di chuyển
    public float jumpHeight = 0.3f;      // Chiều cao nhảy
    public float waitTime = 0.5f;        // Thời gian chờ giữa các lần di chuyển
    private bool isMoving = false;       // Kiểm tra nếu cua đang di chuyển
    private Vector2 direction;            // Hướng di chuyển cố định

    private void Start()
    {
        direction = Vector2.right; // Chỉ định hướng di chuyển cố định (có thể thay đổi sang trái bằng Vector2.left)
        StartCoroutine(MoveWithDelay());
    }

    private IEnumerator MoveWithDelay()
    {
        while (true)
        {
            yield return StartCoroutine(MoveWithJump());
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator MoveWithJump()
    {
        if (isMoving) yield break;

        isMoving = true;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * moveDistance;

        // Căn chỉnh endPosition với trung tâm ô trên tilemap
        endPosition = tilemap.GetCellCenterWorld(tilemap.WorldToCell(endPosition));

        float elapsedTime = 0f;
        float moveDuration = Vector2.Distance(startPosition, endPosition) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            float horizontalPosition = Mathf.Lerp(startPosition.x, endPosition.x, elapsedTime / moveDuration);
            float verticalPosition = Mathf.Lerp(startPosition.y, endPosition.y, elapsedTime / moveDuration);
            float jumpOffset = Mathf.Sin(Mathf.PI * (elapsedTime / moveDuration)) * jumpHeight;
            transform.position = new Vector2(horizontalPosition, verticalPosition + jumpOffset);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Đặt vị trí đến trung tâm của ô mới
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
