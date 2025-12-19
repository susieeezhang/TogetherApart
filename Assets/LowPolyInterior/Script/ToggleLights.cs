using UnityEngine;

public class ToggleLights : MonoBehaviour
{
    public Light[] spotLights; // 灯光数
    public Camera mainCamera; // 主相机

    private bool lightsOn = false; // 灯光是否开启的状态

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 如果射线击中了物体
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // 切换灯光的状态
                    lightsOn = !lightsOn;
                    // 根据灯光的当前状态打开或关闭灯光
                    ToggleLightsState(lightsOn);
                }
            }
        }
    }

    // 根据状态打开或关闭灯光
    private void ToggleLightsState(bool state)
    {
        foreach (Light light in spotLights)
        {
            // 设置灯光的开启或关闭
            light.enabled = state;
        }
    }
}