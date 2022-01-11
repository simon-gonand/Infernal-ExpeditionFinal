using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour, EnemiesAI
{
    [SerializeField]
    private Transform linkedGate;

    [SerializeField]
    private float timeToOpen;

    private Vector3 openGatePosition;
    private Vector3 initialPos;
    private bool isOpen = false;

    public void Die()
    {
        if (!isOpen)
        {
            StartCoroutine(Open());
            isOpen = true;
        }
    }

    IEnumerator Open()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (1/timeToOpen);
            linkedGate.position = Vector3.Lerp(initialPos, openGatePosition, t);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void ResetCurrentTarget()
    {
        // No implementation needed for this
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPos = linkedGate.position;
        Collider fenceCollider = linkedGate.GetComponent<Collider>();
        openGatePosition = new Vector3(initialPos.x, initialPos.y - fenceCollider.bounds.size.y, initialPos.z);
    }
}
