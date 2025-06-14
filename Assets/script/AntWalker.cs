using UnityEngine;

namespace script
{
    public class AntWalker : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float idleTime = 1.5f;
        public Camera targetCamera;

        private Vector3 _targetPosition;
        private bool _isIdle = false;
        private float _timer = 0f;
        private Animator _animator;
        private Vector3 _screenMin, _screenMax;

        void Start()
        {
            _animator = GetComponent<Animator>();
            if (targetCamera == null)
                targetCamera = Camera.main;

            if (targetCamera != null)
            {
                // 取得 X-Z 平面的邊界
                _screenMin = targetCamera.ScreenToWorldPoint(new Vector3(0, 0, targetCamera.transform.position.y));
                _screenMax = targetCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, targetCamera.transform.position.y));
            }

            PickNewTarget();
        }

        void Update()
        {
            if (_isIdle)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _isIdle = false;
                    PickNewTarget();
                }
                return;
            }

            Vector3 direction = (_targetPosition - transform.position);
            direction.y = 0; // 只在 X-Z 平面移動

            if (direction.magnitude > 0.05f)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;

                // 朝向移動方向（只轉 Y 軸）
// 平滑轉向（含模型朝 X+ 的補正 -90 度）
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0f, 90f, 0f); // 模型補正

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            }
            else
            {
                _isIdle = true;
                _timer = idleTime;

                if (_animator)
                    _animator.SetBool("IsWalking", false);
            }
        }

        void PickNewTarget()
        {
            _targetPosition = new Vector3(
                Random.Range(_screenMin.x, _screenMax.x),
                transform.position.y,
                Random.Range(_screenMin.z, _screenMax.z)
            );

            if (_animator)
                _animator.SetBool("IsWalking", true);
        }
    }
}
