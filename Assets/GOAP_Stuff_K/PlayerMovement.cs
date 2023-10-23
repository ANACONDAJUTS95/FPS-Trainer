using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float moveSpeed, gravityModifier, jumpPower, walkSpeed = 3.75f;
    public CharacterController charCon;

    private Vector3 moveInput;
    public Transform camTrans, groundCheckPoint;

    public float mouseSensitivity;
    public bool invertX, invertY, canJump;

    public LayerMask whatIsGround;

    public Animator anim;

    public AudioSource footstepFast, footstepSlow;

    public GameObject bullet;
    public Transform firePoint;

    public int bulletsFired = 0; // Tracks bullets fired
    private UIController uiController;

    // Add a reference to the BulletCounter script
    private BulletCounter bulletCounter;

    // bool checkers for player actions: // can add more in the future
    public bool 
        isPlayerInAir = false,
        isPlayerWalking = false,
        isPlayerRunning = false,
        isPlayerShooting = false,
        isPlayerZoomedIn = false;

    private void ResetIsPlayerInAir()
    {
        isPlayerInAir = false;
    }

    private void ResetIsPlayerWalking()
    {
        isPlayerWalking = false;
    }

    private void ResetIsPlayerRunning()
    {
        isPlayerRunning = false;
    }

    private void ResetIsPlayerShooting()
    {
        isPlayerShooting = false;
    }

    private void ResetIsPlayerZoomedIn()
    {
        isPlayerZoomedIn = false;
    }
    // add other reseters here, later

    //////////////////////////////////////////////////////////////////////////////////////
    
    // Ease for switching weapons
    public Gun activeGun;

    // ADS
    public Transform adsPoint, gunHolder;
    private Vector3 gunStartPos;
    public float adsSpeed = 2f;

    // Muzzle Flash
    public GameObject muzzleFlash;

    private void Awake()
    {
        instance = this;

        bulletCounter = GameObject.Find("BulletCounterManager").GetComponent<BulletCounter>();
    }


    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        gunStartPos = gunHolder.localPosition;

    }
    // IsInAir  IsPlayerMoving IsPlayerShooting IsPlayerZoomedIn 

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
                isPlayerWalking = true;
                isPlayerRunning = false;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
                isPlayerWalking = false;
                isPlayerRunning = true;
                // check for isPlayerRunning here
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
                moveInput.y = jumpPower;
                isPlayerInAir = true;
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
                isPlayerShooting = true; // player is shooting //find a way to reset isPlayerShooting to false, later
                Invoke("ResetIsPlayerShooting", 1.0f); // to reset the isPlayerShooting to false,
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
                isPlayerZoomedIn = true;
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
                isPlayerZoomedIn = false;
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
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            // Increment the bullet count using the BulletCounter component
            bulletCounter.bulletsFired++;

            // Update the UI with the current bullet count
            uiController.SetBulletsUsed(bulletCounter.bulletsFired);
            

            muzzleFlash.SetActive(true);
        }

    }
}
