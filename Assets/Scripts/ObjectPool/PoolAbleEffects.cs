
public class PoolAbleEffects : PoolAble
{
    public float destroyTime = 1.5f;
    void OnEnable()
    {
        this.CallOnDelay(destroyTime, ReleaseObject);
    }
}
