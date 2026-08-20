using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public GameObject moveMarker;

    [Header("Legs")]
    public Transform leftLeg;
    public Transform rightLeg;

    public float legSpeed = 10f;
    public float legAngle = 25f;

    private Vector3 targetPosition;
    private bool moving;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        targetPosition = transform.position;

        if (moveMarker != null)
            moveMarker.SetActive(false);
    }

    private void Update()
    {
        void Update()
        {
            if (GameManager.gameEnded)
            {
                return;
            }
            // Не двигаться если нажали UI
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
            }

            // Остальной код движения ниже

            // Если открыт режим строительства — полностью отключаем движение
            if (BuildSystem.Instance != null && BuildSystem.Instance.building)
            {
                moving = false;

                if (moveMarker != null)
                    moveMarker.SetActive(false);

                if (leftLeg != null)
                    leftLeg.localRotation = Quaternion.identity;

                if (rightLeg != null)
                    rightLeg.localRotation = Quaternion.identity;

                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
              RaycastHit2D hit = Physics2D.Raycast(
              Camera.main.ScreenToWorldPoint(Input.mousePosition),
              Vector2.zero
);


            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Turret>() != null)
                {
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                targetPosition = pos;
                moving = true;
            }

            if (moving)
            {
                Vector3 direction = targetPosition - transform.position;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    speed * Time.deltaTime);

                if (direction.x > 0.05f)
                {
                    transform.localScale = new Vector3(
                        Mathf.Abs(originalScale.x),
                        originalScale.y,
                        originalScale.z);
                }
                else if (direction.x < -0.05f)
                {
                    transform.localScale = new Vector3(
                        -Mathf.Abs(originalScale.x),
                        originalScale.y,
                        originalScale.z);
                }

                float angle = Mathf.Sin(Time.time * legSpeed) * legAngle;

                if (rightLeg != null)
                    rightLeg.localRotation = Quaternion.Euler(0, 0, angle);

                if (leftLeg != null)
                    leftLeg.localRotation = Quaternion.Euler(0, 0, -angle);

                if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
                {
                    moving = false;

                    if (moveMarker != null)
                        moveMarker.SetActive(false);

                    if (leftLeg != null)
                        leftLeg.localRotation = Quaternion.identity;

                    if (rightLeg != null)
                        rightLeg.localRotation = Quaternion.identity;
                }
            }
        }
    }
}