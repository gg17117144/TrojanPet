using System;
using UnityEngine;

namespace script
{
    public class InstantiateAntEventCenter : MonoBehaviour
    {
        /// <summary>
        /// 要生成螞蟻時呼叫
        /// </summary>
        public static event Action OnInstantiateAnt;

        public static void DoInstantiateAnt()
        {
            OnInstantiateAnt?.Invoke();
        }

        /// <summary>
        /// 要生成螞蟻屍體時呼叫
        /// </summary>
        public static event Action<Transform> OnInstantiateAntDeadBody;

        public static void DoInstantiateAntDeadBody(Transform transform)
        {
            OnInstantiateAntDeadBody?.Invoke(transform);
        }
    }
}