using UnityEngine;

namespace script
{
    public class MouseDragRotate : MonoBehaviour
    {
        public float rotationSpeed = 5.0f;

        private Vector3 _lastMousePosition;
        private bool _isDragging = false;

        void Update()
        {
            // 滑鼠左鍵按下
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                _lastMousePosition = Input.mousePosition;
            }

            // 滑鼠左鍵釋放
            if (Input.GetMouseButtonUp(1))
            {
                _isDragging = false;
            }

            // 拖曳中，執行旋轉
            if (_isDragging)
            {
                Vector3 delta = Input.mousePosition - _lastMousePosition;
                float xRotation = delta.y * rotationSpeed * Time.deltaTime;
                float yRotation = -delta.x * rotationSpeed * Time.deltaTime;

                transform.Rotate(xRotation, yRotation, 0, Space.World);
                _lastMousePosition = Input.mousePosition;
            }
        }
    }
}