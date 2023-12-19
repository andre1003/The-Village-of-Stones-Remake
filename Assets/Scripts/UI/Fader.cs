using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    // Fading CanvasGroup
    public CanvasGroup fadingCanvasGroup;
    private bool hasFaded = false;
    private bool hasFullFaded = false;
    private bool hasHalfFullFaded = false;

    /// <summary>
    /// Perform a fade in on canvas group. The fade in updates alpha from 0 to 1.
    /// </summary>
    /// <param name="duration">Fade in duration.</param>
    public void FadeIn(float duration = 1f)
    {
        DoFade(1f, duration);
    }

    /// <summary>
    /// Perform a fade out on canvas group. The fade out updates alpha from 1 to 0.
    /// </summary>
    /// <param name="duration">Fade out duration.</param>
    public void FadeOut(float duration = 1f)
    {
        DoFade(0f, duration);
    }

    /// <summary>
    /// Perform a full fade on canvas group. This means that:
    /// <para>1. Update alpha from 0 to 1 on a given duration</para>
    /// <para>2. Wait a given delay</para>
    /// <para>3. Update alpha from 0 to 1 on a given duration</para>
    /// </summary>
    /// <param name="duration">Fade duration.</param>
    /// <param name="delay">Black screen duration.</param>
    public void FullFade(float duration = 1f, float delay = 0.5f)
    {
        hasFullFaded = false;
        hasHalfFullFaded = false;
        DoFade(1f, duration);
        StartCoroutine(FullFadeDelay(duration, delay));
    }

    // Full fade delay
    IEnumerator FullFadeDelay(float duration, float delay)
    {
        yield return new WaitForSeconds(duration);
        hasHalfFullFaded = true;
        yield return new WaitForSeconds(delay);
        DoFade(0f, duration);
        yield return new WaitForSeconds(duration);
        hasFullFaded = true;
    }

    // Do a fade
    private void DoFade(float value, float duration)
    {
        hasFaded = false;
        var something = fadingCanvasGroup.DOFade(value, duration);
        something.onComplete += delegate { FinishFade(); };
    }

    // Flag that the fade has finished
    private void FinishFade()
    {
        hasFaded = true;
    }

    #region Getters
    // Get hasFaded value
    public bool GetHasFaded()
    {
        return hasFaded;
    }

    // Get hasFullFaded value
    public bool GetHasFullFaded()
    {
        return hasFullFaded;
    }

    // Get hasHalfFullFaded value
    public bool GetHasHalfFullFaded()
    {
        return hasHalfFullFaded;
    }
    #endregion
}

#region Fade Yield Instructions
/// <summary>
/// Suspends the the coroutine execution until a fade, in or out, is not finished.
/// </summary>
public class WaitForFade : CustomYieldInstruction 
{
    public Fader fader;

    public WaitForFade(Fader fader)
    {
        this.fader = fader;
    }

    public override bool keepWaiting
    {
        get
        {
            return !fader.GetHasFaded();
        }
    }
}

/// <summary>
/// Suspends the the coroutine execution until a full fade is not finished.
/// </summary>
public class WaitForFullFade : CustomYieldInstruction
{
    public Fader fader;

    public WaitForFullFade(Fader fader)
    {
        this.fader = fader;
    }

    public override bool keepWaiting
    {
        get
        {
            return !fader.GetHasFullFaded();
        }
    }
}

/// <summary>
/// Suspends the the coroutine execution until a half of full fade is not finished.
/// </summary>
public class WaitForHalfFade : CustomYieldInstruction
{
    public Fader fader;

    public WaitForHalfFade(Fader fader)
    {
        this.fader = fader;
    }

    public override bool keepWaiting
    {
        get
        {
            return !fader.GetHasHalfFullFaded();
        }
    }
}
#endregion
