using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridChild
{
    void HandleLeftClick(Transform transform, int x, int y);
    void HandleRightClick(Transform transform);
}
