using UnityEngine;
using System.Collections;

public class ArrowLine : MonoBehaviour
{
    private float speed;
    private UITexture texture;
    private GameObject cursor;
    public Vector2 BeginPos;
    public bool isShowing;
    public Camera uicamera;
    public int minDis;//线条最小长度（px）

    //创建指向线
    public static ArrowLine CreateArrowLine(string parentPath = "UI Root")
    {
        GameObject parent = GameObject.Find(parentPath);
        if (parent != null)
        {
            //实例化
            GameObject prefab = Resources.Load<GameObject>("ArrowLine");
            GameObject arrowLine = NGUITools.AddChild(parent, prefab);

            //脚本初始化
            ArrowLine script = arrowLine.GetComponent<ArrowLine>();
            script.Init(prefab);
            LogsSystem.Instance.Print("箭头初始化完毕");
            return script;
        }
        else
        {
            LogsSystem.Instance.Print("创建指向线的路径不存在," + parentPath, LogLevel.ERROR);
            return null;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init(GameObject prefab)
    {
        //变量赋值
        speed = 1;
        texture = GetComponent<UITexture>();
        texture.pivot = UIWidget.Pivot.Bottom;
        cursor = GameObject.FindGameObjectWithTag(Tags.Cursor);
        uicamera = GameObject.Find("UI Root/Camera").GetComponent<Camera>();
        minDis = 40;

        //基本设置
        transform.localScale = prefab.transform.localScale;
        GetComponent<UITexture>().alpha = 1.0f;
        gameObject.SetActive(false);
        isShowing = false;
    }

    private void Update()
    {

        if (isShowing)
        {
            if (cursor != null)
            {
                SetWidge();
                SetUVRect();
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
    private void SetWidge()
    {
        Vector2 cursorWorldPos = new Vector2(cursor.transform.position.x, cursor.transform.position.y);
        SetWidge(BeginPos, cursorWorldPos);
    }
    private void SetWidge(Vector2 from, Vector2 to)
    {
        Vector2 vpDir = uicamera.WorldToViewportPoint(to) - uicamera.WorldToViewportPoint(from);//视口坐标差
        Vector2 dir = Vector2.Scale(vpDir, Global.Instance.screenSize);

        float dis = dir.magnitude;
        int coefficient = dir.x > 0 / 2 ? -1 : 1;//方向系数
        float angle = Vector2.Angle(Vector2.up, dir) * coefficient;
        transform.position = new Vector3(from.x, from.y);
        transform.eulerAngles = new Vector3(0, 0, angle);
        texture.height = Mathf.FloorToInt(dis);

        //小于一定像素大小不显示线条
        if (dis < minDis)
        {
            texture.alpha = 0;
        }
        else
        {
            texture.alpha = 1;
        }
    }

    /// <summary>
    /// 设置箭头指向线的来源位置
    /// </summary>
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
        SetWidge();
        isShowing = true;
        gameObject.SetActive(true);
    }
    public void ShowArrowLine(Vector2 from)
    {
        SetBeginPos(from);
        ShowArrowLine();
    }
    public void ShowArrowLine(GameObject from)
    {
        SetBeginPos(from);
        ShowArrowLine();
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
