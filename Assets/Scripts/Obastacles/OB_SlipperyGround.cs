using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_SlipperyGround : MonoBehaviour
{
    private PhysicMaterial slipperyMaterial; // �������� ���� PhysicMaterial
    private Collider playerCollider; // �÷��̾��� Collider

    private void Start()
    {
        // �������� ���� PhysicMaterial ����
        slipperyMaterial = new PhysicMaterial();
        slipperyMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        slipperyMaterial.staticFriction = 0f;
        slipperyMaterial.dynamicFriction = 0f;

        // �÷��̾��� Collider ��������
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �浹�� ������Ʈ�� "Player" �±׸� ���� �÷��̾���
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� Collider�� �������� ���� PhysicMaterial ����
            if (playerCollider != null)
            {
                playerCollider.material = slipperyMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ ������Ʈ�� ���������� ��, ���� PhysicMaterial�� ����
        if (other.CompareTag("Player"))
        {
            if (playerCollider != null)
            {
                playerCollider.material = null; // ���� PhysicMaterial�� �����ϰų� �ٸ� PhysicMaterial�� ����
            }
        }
    }
}
