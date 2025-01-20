using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LittleDialogue.Runtime
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private GameObject m_dialogueBoxPanel;
        [SerializeField] private TextMeshProUGUI m_dialogueText;
        [SerializeField] private GameObject m_choiceButtonsParent;
        [SerializeField] private List<Button> m_choiceButtons;

        // public GameObject DialogueBoxPanel => m_dialogueBoxPanel;
        // public TextMeshProUGUI DialogueText => m_dialogueText;
        // public List<Button> ChoiceButtons => m_choiceButtons;

        private void OnEnable()
        {
            
        }

        public void ShowBox()
        {
            m_dialogueBoxPanel.gameObject.SetActive(true);
        }

        public void UpdateText(string newText)
        {
            m_dialogueText.text = newText;
        }

        public void UpdateChoiceButton(int index, string buttonText = "Null", UnityAction callback = null)
        {
            if(index > m_choiceButtons.Count - 1) return;
            Button button = m_choiceButtons[index];
            button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            button.onClick.AddListener(callback);
        }

        // public void UpdateChoiceButtonTexts(params string[] options)
        // {
        //     for (int i = 0; i < options.Length; i++)
        //     {
        //         if(i>m_choiceButtons.Count-1) break;
        //         m_choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        //     }
        // }
    }
}
