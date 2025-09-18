using UnityEngine;

public class PlayeController2 : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpPower = 5f;

    [Header("�Q��")]
    [SerializeField] Transform cameraTransform;

    [Header("�J�����֘A")]
    [SerializeField] Transform cameraPos;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // �ڒn����
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // �ڒn���艻
        }

        // ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            // �J������̈ړ������ɕϊ�
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        //�J��������
        cameraPos.position = transform.position;
        float cx = Input.GetAxisRaw("Vertical2");
        float cy = Input.GetAxisRaw("Horizontal2");
      cameraPos.RotateAround(transform.position, transform.up, cy * 250 * Time.deltaTime);
        cameraPos.RotateAround(transform.position, -1 * transform.right, cx * 250 * Time.deltaTime);
        // �W�����v
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }

        // �d��
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private float rotationVelocity;
}