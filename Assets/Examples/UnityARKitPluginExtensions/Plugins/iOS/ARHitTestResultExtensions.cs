using UnityEngine.XR.iOS;

public static class ARHitTestResultExtensions
{
    public static UnityARUserAnchorData ToUnityArUserAnchorData(this ARHitTestResult arHitTestResult)
    {
        var transform = UnityARMatrixOps.GetMatrix(arHitTestResult.worldTransform);
        var anchorData = new UnityARUserAnchorData
        {
            transform = transform
        };
        return anchorData;
    }
}
