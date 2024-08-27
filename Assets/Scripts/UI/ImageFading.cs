using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFading : MonoBehaviour
{
    public Image[] images;

    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Image current in images)
        {
            current.color = new Color(current.color.r, current.color.g, current.color.g, current.color.a - (Time.deltaTime * speed));
        }
    }

    public void SetAlphasToOne()
    {
        foreach(Image current in images)
        {
            current.color = new Color(current.color.r, current.color.g, current.color.g, 1.0f);
        }
    }
}
