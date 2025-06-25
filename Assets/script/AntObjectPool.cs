using System.Collections.Generic;
using UnityEngine;

namespace script
{
    public class AntObjectPool : MonoBehaviour
    {
        [Header("Pool Parents")] [SerializeField]
        private Transform antPool;

        [SerializeField] private Transform antDeadPool;

        [Header("Prefabs")] [SerializeField] private GameObject antPrefab;
        [SerializeField] private GameObject antDeadPrefab;

        [Header("Initial Pool Size")] [SerializeField]
        private int initialAntCount = 10;

        [SerializeField] private int initialDeadAntCount = 5;

        private Queue<GameObject> antPoolQueue = new Queue<GameObject>();
        private Queue<GameObject> antDeadPoolQueue = new Queue<GameObject>();

        private void Awake()
        {
            InitializePool(antPrefab, antPool, initialAntCount, antPoolQueue);
            InitializePool(antDeadPrefab, antDeadPool, initialDeadAntCount, antDeadPoolQueue);
        }

        private void OnEnable()
        {
            InstantiateAntEventCenter.OnInstantiateAnt += HandleSpawnAnt;
            InstantiateAntEventCenter.OnInstantiateAntDeadBody += HandleSpawnAntDeadBody;
        }

        private void OnDisable()
        {
            InstantiateAntEventCenter.OnInstantiateAnt -= HandleSpawnAnt;
            InstantiateAntEventCenter.OnInstantiateAntDeadBody -= HandleSpawnAntDeadBody;
        }
        
        private void HandleSpawnAnt()
        {
            GameObject ant = SpawnAnt();
            // 可在此做額外設定，如指定初始位置、播放動畫等
        }

        private void HandleSpawnAntDeadBody(Transform transform)
        {
            GameObject antDead = SpawnAntDeadBody();
            antDead.transform.position = transform.position;
            // 同上，可加入特效或特定行為
        }

        private void InitializePool(GameObject prefab, Transform parent, int count, Queue<GameObject> queue)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab, parent);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
        }

        private GameObject SpawnAnt()
        {
            return SpawnFromPool(antPoolQueue, antPrefab, antPool);
        }

        private GameObject SpawnAntDeadBody()
        {
            return SpawnFromPool(antDeadPoolQueue, antDeadPrefab, antDeadPool);
        }

        private GameObject SpawnFromPool(Queue<GameObject> poolQueue, GameObject prefab, Transform parent)
        {
            GameObject obj;

            obj = poolQueue.Count > 0 ? poolQueue.Dequeue() : Instantiate(prefab, parent);
            obj.transform.SetParent(parent); // 如果你希望出來的物件不是在 pool 裡
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToAntPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(antPool);
            antPoolQueue.Enqueue(obj);
        }

        public void ReturnToDeadPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(antDeadPool);
            antDeadPoolQueue.Enqueue(obj);
        }
    }
}