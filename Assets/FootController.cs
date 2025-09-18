using UnityEngine;

public class FootController : MonoBehaviour
{
    public static bool isGrounded=false;
    [SerializeField] BoxCollider BoxCollider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {

            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
