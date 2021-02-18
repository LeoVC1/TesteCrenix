using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UI_InventoryGear : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Image placeholderImage;

    private Camera mainCamera;
    private Transform m_StartParent;
    private Image m_Image;
    private RectTransform m_ParentCanvas;
    private RectTransform m_RectTransform;
    private Vector3 startPosition;
    private GearSlot currentGearSlot;

    private void Start()
    {
        mainCamera = Camera.main;

        m_RectTransform = GetComponent<RectTransform>();
        m_StartParent = m_RectTransform.parent;
        m_Image = GetComponent<Image>();
        m_ParentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        startPosition = m_RectTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        placeholderImage.enabled = true; //Activate placeholder image, just for visual details

        m_RectTransform.SetParent(m_ParentCanvas); //Set Canvas as Parent
        m_ParentCanvas.SetAsLastSibling(); //Set as last sibling to always stay on top

        SetDragPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDragPosition(eventData);

        if (CheckWorldCollision())
        {
            m_RectTransform.DOScale(1.5f, 0.25f); //If on top of a slot, scale up just for visual feedback
        }
        else
        {
            m_RectTransform.DOScale(1, 0.25f); //If not on top of a slot, scale down just for visual feedback
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_RectTransform.SetParent(m_StartParent); //Return to original parent

        if (CheckWorldCollision()) //If end drag on top of a slot, hide the object
        {
            HideVisuals();

            if (currentGearSlot)
            {
                currentGearSlot.ActivateGear(m_Image.color, this);
            }
        }
        else //Else, return it to start location
        {
            m_RectTransform.DOLocalMove(startPosition, 0.3f, true).OnComplete(() => placeholderImage.enabled = false);
        }

    }

    /// <summary>
    /// Set the object to mouse position
    /// </summary>
    /// <param name="data"></param>
    private void SetDragPosition(PointerEventData data)
    {
        Vector3 mousePosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_ParentCanvas, data.position, data.pressEventCamera, out mousePosition))
        {
            m_RectTransform.position = mousePosition;
        }
    }

    /// <summary>
    /// Check the world collision, returns true if on top of a slot
    /// </summary>
    /// <returns></returns>
    private bool CheckWorldCollision()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D mouseHit = Physics2D.Raycast(mousePosition, Vector2.zero, float.PositiveInfinity, layerMask);

        if (mouseHit.collider != null)
        {
            if (mouseHit.collider.CompareTag("GearWorldSlot"))
            {
                currentGearSlot = mouseHit.collider.GetComponent<GearSlot>();
                return true;
            }
        }

        currentGearSlot = null;
        return false;
    }

    /// <summary>
    /// Shows the visuals of the inventory gear
    /// </summary>
    public void ShowVisuals()
    {
        m_Image.enabled = true;
    }

    /// <summary>
    /// Hide the visuals of the inventory gear, and return it to original position
    /// </summary>
    public void HideVisuals()
    {
        m_Image.enabled = false;
        placeholderImage.enabled = false;

        m_RectTransform.DOPause();
        m_RectTransform.localPosition = startPosition;
        m_RectTransform.localScale = Vector3.one;
    }

}
