using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDynamicLight
{
    public float GetDistance(Vector3 pos);
    public void Enable();
    public void Disable();
    public void Initialize();
}
