using UnityEngine;

public class BuildCell : MonoBehaviour
{
    public bool blocked;

    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetBlocked(bool value)
    {
        blocked = value;

        if (blocked)
            sprite.color = new Color(1, 0, 0, 0.45f);
        else
            sprite.color = new Color(0, 1, 0, 0.35f);
    }

    void OnMouseDown()
    {
        if (!BuildSystem.Instance.building)
            return;

        if (blocked)
            return;

        BuildSystem.Instance.PlaceTurret(transform.position);

        blocked = true;
        SetBlocked(true);
    }
}