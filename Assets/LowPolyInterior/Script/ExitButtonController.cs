using UnityEngine;
using UnityEngine.UI;

public class ExitButtonController : MonoBehaviour
{
    public Button exitButton; // 退出

    void Start()
    {
        // 获取按钮
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(QuitApplication);
        }
        else
        {
            Debug.LogError("Exit button not assigned in the inspector.");
        }
    }

    // 点击按钮时退出
    public void QuitApplication()
    {
        Application.Quit(); // 退出Unity应用程序
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 如果在Unity编辑器中运行，停止播放模式
#endif
    }
}