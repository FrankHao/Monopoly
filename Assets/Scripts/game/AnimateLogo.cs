using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLogo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartChoreography();
    }
    void StartChoreography()
    {
        TweenUtil.TweenSize(transform, 50f, 1.75f);
        TweenUtil.TweenAlpha(transform, 88, 1.75f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
