﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GearSlot : MonoBehaviour
{
    private static int _gearActives;
    private static int GearActives 
    {
        get
        {
            return _gearActives;
        }
        set
        {
            _gearActives = value;

            OnGearActivesChange.Invoke(value);
        }
    }

    public static IntUnityEvent OnGearActivesChange = new IntUnityEvent();

    [SerializeField]
    private SpriteRenderer gearVisual;
    [SerializeField]
    private ParticleSystem vfxSmoke;
    [SerializeField, Tooltip("Negative number rotates to left, positive rotates to right")]
    private float rotationFactor;

    private UI_InventoryGear UI_InventoryGear;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 startPosition;
    private bool rotationStatus;

    private void Start()
    {
        startPosition = gearVisual.transform.localPosition;
        m_Rigidbody2D = gearVisual.GetComponent<Rigidbody2D>();

        OnGearActivesChange.AddListener(CheckGears);
    }

    /// <summary>
    /// Activate the gear with a little animation
    /// </summary>
    /// <param name="gearColor"></param>
    /// <param name="inventoryGear"></param>
    public void ActivateGear(Color32 gearColor, UI_InventoryGear inventoryGear)
    {
        gearVisual.transform.localScale = Vector3.one * 0.8f; //Scale down the image to do an animation later

        gameObject.layer = 0;
        gearVisual.color = gearColor;
        gearVisual.enabled = true;

        vfxSmoke.Play();

        gearVisual.transform.DOScale(1, 0.4f).SetEase(Ease.OutElastic); //Animate the entrance of the image

        UI_InventoryGear = inventoryGear; //Save the reference of the gear of the inventory

        GearActives++;
    }

    /// <summary>
    /// Deactivate the world gear object
    /// </summary>
    private void DeactivateGear()
    {
        gameObject.layer = 8;
        gearVisual.enabled = false;
        UI_InventoryGear = null;

        GearActives--;
    }

    public void OnMouseDown()
    {
        if (gearVisual.enabled)
        {
            gameObject.layer = 2;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; //Disable the freeze position of the Rigidbody
            m_Rigidbody2D.gravityScale = Random.Range(2, 3.5f); //Random gravity to make more organic

            StartCoroutine(OnDeactivate());
        }
    }

    private IEnumerator OnDeactivate()
    {
        while(gearVisual.transform.position.y > -2.45f)
        {
            yield return null;
        }

        UI_InventoryGear.ShowVisuals();

        DeactivateGear();

        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Rigidbody2D.transform.localPosition = startPosition;
    }

    /// <summary>
    /// Check the variable GearActives and set rotationStatus based on it
    /// </summary>
    private void CheckGears(int gearActives)
    {
        if (gearActives == 5)
            rotationStatus = true;
        else
            rotationStatus = false;
    }

    private void Update()
    {
        if (rotationStatus)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationFactor);
        }
    }
}
