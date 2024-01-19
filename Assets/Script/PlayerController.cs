using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    private CharacterController characterController;

    [SerializeField] private LayerMask layerMask;
    private Vector3 currentLookTarget = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 moveDirection = new Vector3(-Input.GetAxis("Horizontal"),0f,-Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection*moveSpeed);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction*500, Color.blue);

        if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
        {
            if(hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10*Time.deltaTime);
        }
    }
}
