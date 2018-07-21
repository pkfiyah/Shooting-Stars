using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour {
    private Transform m_Transform;
    private float leftConstraint = 0.0f;
    private float rightConstraint = 0.0f;
    private float topConstraint = 0.0f;
    private float bottomConstraint = 0.0f;
    private float buffer = 0.1f;
    // Use this for initialization
    private void Awake()
    {

        // Setting up references.
        m_Transform = GetComponent<Transform>();
        leftConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.position.z)).x;
        rightConstraint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, Camera.main.transform.position.z)).x;
        topConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, Camera.main.transform.position.z)).y;
        bottomConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.position.z)).y;
    }

    // Update is called once per frame
    void Update () {
		if (m_Transform.position.x < leftConstraint - buffer)
        {
            m_Transform.position = new Vector2 (rightConstraint + buffer, m_Transform.position.y);
        }

        if (m_Transform.position.x > rightConstraint + buffer)
        {
            m_Transform.position = new Vector2(leftConstraint - buffer, m_Transform.position.y);
        }

        if (m_Transform.position.y < bottomConstraint - buffer) {
            m_Transform.position = new Vector2(m_Transform.position.x, topConstraint + buffer);
        }

        if (m_Transform.position.y > topConstraint + buffer)
        {
            m_Transform.position = new Vector2(m_Transform.position.x, bottomConstraint - buffer);
        }
	}
}
