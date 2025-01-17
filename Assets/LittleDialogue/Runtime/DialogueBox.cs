using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LittleDialogue.Runtime
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private GameObject m_dialogueBoxPanel;
        [SerializeField] private TextMeshProUGUI m_dialogueText;
        [SerializeField] private List<Button> m_choiceButtons;

        // public GameObject DialogueBoxPanel => m_dialogueBoxPanel;
        // public TextMeshProUGUI DialogueText => m_dialogueText;
        // public List<Button> ChoiceButtons => m_choiceButtons;

        public void ShowBox()
        {
            m_dialogueBoxPanel.gameObject.SetActive(true);
        }

        public void UpdateText(string newText)
        {
            m_dialogueText.text = newText;
        }
    }
}
