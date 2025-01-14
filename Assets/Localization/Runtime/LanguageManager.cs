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

        void Start()
        {
            Debug.Log(FindObjectsByType<MaskableGraphic>(FindObjectsInactive.Include, FindObjectsSortMode.None).Count());
            GetChildrenRecursively(SceneManager.GetActiveScene().GetRootGameObjects());



            /*foreach (var gameobject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                Debug.Log("root gameobject");
                if(gameobject.TryGetComponent(out TMP_Text tmpTextComponent))
                {
                    Debug.Log("root TryGet TMP_TEXT");
                    TextList.Add(new LocalizationData(tmpTextComponent.text));
                }

                if (gameobject.TryGetComponent(out Text textComponent))
                {
                    Debug.Log("root TryGet TEXT");
                    TextList.Add(new LocalizationData(textComponent.text));
                }

                foreach (var tmpText in gameObject.GetComponentsInChildren<TMP_Text>())
                {
                    Debug.Log("Child get TMP_TEXT");
                    TextList.Add(new LocalizationData(tmpText.text));
                }

                foreach (var text in gameObject.GetComponentsInChildren<Text>())
                {
                    Debug.Log("Child get TEXT");
                    TextList.Add(new LocalizationData(text.text));
                }
            }*/

            Debug.Log(TextList.Count);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GetChildrenRecursively(GameObject[] parentsGameobjects)
        {
            foreach(GameObject currentGameobject in parentsGameobjects) 
            {
                Debug.Log($"{currentGameobject.name}");
                if (currentGameobject.transform.childCount <= 0) continue;

                GameObject[] childrenArray = new GameObject[currentGameobject.transform.childCount];

                for(int i = 0; i < currentGameobject.transform.childCount; i++)
                {
                    childrenArray[i] = currentGameobject.transform.GetChild(i).gameObject;
                }

                GetChildrenRecursively(childrenArray);

                //AddTextToList(currentGameobject);
            }
        }

        private void AddTextToList()
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

        [Button("Refresh")]
        public bool azer;

        public void Refresh()
        {
            AddTextToList();
        }
    }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string methodName = (attribute as ButtonAttribute).MethodName;
            UnityEngine.Object target = property.serializedObject.targetObject;
            System.Type type = target.GetType();
            System.Reflection.MethodInfo method = type.GetMethod(methodName);

            Debug.Log(method);
            if (method == null)
            {
                GUI.Label(position, "Method could not be found. Is it public?");
                return;
            }
            if (method.GetParameters().Length > 0)
            {
                GUI.Label(position, "Method cannot have parameters.");
                return;
            }
            if (GUI.Button(position, method.Name))
            {
                method.Invoke(target, null);
            }
        }
    }
#endif

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string MethodName { get; }
        public ButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }

}
