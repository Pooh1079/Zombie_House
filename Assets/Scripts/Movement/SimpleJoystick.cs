using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;

    public float handleLimit = 50f;

    private Vector2 input;


    public Vector2 Direction
    {
        get { return input; }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        MoveHandle(eventData);
    }



    public void OnDrag(PointerEventData eventData)
    {
        MoveHandle(eventData);
    }



    void MoveHandle(PointerEventData eventData)
    {
        Vector2 position;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );


        input = position / (background.sizeDelta / 2f);


        input = Vector2.ClampMagnitude(input, 1);



        handle.anchoredPosition =
            input * handleLimit;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;

        handle.anchoredPosition = Vector2.zero;
    }
}