using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Codice.Client.BaseCommands;
using System;
using System.Xml;
using System.Linq;
using UnityEditor;
using System.IO;
using Mono.Cecil;

namespace Localization.Runtime
{
    public class LanguageManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private LANGUAGES_STATE LANGUAGE;
        public TextAsset _transalationAsset;
        public List<LocalizationData> TextList;
        [SerializeField] CSV_TextTable translationTable = new CSV_TextTable();

        public LANGUAGES_STATE LANGUAGEProperty { get { return LANGUAGE; } set { LANGUAGE = value; OnLANGUAGEValueChange?.Invoke(); } }

        private Action OnLANGUAGEValueChange;

        private void Awake()
        {
            translationTable.Load(_transalationAsset);
        }

        private void OnEnable()
        {
            OnLANGUAGEValueChange += TranslateAllTexts;
        }

        private void OnDisable()
        {
            OnLANGUAGEValueChange -= TranslateAllTexts;
        }

        private void Start()
        {
            TranslateAllTexts();
        }

        public void AddTextToList()
        {
            MaskableGraphic[] maskableGraphics = FindObjectsByType<MaskableGraphic>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            TextList.Clear();

            foreach (MaskableGraphic mg in maskableGraphics)
            {
                switch (mg)
                {
                    case TMP_Text tmpText: 
                        if(!CheckForDuplicateInList(tmpText))
                            TextList.Add(new LocalizationData(tmpText)); 
                        break;
                    case Text text: 
                        if(!CheckForDuplicateInList(text))
                            TextList.Add(new LocalizationData(text)); 
                        break;
                }
            }
        }

        private void TranslateAllTexts()
        {
            foreach (LocalizationData data in TextList)
            {
                for (int i = 0; i < translationTable.NumRows(); i++)
                {
                    if (data.Key == translationTable.GetRowList()[i].Key)
                    {
                        switch (data.TextComponent)
                        {
                            case TMP_Text tmpText:
                                switch (LANGUAGE)
                                {
                                    case LANGUAGES_STATE.ENGLISH:
                                        tmpText.text = translationTable.GetRowList()[i].en;
                                        break;
                                    case LANGUAGES_STATE.FRENCH:
                                        tmpText.text = translationTable.GetRowList()[i].fr;
                                        break;
                                }
                                break;
                            case Text text:
                                switch (LANGUAGE)
                                {
                                    case LANGUAGES_STATE.ENGLISH:
                                        text.text = translationTable.GetRowList()[i].en;
                                        break;

                                    case LANGUAGES_STATE.FRENCH:
                                        text.text = translationTable.GetRowList()[i].fr;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private bool CheckForDuplicateInList(Component component)
        {
            foreach(LocalizationData data in TextList)
            {
                if(data.TextComponent == component)
                {
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        public void ClearTextList()
        {
            TextList.Clear();
        }
#endif
    }
}
