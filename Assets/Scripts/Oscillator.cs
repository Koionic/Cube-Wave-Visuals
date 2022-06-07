
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private float distance;
    private float offset;
    private float startAngle;
    private float startHeight;

    private float rowAmount;
    private CubeWave initializer;

    public void Init(float distance, float offset, float startAngle, float startHeight, float rowAmount,
        CubeWave initializer)
    {
        this.distance = distance;
        this.offset = offset;
        this.startAngle = startAngle;
        this.startHeight = startHeight;
        this.rowAmount = rowAmount;
        this.initializer = initializer;
    }

    public void UpdateAngle(float angle)
    {
        var diff = (transform.localPosition - initializer.centerPosition);
    
        var d = new Vector3(diff.x * initializer.centerScale.x,diff.y * initializer.centerScale.y,diff.z * initializer.centerScale.z).magnitude % rowAmount / 2f;
        var offset = MathHelper.Map(d, 0, rowAmount / 2f, -Mathf.PI, Mathf.PI);
        var a = angle + offset;
        var h = MathHelper.Map(Mathf.Sin(a), -1, 1, 1, 20);
        
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, h);
    }
}
