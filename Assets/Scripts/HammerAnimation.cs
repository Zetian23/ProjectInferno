using UnityEngine;

public class HammerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator hammerAnimator;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
            hammerAnimator.SetTrigger("hammerSwing");
    }
}
