using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IModifier : MonoBehaviour
{
    protected float durationTime;

    public void StartModifier(float durationTime)
    {
        if (LevelManager.instance.levelModifiers)
        {
            this.durationTime = durationTime;
            StartCoroutine(ApplyModifierCoroutine());
        }
    }

    private IEnumerator ApplyModifierCoroutine()
    {
        StartBehaviour();
        yield return new WaitForSeconds(durationTime);
        EndBehaviour();
    }

    protected abstract void StartBehaviour();

    protected abstract void EndBehaviour();
}
