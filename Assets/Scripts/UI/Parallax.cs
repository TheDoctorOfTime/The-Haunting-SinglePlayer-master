using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Vector2 minXY;
    [SerializeField] Vector2 maxXY;

    [SerializeField] bool isCamSpace = false;
    [SerializeField] RectTransform backGroundRect;
    // Update is called once per frame
    void Update()
    {
        if (isCamSpace)
        {
            Vector3 distance = new Vector3(Map(Input.mousePosition.x, 0.0f, Screen.width, minXY.x, maxXY.x), Map(Input.mousePosition.y, 0.0f, Screen.height, minXY.y, maxXY.y), backGroundRect.position.z);
            backGroundRect.position = distance;
        }
        else
        {
            Vector3 distance = new Vector3(Map(Input.mousePosition.x, 0.0f, Screen.width, minXY.x, maxXY.x) + Screen.width / 2, Map(Input.mousePosition.y, 0.0f, Screen.height, minXY.y, maxXY.y) + Screen.height / 2, 0);
            backGroundRect.position = distance;
        }
    }

    float Map(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
		if (OldMin != OldMax && NewMin != NewMax)
		    return (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin;
		else
		    return (NewMax + NewMin) / 2;
	}
}
