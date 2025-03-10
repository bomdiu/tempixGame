using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBoundsExample : MonoBehaviour
{
    public Tilemap tilemap; // Kéo thả Tilemap của bạn vào đây
    private Vector3 leftBoundary;
    private Vector3 rightBoundary;

    void Start()
    {
        // Lấy bounds của Tilemap
        Bounds tilemapBounds = tilemap.localBounds;

        // Xác định giới hạn trái và phải
        leftBoundary = new Vector3(tilemapBounds.min.x, tilemapBounds.center.y, 0);
        rightBoundary = new Vector3(tilemapBounds.max.x, tilemapBounds.center.y, 0);

        // In ra các giá trị giới hạn (có thể xem trong Console)
        Debug.Log("Left Boundary: " + leftBoundary);
        Debug.Log("Right Boundary: " + rightBoundary);
    }
}
