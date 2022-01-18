using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogLevel7Behaviour : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private float timeOffset;
    
    public void RemoveFog()
    {
        StartCoroutine(RemoveFogCoroutine());
    } 

    private IEnumerator RemoveFogCoroutine()
    {
        float fogValue = rend.sharedMaterial.GetFloat("Vector1_6F1EA0F8");
        while (fogValue > 0.0f)
        {
            fogValue -= 0.5f;
            rend.material.SetFloat("Vector1_6F1EA0F8", fogValue);
            yield return new WaitForSeconds(timeOffset);
        }
        yield return null;
    }
}
