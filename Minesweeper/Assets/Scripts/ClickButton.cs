using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    // X, Y zijn  coördinaten van blokjes/cells
    int x, y;
    GridChild child;

    void Start()
    {
        //substring haalt iets er van af (dus het haalt de eerste 10 letters weg)
        string[] outcome = name.Substring(10, name.Length - 10).Split(':');
        x = int.Parse(outcome[0]);
        y = int.Parse(outcome[1]);
        child = Grid.instance.grid[x, y];
    }

    private void HandleLeftClick()
    {
        //string format veranderd de placeholders ( placeholders = {0} bijv. ) naar de variabelen die zijn aangegeven
        Debug.Log(String.Format("Handle left click for {0}, {1}", x, y));
        child.HandleLeftClick(transform, x, y);
    }

    private void HandleRightClick()
    {
        //string format veranderd de placeholders ( placeholders = {0} bijv. ) naar de variabelen die zijn aangegeven
        Debug.Log(String.Format("Handle right click for {0}, {1}", x, y));
        child.HandleRightClick(transform);
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
