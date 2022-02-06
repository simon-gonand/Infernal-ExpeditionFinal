using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour, EnemiesAI
{
    [SerializeField]
    private GateCollision linkedGateScript;
    [SerializeField]
    private Outline selfOutline;
    [SerializeField]
    private LineRenderer selfLineRenderer;

    [SerializeField]
    private float timeToOpen;

    [SerializeField]
    private Animator switchAnimator;

    private Vector3 openGatePosition;
    private Vector3 initialPos;
    private bool _isOpen = false;
    public bool isOpen { get { return _isOpen; } }

    private int numOfPlayerClose;

    public void Die(PlayerController player)
    {
        if (!_isOpen)
        {
            selfLineRenderer.enabled = false;
            selfOutline.enabled = false;

            AudioManager.AMInstance.ropeCutSFX.Post(gameObject);
            switchAnimator.SetTrigger("Fall");

            StartCoroutine(Open());
            _isOpen = true;
        }
    }

    IEnumerator Open()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (1/timeToOpen);
            linkedGateScript.gameObject.transform.position = Vector3.Lerp(initialPos, openGatePosition, t);
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
        
        selfOutline.enabled = false;
        linkedGateScript.line = selfLineRenderer;

        initialPos = linkedGateScript.transform.position;
        Collider fenceCollider = linkedGateScript.gameObject.GetComponent<Collider>();
        openGatePosition = new Vector3(initialPos.x, initialPos.y - fenceCollider.bounds.size.y, initialPos.z);

        selfLineRenderer.SetPosition(0, selfLineRenderer.transform.position);
        selfLineRenderer.SetPosition(1, linkedGateScript.ropeLinkTransform.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!_isOpen)
        {
            if (other.gameObject.tag == "Player")
            {
                numOfPlayerClose += 1;

                if (numOfPlayerClose > 0)
                {
                    selfOutline.enabled = true;

                    if (numOfPlayerClose > 4)
                    {
                        numOfPlayerClose = 4;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isOpen)
        {
            if (other.gameObject.tag == "Player")
            {
                numOfPlayerClose -= 1;

                if (numOfPlayerClose <= 0)
                {
                    numOfPlayerClose = 0;
                    selfOutline.enabled = false;
                }
            }
        }
    }
}
