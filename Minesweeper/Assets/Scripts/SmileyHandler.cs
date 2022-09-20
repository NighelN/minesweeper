using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SmileyHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    void Start()
    {
    }

    private void HandleLeftClick()
    {
        Grid.instance.Reset();
    }

    private void HandleRightClick()
    {
        Grid.instance.ResetField();
        MenuHandler.instance.game.SetActive(false);
        MenuHandler.instance.lobby.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Middle) HandleLeftClick();
        else if (eventData.button == PointerEventData.InputButton.Right) HandleRightClick();
    }

    public void OnPointerEnter(PointerEventData eventData) { }
    public void OnPointerExit(PointerEventData eventData) { }
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }
    public void OnSelect(BaseEventData eventData) { }
}
