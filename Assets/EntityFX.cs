using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material orginalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        orginalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(.2f);

        sr.material = orginalMat;
    }
}
