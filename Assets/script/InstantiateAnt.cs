using System.Collections;
using UnityEngine;

namespace script
{
    public class InstantiateAnt : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(nameof(AutoSpawnAnt));
        }

        IEnumerator AutoSpawnAnt()
        {
            while (true)
            {
                float waitTime = Random.Range(1f, 15f);
                yield return new WaitForSeconds(waitTime);

                InstantiateAntEvent(); // 呼叫你的螞蟻生成事件
            }
            // ReSharper disable once IteratorNeverReturns
        }
        
        private void InstantiateAntEvent()
        {
            InstantiateAntEventCenter.DoInstantiateAnt();
        }
    }
}