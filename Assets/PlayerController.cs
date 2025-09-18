using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Animator anim;
    [SerializeField] float speed;
    [SerializeField] CharacterController characontroller;
    [SerializeField] GameObject CamMove,CamPos;
    Vector3 dir,camdir;
    bool isGrounded;
    const float RotateSpeed = 720f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        CamPos.transform.position = transform.position;
        CamMove.transform.position = transform.position;
        //着地しているかの判定
        isGrounded =characontroller.isGrounded;
       //移動
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        //Transform cam = Camera.main.transform;
        //Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        //Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;

        //// 入力をカメラ基準の方向に変換
        //Vector3 moveDir = (camForward * z + camRight * x).normalized;
        //if (moveDir.magnitude >= 0.1f)
        //{
        //    characontroller.Move(moveDir * speed * Time.deltaTime);

        //    //// 歩いている方向を向く
        //    //Quaternion from = transform.rotation;
        //    //Quaternion to = Quaternion.LookRotation(moveDir);
        //    //transform.rotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);
        //}

          dir.x = x;

        float z1 = z;
        float x1 = x;
        characontroller.Move(dir*speed*Time.deltaTime);

        //歩いている方向を向くやつ
        if (z1 != 0 || x1 != 0)
        {
            Vector3 direction = new Vector3(x1, 0, z1);
            Quaternion from = transform.localRotation;
            Quaternion to = Quaternion.LookRotation(direction);
            transform.localRotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);
        }
        Debug.Log(z);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            anim.CrossFade("Run", 0.15f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.CrossFade("Walk", 0.15f);
        }
        //カメラの動き
        float cx= Input.GetAxisRaw("Vertical2");
        float cy = Input.GetAxisRaw("Horizontal2");
       // Debug.Log(Input.GetAxisRaw("Vertical2"));
        transform.eulerAngles= new Vector3(transform.eulerAngles.x, cy, transform.eulerAngles.z);
        CamMove.transform.RotateAround(transform.position,    CamPos.transform.up   , cy * 250 * Time.deltaTime);
        CamMove.transform.RotateAround(transform.position, -1*CamMove.transform.right, cx * 250 * Time.deltaTime);
        
        // IsConfirmPressed();
    }
    //コントローラの識別
    //bool IsConfirmPressed()
    //{
    //    if (Gamepad.current == null) return false;
    //    Debug.Log("接続あり");
    //    if (Gamepad.current.displayName.Contains("Nintendo"))
    //    {
    //        Debug.Log("SW");
    //        // Switch系 → AボタンをB扱いに
    //        return Gamepad.current.buttonEast.wasPressedThisFrame; // Nintendo "A"
    //    }
    //    else
    //    {
    //        Debug.Log("XBOX");
    //        // Xbox系 → Aボタンをそのまま
    //        return Gamepad.current.buttonSouth.wasPressedThisFrame; // Xbox "A"
    //    }
    //}

}
