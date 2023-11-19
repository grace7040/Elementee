using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class M_General : MonoBehaviour
{
    public Transform[] waypoints; // AI�� �̵��� Waypoint���� �迭

    private int currentWaypointIndex = 0;
    private Transform currentWaypoint;
    public float movementSpeed = 3f;

    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        if (waypoints.Length > 0)
        {
            currentWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private void Update()
    {
        if (currentWaypoint != null)
        {
            MoveTowardsWaypoint();
        }
    }

    private void MoveTowardsWaypoint()
    {
        // ���� Waypoint�� �̵�
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, movementSpeed * Time.deltaTime);

        // ���� AI�� ���� Waypoint�� �����ߴٸ� ���� Waypoint�� ����
        if (Vector2.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        // ���� Waypoint�� �����ϰ�, �迭�� ���� �����ϸ� ó�� Waypoint���� ���ư�
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        currentWaypoint = waypoints[currentWaypointIndex];
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Weapon")
        {
            Debug.Log(123);
            TakeDamage(other.GetComponentInParent<PlayerController>().damage); // �̰� �³�?
        }
        else if (other.tag == "WeaponB")
        {
            TakeDamage(20);
            Destroy(other.gameObject, 0.1f);
        }
    }
}
