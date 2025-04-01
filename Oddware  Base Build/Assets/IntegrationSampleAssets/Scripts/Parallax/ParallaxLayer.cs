using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    public ParallaxManager Manager;

    public void Move(float delta)
    {
        Vector3 newPos = transform.position;
        newPos.x += delta * Manager.XLayerParallaxScale;

        transform.position = newPos;
    }

    public void MoveLiteral(float distance)
    {
        Vector3 newPos = transform.position;
        newPos.x += distance;

        transform.position = newPos;
    }
}
