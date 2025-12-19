using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f; // 鼠标灵敏度
    public float moveSpeed = 3f; // 移动速度
    public float jumpSpeed = 5f; // 跳跃速度
    private CharacterController controller; // 角色控制器
    private float verticalSpeed = 0f; // 用于垂直方向的速度

    void Start()
    {
        controller = GetComponent<CharacterController>(); // 获取角色控制器组件
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on the UI");
            return; // 如果鼠标在UI上，则不响应玩家输入
        }

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity; // 获取鼠标X轴移动距离
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity; // 获取鼠标Y轴移动距离

            Vector3 rotation = transform.localRotation.eulerAngles; // 获取当前旋转角度
            rotation.x -= mouseY; // Y轴旋转
            rotation.y += mouseX; // X轴旋转
            transform.localRotation = Quaternion.Euler(rotation); // 应用新的旋转角度
        }

        float horizontal = Input.GetAxis("Horizontal"); // 获取水平输入
        float vertical = Input.GetAxis("Vertical"); // 获取垂直输入

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical); // 移动方向
        moveDirection = moveDirection.normalized * moveSpeed; // 根据速度归一化移动方向
        moveDirection = controller.transform.TransformDirection(moveDirection); // 将移动方向转换为世界坐标

        // 添加跳跃功能
        if (controller.isGrounded && Input.GetButton("Jump")) // 角色在地面上并且跳跃键持续按下
        {
            if (Mathf.Approximately(horizontal, 0) && Mathf.Approximately(vertical, 0))
            {
                // 如果没有水平移动输入，则执行原地跳跃
                verticalSpeed = jumpSpeed;
            }
            else
            {
                // 否则执行移动中跳跃
                verticalSpeed = jumpSpeed;
            }
        }

        // 添加重力效果
        verticalSpeed -= 9.81f * Time.deltaTime; // 模拟重力
        moveDirection.y = verticalSpeed; // 添加垂直速度

        // 碰撞检测
        if (controller.isGrounded) // 角色在地面上
        {
            if (moveDirection.y < 0)
            {
                moveDirection.y = 0f; // 防止角色往下穿越地面
                verticalSpeed = 0f; // 重置垂直速度
            }
        }

        controller.Move(moveDirection * Time.deltaTime); // 移动角色
    }
}