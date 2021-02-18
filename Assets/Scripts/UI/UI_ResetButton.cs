using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_ResetButton : MonoBehaviour
{
    [SerializeField]
    private GearSlot[] worldGearSlots;

    private Button m_ResetButton;

    private void Start()
    {
        m_ResetButton = GetComponent<Button>();
        m_ResetButton.onClick.AddListener(ResetGearSlots);
    }

    /// <summary>
    /// Reset all gear slots in worldGearSlots[]
    /// </summary>
    private void ResetGearSlots()
    {
        foreach (GearSlot gearSlot in worldGearSlots)
        {
            gearSlot.OnMouseDown();
        }
    }
}
