using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFading : MonoBehaviour
{
    [Tooltip("the images that will fasde over time")]
    public Image[] images;

    [SerializeField, Tooltip("how fast the images will fade")]
    private float speed;

    /// <summary>
    /// will minus the alpha of each image
    /// </summary>
    void Update()
    {
        foreach(Image current in images)
        {
            current.color = new Color(current.color.r, current.color.g, current.color.g, current.color.a - (Time.deltaTime * speed));
        }
    }

    /// <summary>
    /// sets the alpha of each image to one
    /// </summary>
    public void SetAlphasToOne()
    {
        foreach(Image current in images)
        {
            current.color = new Color(current.color.r, current.color.g, current.color.g, 1.0f);
        }
    }
}
