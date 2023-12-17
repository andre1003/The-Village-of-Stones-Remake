using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper
{
    // Get animation clip name by index
    public static string GetAnimationClipNameByIndex(Animation animation, int index)
    {
        return GetAnimationClipByIndex(animation, index).name;
    }

    // Get animation clip length by index
    public static float GetAnimationClipLengthByIndex(Animation animation, int index)
    {
        return GetAnimationClipByIndex(animation, index).length;
    }

    // Get animation clip by index
    public static AnimationClip GetAnimationClipByIndex(Animation animation, int index)
    {
        // If index is negative, return the default clip name
        if(index < 0)
        {
            return null;
        }

        // Loop all animation clips and return it if the index matches
        int auxIndex = 0;
        foreach(AnimationState animationState in animation)
        {
            if(auxIndex == index)
            {
                return animationState.clip;
            }
            auxIndex++;
        }

        // If index is not found, return the default clip name
        return null;
    }
}
