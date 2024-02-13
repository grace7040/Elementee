using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Default : MonsterController
{


    // Update is called once per frame
    void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            SetWaypoints();
            timer = 0.0f;
        }

        if (isKnockedBack) { }
        else
        {
            if (currentWaypoint != null)
            {
                MoveTowardsWaypoint();
            }
        }
    }
}
