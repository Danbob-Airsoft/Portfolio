using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGateBase : MonoBehaviour
{
    public bool GateTaken;

    public IEnumerator TakenDelay()
    {
        yield return new WaitForSeconds(10f);
        GateTaken = false;
    }
}
