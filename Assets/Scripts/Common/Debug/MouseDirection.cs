using UnityEngine;

/// <summary>
/// 每帧获取鼠标指向的碰撞体并打印
/// </summary>
public class MouseDirection : MonoBehaviour
{
    Camera uicamera;
    void Awake()
    {
        uicamera = GameObject.FindGameObjectWithTag(Tags.UICamera).GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 Mps = Input.mousePosition;
        Ray Mray = uicamera.ScreenPointToRay(Mps);
        RaycastHit Mhit;
        if (Physics.Raycast(Mray, out Mhit))
        {
            Debug.Log(Mhit.collider.gameObject);
        }
    }
}
