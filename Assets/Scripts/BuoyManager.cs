using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuoyManager : MonoBehaviour
{
    public Tilemap tilemap;              // Tham chiếu đến Tilemap
    public Tile buoyTile;                // Tile đại diện cho phao cứu hộ
    public float appearDuration = 1f;    // Thời gian phao hiện
    public float disappearDuration = 1f; // Thời gian phao ẩn
    public Vector3Int[] buoyPositions;   // Các vị trí trong Tilemap mà phao sẽ xuất hiện

    private void Start()
    {
        // Bắt đầu hệ thống phao ẩn hiện
        StartCoroutine(BuoyWaveRoutine());
    }

    private IEnumerator BuoyWaveRoutine()
    {
        while (true)
        {
            // Phao xuất hiện
            foreach (Vector3Int position in buoyPositions)
            {
                tilemap.SetTile(position, buoyTile); // Đặt tile phao vào vị trí
            }
            yield return new WaitForSeconds(appearDuration); // Chờ thời gian hiện

            // Phao ẩn đi
            foreach (Vector3Int position in buoyPositions)
            {
                tilemap.SetTile(position, null); // Xóa tile phao tại vị trí
            }
            yield return new WaitForSeconds(disappearDuration); // Chờ thời gian ẩn
        }
    }
}
