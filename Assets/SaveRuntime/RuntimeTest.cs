using System;
using NUnit.Framework.Internal;
using SaveRuntime.Runtime;
using TMPro;
using UnityEngine;

    public class RuntimeTest : MonoBehaviour
    {
        [SerializeField] private GameObject compToSave;

        [SerializeField] private TMP_Text text;

        [SerializeField] private TMP_InputField input;
        
        public void LaunchSave()
        {
            SaveFile.Save(0);
        }
        public void LaunchLoad()
        {
            SaveFile.Load(0);
            Test1 value = compToSave.GetComponent<Test1>();
            text.text = value.string1;

        }

        public void ChangeText()
        {
            text.text = input.text;
            Test1 value = compToSave.GetComponent<Test1>();
            value.string1 = text.text;
        }
        
    }

