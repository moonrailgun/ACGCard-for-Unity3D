using UnityEngine;
using System.Collections;

public class ArrowLine : MonoBehaviour
{
    private float speed;
    private UITexture texture;
    private GameObject cursor;
    public Vector2 BeginPos;
    public bool isShowing;

    private void Awake()
    {
        speed = 1;
        texture = GetComponent<UITexture>();
        texture.pivot = UIWidget.Pivot.Bottom;
        cursor = GameObject.FindGameObjectWithTag(Tags.Cursor);
    }

    private void Update()
    {
        if (isShowing)
        {
            if (cursor != null)
            {
                Vector2 cursorPos = new Vector2(cursor.transform.localPosition.x, cursor.transform.localPosition.y);

                SetUVRect();
                SetWidge(BeginPos, cursorPos);
            }
            else
            {
                HideArrowLine();
            }
        }

    }

    /// <summary>
    /// 设置UV贴图
    /// </summary>
    private void SetUVRect()
    {
        Rect rect = texture.uvRect;
        rect.y -= Time.deltaTime * speed;
        texture.uvRect = rect;
    }

    /// <summary>
    /// 设置UI容器大小和方向
    /// </summary>
    private void SetWidge(Vector2 from, Vector2 to)
    {

        Vector2 dir = to - from;
        float dis = dir.magnitude;
        int coefficient = dir.x > 0 ? -1 : 1;
        float angle = Vector2.Angle(Vector2.up, to - from) * coefficient;

        transform.position = new Vector3(from.x, from.y);
        transform.eulerAngles = new Vector3(0, 0, angle);
        texture.height = Mathf.FloorToInt(dis);
    }

    /// <summary>
    /// 设置箭头指向线的来源位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetBeginPos(Vector2 pos)
    {
        this.BeginPos = pos;
    }
    public void SetBeginPos(GameObject go)
    {
        this.BeginPos = new Vector2(go.transform.position.x, go.transform.position.y);
    }

    /// <summary>
    /// 显示线条
    /// </summary>
    public void ShowArrowLine()
    {
        isShowing = true;
        gameObject.SetActive(true);
    }
    public void ShowArrowLine(Vector2 from)
    {
        ShowArrowLine();
        SetBeginPos(from);
    }

    /// <summary>
    /// 隐藏线条
    /// </summary>
    public void HideArrowLine()
    {
        isShowing = false;
        gameObject.SetActive(false);
    }
}
