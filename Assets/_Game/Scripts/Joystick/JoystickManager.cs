using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image imageJoystickBackground;
    [SerializeField] private Image imageJoystickHandler;
    private Vector2 posInput;

    

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageJoystickBackground.rectTransform, 
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
        {
            posInput.x = posInput.x / (imageJoystickBackground.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imageJoystickBackground.rectTransform.sizeDelta.y);

            // normalize
            if (posInput.magnitude > 1.0f) posInput = posInput.normalized;

            // joystick move
            imageJoystickHandler.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (imageJoystickHandler.rectTransform.sizeDelta.x / 2),
                posInput.y * (imageJoystickHandler.rectTransform.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //isResetJoystick = false;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imageJoystickHandler.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float InputHorizontal()
    {
        if (posInput.x != 0)
        {
            return posInput.x;
        }
        else return Input.GetAxis("Horizontal");
    }

    public float InputVertical()
    {
        if (posInput.y != 0)
        {
            return posInput.y;
        }
        else return Input.GetAxis("Vertical");
    }

    
}
