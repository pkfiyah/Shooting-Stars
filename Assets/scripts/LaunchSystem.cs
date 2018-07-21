using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSystem : MonoBehaviour {
    // Use this for initialization
    private SpriteRenderer bar_Renderer;
    private SpriteRenderer box_Renderer;
    private Transform bar_Transform;
	void Start () {
        box_Renderer = gameObject.transform.Find("Box").GetComponent<SpriteRenderer>();
        bar_Transform = gameObject.transform.Find("Inner");
        bar_Renderer = bar_Transform.GetComponent<SpriteRenderer>();
        box_Renderer.enabled = false;
        bar_Renderer.enabled = false;
    }

    public void displayCharge(float value, float max)
    {
        if (!box_Renderer.enabled) box_Renderer.enabled = true;
        if (!bar_Renderer.enabled) bar_Renderer.enabled = true;
        bar_Transform.localScale = new Vector3(value / max, bar_Transform.localScale.y, 0.0f);
    }

    public void endCharge ()
    {
        box_Renderer.enabled = false;
        bar_Renderer.enabled = false;
    }
}
