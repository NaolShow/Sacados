using Unity.Netcode;
using UnityEngine;

namespace Sacados.Samples {

    /// <summary>
    /// Extremely simple (client authority) character controller
    /// </summary>
    public class SimpleCharacterController : NetworkBehaviour {

        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed;
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float sensitivity;

        private Vector2 rotation;

        public override void OnNetworkSpawn() {
            if (IsLocalPlayer) return;

            Destroy(controller);
            Destroy(camera);
        }

        private void Update() {
            if (!IsLocalPlayer) return;

            // Move towards the controller's facing direction
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
            movement = transform.TransformDirection(movement);
            controller.Move(movement);

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            rotation += new Vector2(-mouseDelta.y, mouseDelta.x) * sensitivity;
            rotation.x = Mathf.Clamp(rotation.x, -80, 80);
            rotation.y %= 360;

            // Apply the rotation to the camera and the controller
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            cameraPivot.localRotation = Quaternion.Euler(rotation.x, 0, 0);

            // Set the cursor state
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

    }

}