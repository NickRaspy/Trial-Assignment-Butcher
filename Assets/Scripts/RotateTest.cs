using UnityEngine;

public class RotateTest : MonoBehaviour
{

    [SerializeField] private float rotationAngle = 15f;
    [SerializeField] private float returnSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RotateCharacter(Input.mousePosition);
        }
        else ReturnToOriginalRotation();
    }
    private void RotateCharacter(Vector3 mousePosition)
    {
        Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
        float offsetX = viewportPosition.x - 0.5f; // Смещение относительно центра экрана (0.5, 0.5)
        float targetAngle = Mathf.Clamp(offsetX * 2 * rotationAngle, -rotationAngle, rotationAngle);

        transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    private void ReturnToOriginalRotation()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
    }
}
