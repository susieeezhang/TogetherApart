using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    private Camera mainCamera; // 主相机
    private GameObject pickedObject; // 当前拾取的物体
    private float pickDistance = 0.5f; // 拾取物体时与相机的距离
    public LayerMask pickableLayer; // 可拾取物体的层级
    private bool isPickingUp = false; // 当前是否在拾取状态

    void Start()
    {
        // 获取主相机
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPickingUp) // 只有在没有拾取物体时才可以拾取
            {
                TryPickObject();
            }
        }

        // 持续跟随鼠标位置
        if (pickedObject != null)
        {
            FollowMouse();
        }

        // 检测鼠标左键释放
        if (Input.GetMouseButtonUp(0) && pickedObject != null)
        {
            DropObject();
        }
    }

    private void TryPickObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pickableLayer))
        {
            // 如果射线击中物体，设置为当前拾取的物体
            pickedObject = hit.transform.gameObject;
            // 将物体的位置设置为相机前方1米的位置
            pickedObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * pickDistance;

            // 使物体不受重力影响
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // 设置为运动学
            }

            isPickingUp = true; // 标记为正在拾取
        }
    }

    private void FollowMouse()
    {
        // 将物体位置设置为相机前方1米，保持在鼠标位置
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = pickDistance; // 将 z 设置为距离相机的距离
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        pickedObject.transform.position = targetPosition;
    }

    private void DropObject()
    {
        // 释放物体，恢复物理效果
        if (pickedObject != null)
        {
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 允许物体受力
            }
            pickedObject = null; // 清空当前拾取的物体
            isPickingUp = false; // 重置拾取状态
        }
    }
}
