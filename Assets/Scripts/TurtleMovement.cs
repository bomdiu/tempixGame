using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurtleMovement : MonoBehaviour
{
    private Tilemap tilemap;
    private float moveSpeed;
    private float jumpHeight;
    private float waitTime;
    private Vector3Int currentCell;   // Ô hiện tại của rùa
    private bool isMoving = false;    // Kiểm tra trạng thái di chuyển
    private int verticalMoves = 3;    // Số bước di chuyển dọc trước khi di chuyển ngang
    private int horizontalMoves = 2;  // Số bước di chuyển ngang
    private int moveCount = 0;        // Bộ đếm số bước đã di chuyển

    // Hàm khởi tạo thông số
    public void Initialize(Tilemap map, float speed, float jump, float wait)
    {
        tilemap = map;
        moveSpeed = speed;
        jumpHeight = jump;
        waitTime = wait;

        // Lấy ô hiện tại mà rùa đang đứng
        currentCell = tilemap.WorldToCell(transform.position);
        StartCoroutine(MoveTurtle());
    }

    private IEnumerator MoveTurtle()
    {
        while (true)
        {
            // Di chuyển xuống hoặc di chuyển ngang tùy vào số bước đã di chuyển
            Vector3Int targetCell;

            if (moveCount < verticalMoves) // Di chuyển xuống trong một vài bước đầu
            {
                targetCell = currentCell + new Vector3Int(0, -1, 0);
                moveCount++;
            }
            else if (moveCount < verticalMoves + horizontalMoves) // Di chuyển ngang sau khi di chuyển xuống
            {
                targetCell = currentCell + new Vector3Int(1, 0, 0); // Di chuyển ngang sang phải
                moveCount++;
            }
            else // Reset lại bộ đếm và bắt đầu lại chu kỳ
            {
                moveCount = 0;
                continue;
            }

            // Di chuyển đến ô tiếp theo
            yield return StartCoroutine(MoveToCell(targetCell));
            currentCell = targetCell; // Cập nhật ô hiện tại

            // Dừng lại trong khoảng thời gian `waitTime`
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator MoveToCell(Vector3Int targetCell)
    {
        if (isMoving) yield break;

        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = tilemap.GetCellCenterWorld(targetCell); // Tìm vị trí trung tâm của ô mục tiêu
        float elapsedTime = 0f;
        float moveDuration = Vector3.Distance(startPosition, endPosition) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            // Tính toán vị trí với hiệu ứng nhảy parabolic
            float horizontalPosition = Mathf.Lerp(startPosition.x, endPosition.x, elapsedTime / moveDuration);
            float verticalPosition = Mathf.Lerp(startPosition.y, endPosition.y, elapsedTime / moveDuration);
            float jumpOffset = Mathf.Sin(Mathf.PI * (elapsedTime / moveDuration)) * jumpHeight;

            transform.position = new Vector3(horizontalPosition, verticalPosition + jumpOffset, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Đặt vị trí đến trung tâm của ô mới
        isMoving = false; // Cho phép di chuyển tiếp
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die(); // Gọi hàm Die() trong script PlayerMovement
            }
        }
    }
}