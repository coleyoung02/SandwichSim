using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

class SelectDefaultLayer : ScriptableWizard
{
    public int layerNo = 0;

    [MenuItem("Editor/Select All of Layer...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard(
            "Select All of Layer...",
            typeof(SelectDefaultLayer),
            "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject[] gos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        List<GameObject> all = new List<GameObject>();
        foreach (GameObject go in gos)
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                all.Add(t.gameObject);
            }
        }

        List<GameObject> filtered = new List<GameObject>();
        foreach (GameObject go in all)
        {
            if (go.layer == layerNo)
            {
                Debug.LogWarning("Added " + go.layer.ToString());
                filtered.Add(go);
            }
            else
            {
                Debug.LogWarning("didnt add " + go.layer.ToString());
            }
        }
        Selection.objects = filtered.ToArray();
    }
}