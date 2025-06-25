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
        private bool _isDead = false;
        private float _timer = 0f;
        private AntAnimatorController _antAnimatorController;
        private Vector3 _screenMin, _screenMax;

        void OnEnable()
        {
            _antAnimatorController = GetComponent<AntAnimatorController>();
            if (targetCamera == null)
                targetCamera = Camera.main;

            if (targetCamera != null)
            {
                // 取得 X-Z 平面的邊界
                _screenMin = targetCamera.ScreenToWorldPoint(new Vector3(0, 0, targetCamera.transform.position.y));
                _screenMax = targetCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    targetCamera.transform.position.y));
            }

            PickNewTarget();
            
            transform.position = PickOutsideScreenTarget();
        }

        void Update()
        {
            if (_isDead) return;
            AntMove();
        }
        
        /// <summary>
        /// 螞蟻移動
        /// </summary>
        void AntMove()
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

                if (_antAnimatorController)
                    _antAnimatorController.PlayAnimation(AntAnimationType.Idle);
            }
        }

        /// <summary>
        /// 找尋新的移動目標
        /// </summary>
        void PickNewTarget()
        {
            _targetPosition = new Vector3(
                Random.Range(_screenMin.x, _screenMax.x),
                transform.position.y,
                Random.Range(_screenMin.z, _screenMax.z)
            );
            
            if (_antAnimatorController)
                _antAnimatorController.PlayAnimation(AntAnimationType.Walking);
        }
        
        private Vector3 PickOutsideScreenTarget()
        {
            // 假設螢幕範圍來自 _screenMin / _screenMax（XZ 平面）
            float margin = 2f; // 多遠算是「螢幕外」
            int edge = Random.Range(0, 4); // 0=左, 1=右, 2=上, 3=下
            Vector3 position = Vector3.zero;
            float y = transform.position.y;

            switch (edge)
            {
                case 0: // 左
                    position = new Vector3(_screenMin.x - margin, y, Random.Range(_screenMin.z, _screenMax.z));
                    break;
                case 1: // 右
                    position = new Vector3(_screenMax.x + margin, y, Random.Range(_screenMin.z, _screenMax.z));
                    break;
                case 2: // 上
                    position = new Vector3(Random.Range(_screenMin.x, _screenMax.x), y, _screenMax.z + margin);
                    break;
                case 3: // 下
                    position = new Vector3(Random.Range(_screenMin.x, _screenMax.x), y, _screenMin.z - margin);
                    break;
            }

            return position;
        }

        void OnMouseDown()
        {
            Debug.Log("🐜 螞蟻被點到了！");
            AntDead();
        }

        private void AntDead()
        {
            if (_isDead) return;
            _isDead = true;
            if (_antAnimatorController)
                _antAnimatorController.PlayAnimation(AntAnimationType.Die);
            Debug.Log("🐜 螞蟻死掉了！");
            InstantiateAntEventCenter.DoInstantiateAntDeadBody(transform);
        }
    }
}