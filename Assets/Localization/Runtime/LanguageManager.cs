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

namespace Localization.Runtime
{
    public class LanguageManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] LANGUAGES_STATE LANGUAGE;
        [SerializeField] List<LocalizationData> TextList;


        public void AddTextToList()
        {
            MaskableGraphic[] maskableGraphics = FindObjectsByType<MaskableGraphic>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (MaskableGraphic mg in maskableGraphics)
            {
                switch (mg)
                {
                    case TMP_Text tmpText: TextList.Add(new LocalizationData(tmpText.text)); break;
                    case Text text: TextList.Add(new LocalizationData(text.text)); break;
                }
            }

            //if(gameObject.TryGetComponent(out TMP_Text tmpTextComponent)) TextList.Add(new LocalizationData(tmpTextComponent.text));
            //if(gameObject.TryGetComponent(out Text textComponent)) TextList.Add(new LocalizationData(textComponent.text));
        }


        /*private void GetChildrenRecursively(GameObject[] parentsGameobjects)
        {
            foreach (GameObject currentGameobject in parentsGameobjects)
            {
                Debug.Log($"{currentGameobject.name}");
                if (currentGameobject.transform.childCount <= 0) continue;

                GameObject[] childrenArray = new GameObject[currentGameobject.transform.childCount];

                for (int i = 0; i < currentGameobject.transform.childCount; i++)
                {
                    childrenArray[i] = currentGameobject.transform.GetChild(i).gameObject;
                }

                GetChildrenRecursively(childrenArray);

                //AddTextToList(currentGameobject);
            }
        }*/
    }
}
