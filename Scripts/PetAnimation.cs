using UnityEngine;

public class PetAnimation : MonoBehaviour
{
    MainScript2 ms2;
    private Animator mAnimator;
    private bool deathAnimationPlayed = false; // Track if death animation has been played

    void Start()
    {
        ms2 = FindAnyObjectByType<MainScript2>();
        mAnimator = GetComponent<Animator>();

        if (ms2 == null)
        {
            Debug.Log("MainScript2 not found!");
        }
        else
        {
            Debug.Log("MainScript2 found. Pet animations ready!");
        }

        // Ensure that death animation doesn't loop
        if (mAnimator != null)
        {
            AnimatorClipInfo[] clips = mAnimator.GetCurrentAnimatorClipInfo(0);
            foreach (var clip in clips)
            {
                if (clip.clip.name == "Death")  // Assuming the death animation clip is named "Death"
                {
                    clip.clip.wrapMode = WrapMode.Once;  // Ensure it plays once and doesn't repeat
                }
            }
        }
    }

    void Update()
    {
        if (ms2 != null && mAnimator != null)
        {
            Animation();
        }
    }

    void Animation()
    {
        // Play death animation only once when health is 0
        if (ms2.getHealth() <= 0 && !deathAnimationPlayed)
        {
            mAnimator.SetTrigger("Death");
            deathAnimationPlayed = true; // Mark death animation as played
        }
        else if (!deathAnimationPlayed)  // Handle Dirtiness Animations only if pet is alive
        {
            if (ms2.getDirt() >= 75f)
            {
                mAnimator.SetTrigger("Dirty75");
            }
            else if (ms2.getDirt() >= 50f)
            {
                mAnimator.SetTrigger("Dirty50");
            }
            else if (ms2.getDirt() >= 25f)
            {
                mAnimator.SetTrigger("Dirty25");
            }
            else if (ms2.getDirt() <= 25f)
            {
                mAnimator.SetTrigger("Clean");
            }
        }
    }
}
