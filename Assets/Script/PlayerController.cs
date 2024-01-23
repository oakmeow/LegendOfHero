using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    private CharacterController characterController;

    [SerializeField] private LayerMask layerMask;
    private Vector3 currentLookTarget = Vector3.zero;

    private Animator anim;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!GameManager.instance.GameOver)
        {
            Vector3 moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0f, -Input.GetAxis("Vertical"));

            bool isWalking = (moveDirection == Vector3.zero) ? false : true;
            anim.SetBool("isWalking", isWalking);

            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("DoubleChop");
            }
            if (Input.GetMouseButtonDown(1))
            {
                anim.Play("SpinAttack");
            }
            characterController.SimpleMove(moveDirection * moveSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.GameOver)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point != currentLookTarget)
                {
                    currentLookTarget = hit.point;
                }
                Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10 * Time.deltaTime);
            }
        }
    }
}
