using UnityEngine;

namespace CatLike
{
    public class MovingSphere : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private float maxSpeed = 0;

        [SerializeField, Range(0, 100)] private float maxAcceleration = 0;

        [SerializeField] private Rect allowedArea;

        [SerializeField, Range(0, 10f)] private float jumpHight = 0;

        [SerializeField, Range(0, 10f)] private float maxAirJumps = 0;

        [SerializeField, Range(0, 100f)] private float maxAirAcceleration = 1f;

        [SerializeField, Range(0, 90f)] private float maxGroundAngel = 25f;

        private Vector3 velocity;

        private Vector3 desiredVelocity;

        private Rigidbody body;

        private bool desiredJump;

        private int groundContactCount;

        private bool OnGround => groundContactCount > 0;

        private int jumpPhase;

        private float minGroundDotProduct;

        private Vector3 contactNormal;


        private MeshRenderer meshRenderer;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();

            OnValidate();

            meshRenderer = GetComponent<MeshRenderer>();
        }


        private void Update()
        {
            //勾股定理原因，会导致斜角45度和直线移动速度不等于
            Vector2 playerInput;
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            desiredJump |= Input.GetButtonDown("Jump");


            desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

            meshRenderer.material.SetColor("_Color", Color.white * (groundContactCount * 0.25f));
        }

        private void FixedUpdate()
        {
            UpdateState();

            AdjustVelocity();

            if (desiredJump)
            {
                desiredJump = false;
                Jump();
            }

            body.velocity = velocity;

            ClearState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            EvaluateCollision(collision);
        }


        private void OnCollisionStay(Collision collision)
        {
            EvaluateCollision(collision);
        }

        private void OnValidate()
        {
            minGroundDotProduct = Mathf.Cos(maxGroundAngel * Mathf.Deg2Rad);
        }


        private void UpdateState()
        {
            velocity = body.velocity;
            if (OnGround)
            {
                jumpPhase = 0;
                if (groundContactCount > 1)
                {
                    contactNormal.Normalize();
                }
            }
            else
            {
                contactNormal = Vector3.up;
            }
        }

        private void Jump()
        {
            if (OnGround || jumpPhase < maxAirJumps)
            {
                jumpPhase += 1;

                float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHight);
                float alignedSpeed = Vector3.Dot(velocity, contactNormal);
                if (alignedSpeed > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }
                velocity += contactNormal * jumpSpeed;
            }
        }

        private void EvaluateCollision(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 nomrmal = collision.GetContact(i).normal;
                if (nomrmal.y >= minGroundDotProduct)
                {
                    groundContactCount += 1;
                    contactNormal += nomrmal;
                }
            }
        }

        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - contactNormal * Vector3.Dot(vector, contactNormal);
        }

        private void AdjustVelocity()
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }

        private void ClearState()
        {
            groundContactCount = 0;
            contactNormal = Vector3.zero;
        }

    }

}