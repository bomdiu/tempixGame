using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap waterTilemap;
    public Tilemap buoyTilemap;
    public Tilemap noteTilemap; // Tilemap chứa các nốt nhạc
    public float moveSpeed = 5f;
    public float jumpHeight = 0.5f;
    private bool isMoving = false;
    private Vector3Int currentCell;
    private Vector2 lastDirection = Vector2.zero;

    public int score = 0; // Điểm số (nốt nhạc đã thu thập)
    public TMP_Text scoreText; // Tham chiếu đến text UI hiển thị số điểm
    public TMP_Text notesCollectedText; // Tham chiếu đến text UI hiển thị số lượng nốt nhạc
    public GameObject gameOverPanel; // Tham chiếu đến GameOverPanel
    public GameObject levelCompletedPanel; // Tham chiếu đến UI "Level Completed"
    public AudioSource backgroundMusic; // Tham chiếu đến nhạc nền
    public AudioSource gameOverSound; // Tham chiếu đến âm thanh game over
    public AudioSource collectNoteSound; // Tham chiếu đến âm thanh thu thập nốt nhạc
    public AudioSource levelCompleteSound; // Tham chiếu đến âm thanh hoàn thành màn chơi

    public bool IsMoving => isMoving;
    public string MoveDirection { get; private set; }

    private int collectedNotes = 0; // Biến đếm số lượng nốt nhạc đã thu thập

    private void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
        MoveToCellCenter();
        UpdateScoreText();
        UpdateNotesCollectedText(); // Cập nhật UI hiển thị số lượng nốt nhạc
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector2 direction = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W)) direction = Vector2.up;
            if (Input.GetKeyDown(KeyCode.S)) direction = Vector2.down;
            if (Input.GetKeyDown(KeyCode.A)) direction = Vector2.left;
            if (Input.GetKeyDown(KeyCode.D)) direction = Vector2.right;

            if (direction != Vector2.zero)
            {
                if (direction == Vector2.up) MoveDirection = "north";
                else if (direction == Vector2.down) MoveDirection = "south";
                else if (direction == Vector2.left) MoveDirection = "west";
                else if (direction == Vector2.right) MoveDirection = "east";

                Vector3Int nextCell = currentCell + new Vector3Int((int)direction.x, (int)direction.y, 0);

                if (tilemap.HasTile(nextCell))
                {
                    if (direction.x != 0)
                    {
                        transform.rotation = Quaternion.Euler(0, direction.x < 0 ? 180 : 0, 0);
                    }

                    StartCoroutine(MoveWithJump(nextCell));
                }
            }
        }
    }

    private IEnumerator MoveWithJump(Vector3Int targetCell)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = tilemap.GetCellCenterWorld(targetCell);
        float elapsedTime = 0f;
        float moveDuration = Vector3.Distance(startPosition, endPosition) / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            float horizontalPosition = Mathf.Lerp(startPosition.x, endPosition.x, elapsedTime / moveDuration);
            float verticalPosition = Mathf.Lerp(startPosition.y, endPosition.y, elapsedTime / moveDuration);
            float jumpOffset = Mathf.Sin(Mathf.PI * (elapsedTime / moveDuration)) * jumpHeight;
            transform.position = new Vector3(horizontalPosition, verticalPosition + jumpOffset, startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        currentCell = targetCell;
        isMoving = false;

        CheckWaterAndBuoy();
        CollectNoteAtCell(targetCell); // Thu thập nốt nhạc nếu có
        if (MoveDirection == "north") // Chỉ tính điểm khi di chuyển lên (về hướng north)
        {
            AddScore(1); // Tăng điểm mỗi khi di chuyển lên 1 ô
        }
    }

    private void MoveToCellCenter()
    {
        Vector3 cellCenterPos = tilemap.GetCellCenterWorld(currentCell);
        transform.position = cellCenterPos;
    }

    private void CheckWaterAndBuoy()
    {
        if (IsInWater() && !IsOnBuoy())
        {
            StartCoroutine(DelayedDeath());
        }
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(0.5f); // Thời gian trì hoãn
        Die();
    }

    private bool IsOnBuoy()
    {
        Vector3Int playerCell = tilemap.WorldToCell(transform.position);
        return buoyTilemap.HasTile(playerCell);
    }

    private bool IsInWater()
    {
        Vector3Int playerCell = tilemap.WorldToCell(transform.position);
        return waterTilemap.HasTile(playerCell);
    }

    private void CollectNoteAtCell(Vector3Int cell)
    {
        if (noteTilemap.HasTile(cell))
        {
            noteTilemap.SetTile(cell, null); // Xóa nốt nhạc khỏi Tilemap
            collectedNotes++; // Tăng số lượng nốt nhạc đã thu thập
            AddScore(5); // Tăng điểm khi thu thập nốt nhạc (5 điểm mỗi lần)
            UpdateNotesCollectedText(); // Cập nhật UI số lượng nốt nhạc

            // Phát âm thanh thu thập nốt nhạc
            if (collectNoteSound != null)
            {
                collectNoteSound.Play();
            }

            // Kiểm tra nếu đã thu thập đủ 3 nốt nhạc
            if (collectedNotes >= 3)
            {
                LevelCompleted();
            }
        }
    }

    private void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score " + score;
        }
    }

    private void UpdateNotesCollectedText()
    {
        if (notesCollectedText != null)
        {
            notesCollectedText.text = " " + collectedNotes + "/3"; // Hiển thị số lượng nốt nhạc đã thu thập
        }
    }

    private void LevelCompleted()
    {
        Debug.Log("Level Completed!");
        levelCompletedPanel.SetActive(true); // Hiển thị UI "Level Completed"
        Time.timeScale = 0f; // Dừng mọi hoạt động trong scene
        backgroundMusic.Stop(); // Dừng nhạc nền

        // Phát âm thanh hoàn thành màn chơi
        if (levelCompleteSound != null)
        {
            levelCompleteSound.Play();
        }
    }

    public void Die()
    {
        Debug.Log("Player is dead!");
        gameOverPanel.SetActive(true); // Hiển thị UI Game Over
        Time.timeScale = 0f; // Dừng mọi hoạt động trong scene
        backgroundMusic.Stop(); // Dừng nhạc nền
        gameOverSound.Play(); // Phát âm thanh game over
    }
}
