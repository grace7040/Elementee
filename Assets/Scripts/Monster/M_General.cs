using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class M_General : MonoBehaviour
{
    public Transform[] waypoints; // AI가 이동할 Waypoint들의 배열

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
        // 현재 Waypoint로 이동
        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, movementSpeed * Time.deltaTime);

        // 만약 AI가 현재 Waypoint에 도착했다면 다음 Waypoint로 변경
        if (Vector2.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        // 다음 Waypoint을 설정하고, 배열의 끝에 도달하면 처음 Waypoint으로 돌아감
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
            TakeDamage(other.GetComponentInParent<PlayerController>().damage); // 이거 맞나?
        }
        else if (other.tag == "WeaponB")
        {
            TakeDamage(20);
            Destroy(other.gameObject, 0.1f);
        }
    }
}
