using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpPower, walkSpeed = 3.75f;
    public CharacterController charCon;

    private Vector3 moveInput;
    public Transform camTrans, groundCheckPoint;

    public float maxViewAngle = 60f;

    public float mouseSensitivity;
    private float defaultSensitivity = 2.0f; // Set the default sensitivity to your preferred value

    public bool invertX, invertY, canJump;

    public LayerMask whatIsGround;

    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;

    // Ease for switching weapons
    public Gun activeGun;

    //ADS
    public Transform adsPoint, gunHolder;
    private Vector3 gunStartPos;
    public float adsSpeed = 2f;

    //Muzzle Flash
    public GameObject muzzleFlash;

    //AudioSource
    public AudioSource footstepFast, footstepSlow;

    //public int bulletsFired = 0; // Tracks bullets fired
    // Add a reference to the BulletCounter script
    private BulletCounter bulletCounter;
    private UIController uiController;

    private void Awake()
    {
        instance = this;
        uiController = UIController.instance;

        // Get a reference to the BulletCounter script on the "BulletCounterManager" GameObject
        bulletCounter = GameObject.Find("BulletCounterManager").GetComponent<BulletCounter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        gunStartPos = gunHolder.localPosition;

        //Mouse Sensitivity
        // Load the sensitivity value from PlayerPrefs or another persistent storage
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", defaultSensitivity);

        // Set the player's camera sensitivity
        mouseSensitivity = savedSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy && !UIController.instance.endGameUI.activeInHierarchy)
        {

            //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            //y velocity (for Gravity)
            float yStore = moveInput.y;

            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * walkSpeed;
            }

            else
            {
                moveInput = moveInput * moveSpeed;
            }

            moveInput.y = yStore;

            //Gravity
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }

            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

            //Jump Control
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                int randomIndex = Random.Range(5, 7); // Generate a random index (0 to 2 for hurt sounds)
                
                moveInput.y = jumpPower;

                //SFX for jumping
                AudioManager.instance.PlaySFX(randomIndex);
            }


            charCon.Move(moveInput * Time.deltaTime);

            //camera rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            //inverting controls
            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }

            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
             
            if (camTrans.rotation.eulerAngles.x > maxViewAngle && camTrans.rotation.eulerAngles.x < 180f)
            {
                camTrans.rotation = Quaternion.Euler(maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }

            else if (camTrans.rotation.eulerAngles.x > 180f && camTrans.rotation.eulerAngles.x < 360f - maxViewAngle)
            {
                camTrans.rotation = Quaternion.Euler(-maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }

            muzzleFlash.SetActive(false);

            //Handle Shooting
            //single shot
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }

                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }


                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            // repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmmount);
            }

            if (Input.GetMouseButton(1))
            {
                gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
            }

            else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }


            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);

        }
    
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            // Increment the bullet count using the BulletCounter component
            bulletCounter.bulletsFired++;

            // Update the UI with the current bullet count
            uiController.SetBulletsUsed(bulletCounter.bulletsFired);
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            muzzleFlash.SetActive(true);
        }
        
    }
}
