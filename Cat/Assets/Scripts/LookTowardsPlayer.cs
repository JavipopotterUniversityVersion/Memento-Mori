using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    Transform player;
    [SerializeField] bool _invert = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        if (_invert) direction *= -1;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
    }
}
