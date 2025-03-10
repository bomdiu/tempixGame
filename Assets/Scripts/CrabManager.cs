using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrabManager : MonoBehaviour
{
    public GameObject crabPrefab;           // Prefab cua
    public Tilemap tilemap;                 // Tilemap để lấy giới hạn di chuyển
    public float gapBetweenCrabs = 1.5f;    // Khoảng cách giữa các con cua
    public int initialCrabs = 10;           // Số lượng cua khởi tạo trên màn hình
    private List<GameObject> crabs = new List<GameObject>(); // Danh sách cua hiện có
    private float leftBoundary;              // Giới hạn trái của Tilemap
    private float rightBoundary;             // Giới hạn phải của Tilemap

    private void Start()
    {
        // Lấy giới hạn của Tilemap
        Bounds tilemapBounds = tilemap.localBounds;
        leftBoundary = tilemapBounds.min.x;
        rightBoundary = tilemapBounds.max.x;

        // Khởi tạo các cua ban đầu
        for (int i = 0; i < initialCrabs; i++)
        {
            Vector2 spawnPosition = new Vector2(leftBoundary + i * gapBetweenCrabs, transform.position.y);
            GameObject newCrab = Instantiate(crabPrefab, spawnPosition, Quaternion.identity);
            crabs.Add(newCrab);
        }
    }

    private void Update()
    {
        // Kiểm tra từng con cua trong danh sách
        for (int i = 0; i < crabs.Count; i++)
        {
            GameObject crab = crabs[i];

            // Nếu con cua vượt quá giới hạn phải, đưa nó về đầu hàng
            if (crab.transform.position.x > rightBoundary)
            {
                crab.transform.position = new Vector2(leftBoundary, crab.transform.position.y);
            }

            // Nếu con cua vượt quá giới hạn trái, tạo con cua mới ở hướng ngược lại
            if (crab.transform.position.x < leftBoundary)
            {
                Vector2 spawnPosition = new Vector2(rightBoundary - gapBetweenCrabs, crab.transform.position.y);
                GameObject newCrab = Instantiate(crabPrefab, spawnPosition, Quaternion.identity);
                crabs.Add(newCrab);
            }
        }
    }
}
