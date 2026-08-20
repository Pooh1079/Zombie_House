using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    public SimpleJoystick joystick;

    public float speed = 5f;

    public Transform leftLeg;
    public Transform rightLeg;

    public float legSpeed = 10f;
    public float legAngle = 25f;

    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            return;
        }
        
        Vector2 dir = joystick.Direction;

        Vector3 move =
            new Vector3(dir.x, dir.y, 0);

        transform.position += move * speed * Time.deltaTime;

        if (dir.x > 0.1f)
        {
            transform.localScale =
                new Vector3(
                    Mathf.Abs(startScale.x),
                    startScale.y,
                    startScale.z);
        }

        if (dir.x < -0.1f)
        {
            transform.localScale =
                new Vector3(
                    -Mathf.Abs(startScale.x),
                    startScale.y,
                    startScale.z);
        }

        if (move.magnitude > 0.1f)
        {
            float angle =
                Mathf.Sin(Time.time * legSpeed) * legAngle;

            if (leftLeg)
                leftLeg.localRotation =
                    Quaternion.Euler(0, 0, angle);

            if (rightLeg)
                rightLeg.localRotation =
                    Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            if (leftLeg)
                leftLeg.localRotation =
                    Quaternion.identity;

            if (rightLeg)
                rightLeg.localRotation =
                    Quaternion.identity;
        }
    }
}