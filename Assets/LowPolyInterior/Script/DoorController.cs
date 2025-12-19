using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;
    private bool doorOpen = false; // 门的当前状态
    public Text promptText; // 界面上提示文字的 Text 组件
    private Camera mainCamera; // 缓存主相机
    public AudioClip doorSound; // 门开关时播放的音频
    private AudioSource audioSource; // 音频源

    private const float PromptOffset = 30f; // 提示文本的偏移量

    void Start()
    {
        // 获取门动画组件
        doorAnimator = GetComponent<Animator>();
        if (doorAnimator == null)
        {
            Debug.LogError("Door animator not found!");
        }

        // 获取主相机
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        // 检查或添加 AudioSource 组件
        if (!TryGetComponent(out audioSource))
        {
            // 如果没有 AudioSource 组件，则动态添加
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 如果有音频文件，设置到音频源
        if (doorSound != null)
        {
            audioSource.clip = doorSound;
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
        // 射线检测以确定鼠标是否悬停在门对象上
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // 如果射线击中门对象
            {
                UpdatePromptText(true);

                // 检查鼠标点击
                if (Input.GetMouseButtonDown(0)) // 左键点击
                {
                    ToggleDoor();
                }
            }
            else
            {
                UpdatePromptText(false);
            }
        }
        else
        {
            UpdatePromptText(false);
        }
    }

    private void UpdatePromptText(bool show)
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(show);
            if (show)
            {
                UpdatePromptPosition();
            }
        }
    }

    private void UpdatePromptPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        // 提示文字位置在鼠标上方，适当偏移
        if (promptText != null)
        {
            promptText.transform.position = new Vector3(mousePosition.x, mousePosition.y + PromptOffset, 0);
        }
    }

    private void ToggleDoor()
    {
        // 切换门的状态
        doorOpen = !doorOpen;
        doorAnimator.SetTrigger(doorOpen ? "Open" : "Closed");

        // 播放门开关音频
        PlayDoorSound();
    }

    private void PlayDoorSound()
    {
        // 确保音频资源可用才播放
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound); // 播放音频
        }
    }
}
