using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsPlayer : MonoBehaviour
{
    Transform player;
    [SerializeField] bool _invert = true;
    [SerializeField] bool _xFollow = false;
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
        transform.rotation = new Quaternion(_xFollow ? rotation.x : 0, rotation.y, _xFollow ? rotation.z : 0, rotation.w);
    }
}
