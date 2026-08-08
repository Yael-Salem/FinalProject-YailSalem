using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private List<Gate> gates = new List<Gate>();
    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        foreach (Gate gate in gates)
        {
            if(gate != null)
                StartCoroutine(CloseAfterDelay(gate));
        }
    }

    private IEnumerator CloseAfterDelay(Gate gate)
    {
        yield return new WaitForSeconds(gate.GetDelaySeconds());
        gate.Close();
    }
}
