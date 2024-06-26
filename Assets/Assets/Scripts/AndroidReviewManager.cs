#if ENABLE_ANDROID_REVIEW

using System.Collections;
using System.Collections.Generic;
using Google.Play.Review;
using UnityEngine;

public class AndroidReviewManager
{
    private static AndroidReviewManager instance;

    public static AndroidReviewManager Instance => instance;

    private ReviewManager reviewManager;
    private PlayReviewInfo playReviewInfo;

    public bool HasPlayReviewInfo => this.playReviewInfo != null;

    public static void CreateInstance()
    {
        instance = new AndroidReviewManager();
    }

    private AndroidReviewManager()
    {
        this.reviewManager = new ReviewManager();
    }

    public IEnumerator RequestAndLaunchReviewFlow()
    {
        Logger.Log("RequestReviewFlow: requested.");

        var requestFlowOperation = this.reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Logger.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        this.playReviewInfo = requestFlowOperation.GetResult();

        Logger.Log("RequestReviewFlow: completed.");

        Logger.Log("LaunchReviewFlow: requested.");

        var launchFlowOperation = this.reviewManager.LaunchReviewFlow(this.playReviewInfo);
        yield return launchFlowOperation;

        this.playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Logger.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }

        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
        Logger.Log("LaunchReviewFlow: completed.");
    }

    public IEnumerator RequestReviewFlow()
    {
        Logger.Log("RequestReviewFlow: requested.");

        var requestFlowOperation = this.reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Logger.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        this.playReviewInfo = requestFlowOperation.GetResult();

        Logger.Log("RequestReviewFlow: completed.");
    }

    public IEnumerator LaunchReviewFlow()
    {
        Logger.Log("LaunchReviewFlow: requested.");

        var launchFlowOperation = this.reviewManager.LaunchReviewFlow(this.playReviewInfo);
        yield return launchFlowOperation;

        this.playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Logger.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }

        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
        Logger.Log("LaunchReviewFlow: completed.");
    }
}

#endif
