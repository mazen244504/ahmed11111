using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeZone : MonoBehaviour
{
    public float chargePerSecond = 50f;
    public bool isActive = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            DashSystem dash = other.GetComponent<DashSystem>();
            if (dash != null)
            {
                dash.AddCharge(chargePerSecond * Time.deltaTime);
            }
        }
    }
}
