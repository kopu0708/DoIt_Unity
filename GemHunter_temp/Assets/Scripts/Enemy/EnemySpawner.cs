using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private int EnemyCount = 10;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private EntityBase target;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

    public static List<EntityBase> Enemies { get; private set; } =
        new List<EntityBase>();  // 플레이어가 공격할 목표를 리스트로 저장 외부에서 쉽게 접근 가능하게 static으로 선언 
    private void Awake()
    {
        //Tilemap의 Bounds 재설정(맵을 수정할 때 Bounds가 변경되지 않는 문제 해결)
        tilemap.CompressBounds();
        //타일맵의 모든 타일을 대상으로 적을 배치할 수 있는 타일 계산
        CalculatePossibleTiles();

        //임의의 타일에 적 10기 생성
        for(int i = 0; i < EnemyCount; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int index = Random.Range(0, possibleTiles.Count);

            GameObject clone = Instantiate(enemyPrefabs[type], possibleTiles[index],
                Quaternion.identity, transform); //그냥 회전 안 시키고 원본 각도 그대로 생성
            clone.GetComponent<EnemyBase>().Initialize(parentTransform);  // 적 스폰에 Initialize 호출 
            clone.GetComponent<EnemyFSM>().Setup(target);
            Enemies.Add(clone.GetComponent<EntityBase>()); //적이 추가되면 리스트에 추가
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
}
