using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    // Start is called before the first frame update

    private bool down;
    private int bVal;
    private List<MeshRenderer> m;
    private Color32[] originalColors;
    private Material[] originalMaterials;
    private Material highlightMat;
    private Coroutine flashing;


    private IEnumerator Flash()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (down)
            {
                bVal -= 7;
                if (bVal <= 11)
                {
                    down = false;
                    bVal = 1;
                }
            }
            else
            {
                bVal += 7;
                if (bVal >= 255)
                {
                    down = true;
                    bVal = 255;
                }
            }
            UpdateColors();
        }
    }

    private void UpdateColors()
    {
        for (int i = 0; i < m.Count; i++)
        {
            m[i].material.color = new Color32(255, 255, (byte)bVal, 255);
        }
    }

    public void StartColors()
    {
        highlightMat = Resources.Load("HIGHLIGHT_MAT", typeof(Material)) as Material;
        down = false;
        m = descendentSearch(gameObject);
        originalColors = new Color32[m.Count];
        originalMaterials = new Material[m.Count];
        for (int i = 0; i < m.Count; i++)
        {
            originalColors[i] = m[i].material.color;
            originalMaterials[i] = m[i].material;
        }
        bVal = 10;
        for (int i = 0; i < m.Count; i++)
        {
            m[i].material = highlightMat;
        }
        UpdateColors();
        flashing = StartCoroutine(Flash());
    }

    public void ResetColors()
    {
        StopAllCoroutines();
        for (int i = 0; i < m.Count; i++)
        {
            m[i].material = originalMaterials[i];
            m[i].material.color = originalColors[i];
        }
    }

    private List<MeshRenderer> descendentSearch(GameObject g)
    {
        if (g.transform.childCount == 0)
        {
            if (g.GetComponent<MeshRenderer>() != null)
            {
                return new List<MeshRenderer>() { g.GetComponent<MeshRenderer>() };
            }
            else
            {
                return new List<MeshRenderer>();
            }
        }
        else
        {
            List<MeshRenderer> mList = new List<MeshRenderer>();
            foreach (Transform child in g.transform)
            {
                mList.AddRange(descendentSearch(child.gameObject));
            }
            return mList;
        }
    }

}
