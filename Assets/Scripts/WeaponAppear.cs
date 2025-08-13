using UnityEngine;

public class WeaponAppear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    string triggerName = "Swing";


    public static WeaponAppear instance;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Appear()
    {
       if(Input.GetButton("Fire1"))
        {
            animator.SetTrigger(triggerName);
        }
    }
}



    

