using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MessagesBalloon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_messagesText;
    [SerializeField]
    private Animator nuggetAnimator;

    string m_currentMessage;

    void Start()
    {
        m_currentMessage = "Encaixe as engrenagens em qualquer ordem!";

        m_messagesText.text = m_currentMessage;

        GearSlot.OnGearActivesChange.AddListener(OnGearActivesChange);
    }

    /// <summary>
    /// Called by GearSlot.OnGearActivesChange to check the number of gear actives, and send the apropriate message
    /// </summary>
    /// <param name="gearActives"></param>
    private void OnGearActivesChange(int gearActives)
    {
        if (gearActives == 5)
        {
            if(m_currentMessage != "Yay, parabéns. Task concluída!") //Check if we've already sent spoke this message to avoid duplicated animation
            {
                m_currentMessage = "Yay, parabéns. Task concluída!";

                nuggetAnimator.SetTrigger("speak");

                StartCoroutine(ChangeTextWithDelay());
            }
        }
        else
        {
            if (m_currentMessage != "Encaixe as engrenagens em qualquer ordem!") //Check if we've already sent this message to avoid duplicated animation
            {
                m_currentMessage = "Encaixe as engrenagens em qualquer ordem!";

                nuggetAnimator.SetTrigger("speak");

                StartCoroutine(ChangeTextWithDelay());
            }
        }
    }

    /// <summary>
    /// Used to sync the message with the animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeTextWithDelay()
    {
        yield return new WaitForSeconds(0.2f);

        m_messagesText.text = m_currentMessage;
    }
}
