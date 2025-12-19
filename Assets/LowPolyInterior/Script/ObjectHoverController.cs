using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class ObjectHoverController : MonoBehaviour
{
    private Camera mainCamera; 
    public Text promptText; // 界面上提示文字的 Text 组件
    public Texture2D newCursorIcon; // 鼠标图标
    public LayerMask targetLayer; // Layer

    private const float PromptOffset = 30f; 
    private bool isCursorChanged = false; // 用于标记鼠标图标是否已改变

    void Start()
    {
        // 获取主相机
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        // 隐藏提示文字
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("PromptText is not assigned in the inspector!");
        }
    }

    void Update()
    {
 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            
            ResetPromptText();
            if (isCursorChanged)
            {
                ResetCursorIcon();
            }
            return;
        }

        // 射线检测以确定鼠标是否悬停在目标物体上
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetLayer))
        {
            // 如果射线击中指定的物体
            UpdatePromptText(true);
            if (!isCursorChanged)
            {
                UpdateCursorIcon(newCursorIcon); // 切换鼠标图标
                isCursorChanged = true;
            }

            // 检查鼠标点击（可以根据需要修改此部分）
            if (Input.GetMouseButtonDown(0)) // 左键点击
            {
                // 你可以在这里执行点击时的操作
                Debug.Log("物体被点击！");
            }
        }
        else
        {
            ResetPromptText();
            if (isCursorChanged)
            {
                ResetCursorIcon(); // 恢复默认鼠标图标
                isCursorChanged = false;
            }
        }
    }

    // 更新提示文本的显示状态
    private void UpdatePromptText(bool show)
    {
        if (promptText != null)
        {
            // 如果状态发生变化，才进行显示/隐藏
            if (promptText.gameObject.activeSelf != show)
            {
                promptText.gameObject.SetActive(show);
            }

            if (show)
            {
                UpdatePromptPosition();
            }
        }
    }

    // 更新提示文本的位置
    private void UpdatePromptPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (promptText != null)
        {
            // 仅在位置有变化时更新
            promptText.transform.position = new Vector3(mousePosition.x, mousePosition.y + PromptOffset, 0);
        }
    }

    // 设置新的鼠标图标
    private void UpdateCursorIcon(Texture2D cursorIcon)
    {
        if (cursorIcon != null)
        {
            Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto); // 设置鼠标图标
        }
    }

    // 恢复默认鼠标图标
    private void ResetCursorIcon()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 恢复默认鼠标图标
    }

    // 重置提示文本显示状态
    private void ResetPromptText()
    {
        if (promptText != null && promptText.gameObject.activeSelf)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
