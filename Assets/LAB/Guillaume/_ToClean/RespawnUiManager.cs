using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUiManager : MonoBehaviour
{
    public static RespawnUiManager instance;

    [Header("Picto of player")]
    public GameObject[] playerPictoArray;
    [Space]
    public Transform verticalGroup;

    private void Awake()
    {
        #region Setup instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is multiple RespwnUiManager");
        }
        #endregion
    }

    public void SpawnPicto(int playerIndex)
    {
        GameObject actualPicto = Instantiate(playerPictoArray[playerIndex], verticalGroup.position, verticalGroup.rotation);
        actualPicto.transform.SetParent(verticalGroup);
    }
}
