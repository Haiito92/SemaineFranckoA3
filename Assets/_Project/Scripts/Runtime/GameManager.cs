using System.Collections.Generic;
using System.Linq;
using LittleDialogue.Runtime;
using LittleGraph.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            List<LGGraphObject> graphObjects = GameObject.FindObjectsByType<LGGraphObject>((FindObjectsSortMode)FindObjectsInactive.Include).ToList();

            foreach (LGGraphObject graphObject in graphObjects)
            {
                graphObject.Init();
            }

            DialogueController dialogueController = FindFirstObjectByType<DialogueController>();
            if (dialogueController != null)
            {
                dialogueController.Init(graphObjects);
            }
        }
    }
}
