using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Events;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject enemySpawnTile;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GemCollecter gemCollecter;
    [SerializeField]
    private EntityBase target;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();
    private MemoryPool enemySpawnTilePool; 
    private WaitForSeconds waitTime = new WaitForSeconds(2.0f);
    public static UnityEvent exitEvent = new UnityEvent();
    public static List<EntityBase> Enemies { get; private set; } =
        new List<EntityBase>();  // 플레이어가 공격할 목표를 리스트로 저장 외부에서 쉽게 접근 가능하게 static으로 선언 

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }
    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        enemySpawnTilePool = new MemoryPool(enemySpawnTile);

        //Tilemap의 Bounds 재설정(맵을 수정할 때 Bounds가 변경되지 않는 문제 해결)
        tilemap.CompressBounds();
        //타일맵의 모든 타일을 대상으로 적을 배치할 수 있는 타일 계산
        CalculatePossibleTiles();
    }

    public void SpawnEnemys(int count)
    {
        Enemies.Clear();
        StartCoroutine(nameof(Process), count);
    }

    private IEnumerator Process(int count)
    {
        Vector3[] positions = new Vector3[count];
        for(int i = 0; i < count; ++i)
        {
            // 적을 배치할 임의의 위치 설정
            positions[i] = possibleTiles[Random.Range(0, possibleTiles.Count)];
            // 적이 배치될 위치에 타일 생성
            enemySpawnTilePool.ActivatePoolItem(positions[i]);
        }

        yield return waitTime;

        // 모든 타일 삭제
        enemySpawnTilePool.DeactivateAllPoolItems();
        // 적 생성 
        for (int i = 0; i < count; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int wayIndex = Random.Range(0, wayPointData.Length);

            GameObject clone = Instantiate(
                enemyPrefabs[type], positions[i], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(
                this, parentTransform, gemCollecter);
            clone.GetComponent<EnemyFSM>().Setup(
                target, wayPointData[wayIndex].wayPoints);

            Enemies.Add(clone.GetComponent<EntityBase>());
        }
    }
    private void CalculatePossibleTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // 외곽 벽에 붙은 타일은 제외하고
        // x, y의 시작값은 1, 끝값은 bounds.size.x - 1, bounds.size.y -1 
        for(int y = 1; y < bounds.size.y - 1; ++y)
        {
            for(int x = 1; x < bounds.size.x -1; ++x)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];

                if(tile!= null)
                {
                    Vector3Int localPosition = bounds.position +
                        new Vector3Int(x, y);
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    position.z = 0;

                    possibleTiles.Add(position);
                }
            }
        }
    }

    public void Deactivate(EnemyBase enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);

        if(Enemies.Count == 0)
        {
            exitEvent?.Invoke();
        }
    }
}
