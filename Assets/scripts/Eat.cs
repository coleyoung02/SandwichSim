using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Eat : MonoBehaviour
{
    private bool winEnabled = true;

    public void SetWinable(bool b)
    {
        winEnabled = b;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (winEnabled && Frobbable.hasLayer(LayerMask.GetMask("Player"), collision.gameObject.layer))
        {
            FindFirstObjectByType<FadeIn>().BeginFade();
        }
    }

}
