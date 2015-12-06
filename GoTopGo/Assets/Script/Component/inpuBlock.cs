using UnityEngine;
using System.Collections;

namespace GoTop
{
    public class inpuBlock : MonoBehaviour
    {

        public GameObject[] block = new GameObject[7];

        public float generateTime;
        float delate;


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            delate -= Time.deltaTime;
            if (delate < 0)
            {
                transform.position = new Vector3(Random.Range(-4f, 4f), 5, 0);
                Instantiate(block[Random.Range(0, 7)], transform.position, Quaternion.identity);
                delate = generateTime;
            }


        }
    }
}
