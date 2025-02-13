using System.Collections.Generic;
using UnityEngine;

public class TreadmillManager : MonoBehaviour
{
    struct Part {
        public GameObject part;
        public Transform tail;
    }

    [SerializeField] int parts = 10;
    List<Part> partsList = new List<Part>();
    GameObject part;
    [SerializeField] float _height;

    [SerializeField] float speed = 1f;

    private void Awake() {
        {
            part = transform.GetChild(0).gameObject;
            Transform tail = part.transform.GetChild(0);
            partsList.Add(new Part {
                part = part,
                tail = tail
            });
        }
        
        for(int i = 0; i < parts; i++) {
            GameObject newPart = Instantiate(part, transform);
            Transform tail = newPart.transform.GetChild(0);

            partsList.Add(new Part {
                part = newPart,
                tail = tail
            });
        }
    }

    private void Update() {
        for(int i = 0; i < partsList.Count; i++) 
        {
            Part part = partsList[i];
            part.part.transform.position += Vector3.back * speed * Time.deltaTime;

            if(part.part.transform.localPosition.z < 0) {
                part.part.transform.position = partsList[partsList.Count - 1].tail.position;
                part.part.transform.localPosition.Set(part.part.transform.localPosition.x, _height, part.part.transform.localPosition.z);
                partsList.Remove(part);
                partsList.Add(part);
            }
        }
    }
}
