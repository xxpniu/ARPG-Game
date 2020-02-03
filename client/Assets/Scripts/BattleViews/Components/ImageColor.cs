using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ColorRun(0,1));
    }

    public void Hide()
    {
        if (this.gameObject.activeSelf)
        {
            StopAllCoroutines();
            StartCoroutine(ColorRun(1, 0));
        }
    }
        
    private IEnumerator ColorRun(float f,float t)
    {
        var start = Time.time;
        var image = this.GetComponent<Image>();
        image.color = new Color(image.color.r,image.color.g,image.color.g, f);
        yield return null;
        while (Time.time - start < 0.3f)
        {
            var a = Mathf.Lerp(f, t, (Time.time - start) / 0.3f);
            image.color = new Color(image.color.r, image.color.g, image.color.g, a);
            yield return null;
        }
      
        image.color = new Color(image.color.r, image.color.g, image.color.g, t);

        if (t == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
