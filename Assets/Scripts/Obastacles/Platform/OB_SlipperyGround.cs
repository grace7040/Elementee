using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_SlipperyGround : MonoBehaviour
{
    private PhysicMaterial slipperyMaterial; // 마찰력을 없앨 PhysicMaterial
    private Collider playerCollider; // 플레이어의 Collider

    private void Start()
    {
        // 마찰력을 없앨 PhysicMaterial 생성
        slipperyMaterial = new PhysicMaterial();
        slipperyMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        slipperyMaterial.staticFriction = 0f;
        slipperyMaterial.dynamicFriction = 0f;

        // 플레이어의 Collider 가져오기
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 만약 충돌한 오브젝트가 "Player" 태그를 가진 플레이어라면
        if (other.CompareTag("Player"))
        {
            // 플레이어의 Collider에 마찰력을 없앤 PhysicMaterial 적용
            if (playerCollider != null)
            {
                playerCollider.material = slipperyMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 오브젝트를 빠져나갔을 때, 기존 PhysicMaterial로 복원
        if (other.CompareTag("Player"))
        {
            if (playerCollider != null)
            {
                playerCollider.material = null; // 기존 PhysicMaterial로 변경하거나 다른 PhysicMaterial로 변경
            }
        }
    }
}
