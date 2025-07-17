using UnityEngine;
 
 public class GroundChecker : MonoBehaviour
 {
     [Header("Ground Check Settings")]
     [SerializeField] private Transform groundCheckPoint;
     [SerializeField] private float checkRadius = 0.2f;
     [SerializeField] private LayerMask groundLayer;
 
     public bool IsGrounded { get; private set; }
 
     private void Update()
     {
         IsGrounded = Physics.CheckSphere(groundCheckPoint.position, checkRadius, groundLayer);
     }
 
     private void OnDrawGizmosSelected()
     {
         if (groundCheckPoint != null)
         {
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
         }
     }
 }