using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReverseCrabManager : MonoBehaviour
{
    public GameObject reverseCrabPrefab;    // Prefab cua ngược
    public Tilemap tilemap;                  // Tilemap để lấy giới hạn di chuyển
    public float gapBetweenCrabs = 1.5f;     // Khoảng cách giữa các con cua
    public int initialReverseCrabs = 10;     // Số lượng cua ngược khởi tạo trên màn hình
    private List<GameObject> reverseCrabs = new List<GameObject>(); // Danh sách cua ngược hiện có
    private float leftBoundary;               // Giới hạn trái của Tilemap
    private float rightBoundary;              // Giới hạn phải của Tilemap

    private void Start()
    {
        // Lấy giới hạn của Tilemap
        Bounds tilemapBounds = tilemap.localBounds;
        leftBoundary = tilemapBounds.min.x;
        rightBoundary = tilemapBounds.max.x;

        // Khởi tạo các cua ngược ban đầu
        for (int i = 0; i < initialReverseCrabs; i++)
        {
            Vector2 spawnPosition = new Vector2(rightBoundary - i * gapBetweenCrabs, transform.position.y);
            GameObject newReverseCrab = Instantiate(reverseCrabPrefab, spawnPosition, Quaternion.identity);
            reverseCrabs.Add(newReverseCrab);
        }
    }

    private void Update()
    {
        // Kiểm tra từng con cua ngược trong danh sách
        for (int i = 0; i < reverseCrabs.Count; i++)
        {
            GameObject reverseCrab = reverseCrabs[i];

            // Nếu con cua ngược vượt quá giới hạn trái, đưa nó về cuối hàng
            if (reverseCrab.transform.position.x < leftBoundary)
            {
                reverseCrab.transform.position = new Vector2(rightBoundary, reverseCrab.transform.position.y);
            }
        }
    }
}
