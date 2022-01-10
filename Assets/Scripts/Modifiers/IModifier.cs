using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier
{
    public float durationTime { get; }

    public void Start();
    public void End();
}
