using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurtleManager : MonoBehaviour
{
    public GameObject turtlePrefab;       // Prefab của rùa
    public Tilemap tilemap;               // Tilemap để căn chỉnh vị trí di chuyển
    public Vector3 startPosition;         // Vị trí bắt đầu hàng rùa trong Tilemap
    public float gapBetweenTurtles = 1.5f; // Khoảng cách giữa các rùa
    public int initialTurtles = 5;        // Số lượng rùa ban đầu
    public float moveSpeed = 3f;          // Tốc độ di chuyển của rùa
    public float jumpHeight = 0.3f;       // Chiều cao nhảy của rùa
    public float waitTime = 0.5f;         // Thời gian chờ giữa các lần di chuyển

    private List<GameObject> turtles = new List<GameObject>(); // Danh sách rùa hiện có

    private void Start()
    {
        // Khởi tạo các rùa ban đầu tại vị trí `startPosition`
        for (int i = 0; i < initialTurtles; i++)
        {
            Vector3 spawnPosition = startPosition + new Vector3(0, -i * gapBetweenTurtles, 0);
            GameObject newTurtle = Instantiate(turtlePrefab, spawnPosition, Quaternion.identity);
            turtles.Add(newTurtle);

            // Bắt đầu di chuyển từng rùa
            TurtleMovement turtleMovement = newTurtle.GetComponent<TurtleMovement>();
            if (turtleMovement != null)
            {
                turtleMovement.Initialize(tilemap, moveSpeed, jumpHeight, waitTime);
            }
        }
    }
}
