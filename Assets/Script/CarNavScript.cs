using System.Collections;
using UnityEngine;

public class CarNavScript : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] carStartingPoint;

    private float time = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > 2.0f)
        {
            int starting = Random.Range(0, carStartingPoint.Length);
            int car = Random.Range(0, carPrefabs.Length);

            Quaternion rotation = Quaternion.identity;
            Vector3 direction = Vector3.forward;
            if (starting >= 4)
            {
                rotation = Quaternion.Euler(0, 180, 0); 
                direction = Vector3.back; 
            }

            GameObject carPrefab = Instantiate(carPrefabs[car], carStartingPoint[starting].position, rotation);


            StartCoroutine(MoveAndDestroyCar(carPrefab, direction));

            time = 0;
        }
    }

    private IEnumerator MoveAndDestroyCar(GameObject car, Vector3 direction)
    {
        float elapsedTime = 0f;
        Rigidbody rb = car.GetComponent<Rigidbody>();

        while (elapsedTime < 22f)
        {
            rb.MovePosition(rb.position + direction * 10.0f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(car);
    }
}