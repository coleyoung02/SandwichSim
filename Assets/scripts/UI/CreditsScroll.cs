using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float scrollRate;
    private float timePassed;
    private float showY;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject mainUI;
    private void OnEnable()
    {
        timePassed = -1f;
    }

    void Update()
    {
        showY = this.gameObject.transform.position.y;
        float mult = 1f;
        if (showY >= 48)
        {
            mult = 2f;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            float y = Mathf.Max(minY, this.gameObject.transform.position.y - .5f * mult);
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, y);
            timePassed = -1f;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            float y = Mathf.Min(maxY, this.gameObject.transform.position.y + .5f * mult);
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, y);
            timePassed = -1f;
        }
        else if (timePassed > 1f)
        {
            float y = Mathf.Min(maxY, this.gameObject.transform.position.y + Time.deltaTime * scrollRate * mult);
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, y);
        }
        timePassed += Time.deltaTime;
        if (showY > maxY - .02f && timePassed > 0f)
        {
            mainUI.SetActive(true);
            transform.position = new Vector2(this.gameObject.transform.position.x, minY);
            creditsUI.SetActive(false);
        }
    }
}