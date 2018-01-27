using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson {
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class UserControlP1 : MonoBehaviour {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        public Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private Rigidbody objRigidbody;
        public string state;
        public Animator anim;

        private void Start() {
            state = "free";
            
            objRigidbody = gameObject.GetComponent<Rigidbody>();
            // get the transform of the main camera
            if (m_Cam == null) {
                Debug.LogWarning("Warning: No camera assigned to controller.");
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update() {
            if (!m_Jump) {
                m_Jump = Input.GetButtonDown("JumpP1");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate() {
            if (state == "free") {
                // read inputs
                float h = Input.GetAxis("HorizontalP1");
                float v = -Input.GetAxis("VerticalP1");

                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;

                // pass all parameters to the character control script
                m_Character.Move(m_Move, m_Jump);
                m_Jump = false;
            }
        }
        public void setState(string s, triggerZone t) {
            if (s == state) {
                return;
            } else if (s == "free") {
                enablePhysics();
            } else if (t != null) {
                disablePhysics();
                StartCoroutine(tweenBaby(this.gameObject, t.babyLocation));
                anim.SetBool(s, true);
            }
        }

        public void OnTriggerStay(Collider other) {
            triggerZone t = null;
            if ((t = other.GetComponent<triggerZone>()) != null && state == "free" && Input.GetButtonDown("EnterPositionP1")) {
                setState(t.newStateOfBaby, t);
            } else if (state != "free" && Input.GetButtonDown("LeavePositionP1")) {
                setState("free", null);
            }
        }

        public void disablePhysics() {
            objRigidbody.isKinematic = false;
            objRigidbody.useGravity = false;
        }

        public void enablePhysics() {
            objRigidbody.isKinematic = true;
            objRigidbody.useGravity = true;
        }

        private IEnumerator tweenBaby(GameObject g, Transform newPos) {
            while (g.transform.position != newPos.transform.position && g.transform.rotation != newPos.transform.rotation) {
                g.transform.position = Vector3.Lerp(g.transform.position, newPos.transform.position, Time.deltaTime);
                g.transform.Rotate(Vector3.Lerp(g.transform.rotation.eulerAngles, newPos.transform.rotation.eulerAngles, Time.deltaTime));
                yield return null;
            }
        }
    }
}

