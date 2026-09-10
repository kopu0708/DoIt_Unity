using UnityEngine;
using System.Collections.Generic;
public class MemoryPool 
{
   // 메모리 풀로 관리하는 오브젝트 정보
   private class PoolItem
    {
        public GameObject gameObject; // 화면에 보이는 실제 오브젝트
        private bool isActive; // gameObject 활성화/비활성화 정보

        public bool IsActive // 프로퍼티 필드값을 읽고 쓴다. 
        {
            set
            {
                isActive = value;
                gameObject.SetActive(isActive);
            }

            get => isActive;
        }
    }

    private GameObject poolObject; // 오브젝트 풀링 메니저에서 관리하는 게임오브젝트 프리팹
    private List<PoolItem> poolItemList; // 관리하는 모든 오브젝트를 저장하는 리스트

    private readonly int increaseCount = 5; // 오브젝트가 부족할 때 Instantiate()로 추가할 오브젝트 개수 

    public int MaxCount { private set; get; } // 현재 리스트에 등록된 오브젝트 개수
    public int ActiveCount { private set; get; } // 현재 활성 상태인 오브젝트 개수 

    // 오브젝트를 임시로 보관할 위치 
    private Vector3 tempPosition =
        new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    public MemoryPool(GameObject poolObject)
    {
        MaxCount = 0;
        ActiveCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    /// <summary>
    /// increaseCount 단위로 오브젝트를 생성한다.
    /// </summary>
    public void InstantiateObjects()
    {
        MaxCount += increaseCount;

        for(int i = 0; i < increaseCount; ++i) // increaseCount 단위로 생성해야하니깐 
        {
            PoolItem poolItem = new PoolItem(); // 객체를 새롭게 만들고 

            poolItem.gameObject = GameObject.Instantiate(poolObject); // 실제로 씬에 나올수 있게 프리팹을 통해 생성
            poolItem.gameObject.transform.position = tempPosition; // 화면 밖으로 치워둠 
            poolItem.IsActive = false; // gameObject를 비활성화(SetActive(false))해서 화면에 안 보이고 동작도 멈추게 함

            poolItemList.Add(poolItem);
        }
    }

    /// <summary>
    /// 현재 관리 중인(활성화/비활성화) 모든 오브젝트를 삭제한다.
    /// </summary>
    public void DestroyObjects() // 씬이 바뀌거나 더 이상 이 풀이 필요없을 때 메모리를 확실히 비우기 위해 있는 로직 
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i<count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear(); // 리스트를 비워주는 함수 
    }

    /// <summary>
    /// poolItemList에 저장된 오브젝트를 활성화해서 사용한다.
    /// 모든 오브젝트가 사용 중이면 InstatiateObjects()로 추가 생성한다. 
    /// </summary>
    public GameObject ActivatePoolItem(Vector3 position)
    {
        if (poolItemList == null) return null;
        
        // 생성해서 관리하는 모든 오브젝트와 현재 오브젝트 개수 비교 
        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 필요
        if( MaxCount == ActiveCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.IsActive == false)
            {
                ActiveCount++;

                poolItem.gameObject.transform.position = position;
                poolItem.IsActive = true;

                return poolItem.gameObject;
            }
        }

        return null;
    }
    /// <summary>
    /// 사용이 끝난 오브젝트를 비활성화 상태로 설정한다.
    /// </summary>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject == removeObject)
            {
                ActiveCount--;

                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;

                return;
            }
        }
    }

    /// <summary>
    /// 게임에 사용 중인 모든 오브젝트를 비활성화 상태로 설정한다.
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.IsActive == true)
            {
                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;
            }
        }
        ActiveCount = 0;
    }
}
