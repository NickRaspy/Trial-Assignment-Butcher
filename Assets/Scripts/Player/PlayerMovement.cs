using UnityEngine;

namespace Butcher_TA
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private Transform control;
        [SerializeField] private Transform modelPoint;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float borderLimit = 1f;
        [SerializeField] private float rotationAngle = 15f;
        [SerializeField] private float returnSpeed = 2f;

        public bool CanMove { get; set; }

        private float moveX;

        private void Update()
        {
            if (CanMove)
            {
                if (Input.GetMouseButton(0))
                {
                    Move();
                }
                if (moveX != 0f) RotateCharacter(Input.mousePosition);
                else ReturnToOriginalRotation();
            }
            else ReturnToOriginalRotation();

            control.localPosition = new Vector3(Mathf.Clamp(control.localPosition.x, -borderLimit, borderLimit), 0f, 0f);
        }

        public void Move()
        {
            moveX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            control.Translate(moveX, 0f, 0f);
        }

        public void RotateCharacter(Vector3 mousePosition)
        {
            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
            float offsetX = viewportPosition.x - 0.5f;
            float targetAngle = Mathf.Clamp(offsetX * rotationAngle * 2, -rotationAngle, rotationAngle);

            modelPoint.localRotation = Quaternion.Euler(0f, targetAngle, 0f);
        }

        public void ReturnToOriginalRotation()
        {
            modelPoint.localRotation = Quaternion.Lerp(modelPoint.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
        }
    }

    public interface IPlayerMovement
    {
        void Move();
        void RotateCharacter(Vector3 mousePosition);
        void ReturnToOriginalRotation();
        bool CanMove { get; set; }
    }
}