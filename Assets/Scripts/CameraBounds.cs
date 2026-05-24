using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private Transform tl, br;
    public Vector2 TopLeft { get => tl.position; }
    public Vector2 BottomRight { get => br.position; }
    public float Top { get => tl.position.y; }
    public float Bottom { get => br.position.y; }
    public float Left { get => tl.position.x; }
    public float Right { get => br.position.x; }

    void Start()
    {
        tl = GameObject.FindGameObjectWithTag("TopLeft").GetComponent<Transform>();
        br = GameObject.FindGameObjectWithTag("BottomRight").GetComponent<Transform>();
    }
}
