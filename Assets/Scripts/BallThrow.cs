using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallThrow : MonoBehaviour
{

    private Vector3 mousePressDownPos;  //mouse basma pozisyonu
    private Vector3 mouseRelasePos;     //mouse serbest býrakma pozisyonu

    private Rigidbody rb;
    private bool isShoot;   //topa týklandý mý

    Vector3 forceInit;
    Vector3 forceV;    //bu hýzla top gönderilecek
    public bool touchedGround;
    public bool firstJump = false;      //ilk atýþdan sonra tekrar top atýlmasýn

    public PhysicMaterial material;

    public int MaxHeightValue;
    public float ShootingSpeed = 1;
    public float Bounce;
    public float ThresholdValue;

    Slider ShootingSpeedSlider;
    Slider BounceSlider;
    Slider ThresholdSlider;

    [SerializeField]
    public float forceMultipler = 2;   //atýþ büyüklüðü-güç çarpaný-topun gideceði mesafeyi ayarlar

    public static bool jumpability;     //zýplama durumu uygunluðu

    Text MaxHeightTxt;

    public static int ballThrowed = 0;        //kaç top atýldýðýnýn sayýsýný tutar

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        forceInit = (Input.mousePosition - mousePressDownPos);      //baþlangýç hýzý

        if (!isShoot)
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);   //Burada lineRenderer scriptine çizilecek linerenderer için hýz ve konum gönderiliyor

    }

    //TOPUN EÐÝK ATIÞ ANÝMASYONU GERÇEKLEÞTÝRÝLÝYOR

    void Update()
    {

        ShootingSpeedSlider = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Slider>();
        BounceSlider = GameObject.FindGameObjectWithTag("Bounce").GetComponent<Slider>();
        ThresholdSlider = GameObject.FindGameObjectWithTag("Threshold").GetComponent<Slider>();

        MaxHeightTxt = GameObject.FindGameObjectWithTag("MaxHeight").GetComponent<Text>();

        //BU BÝLGÝLERE GÖRE ATIÞ ÝÞLEMÝ GERÇEKLEÞECEK
        ShootingSpeed = float.Parse(ShootingSpeedSlider.value.ToString());
        Bounce = float.Parse(BounceSlider.value.ToString());
        ThresholdValue = float.Parse(ThresholdSlider.value.ToString());

        material.bounciness = Bounce;

        if (firstJump == false)
        {
            forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultipler;         //istediðimiz hedef noktaya topu gönderebilmek için vector3 cinsinden hýz vektörüne ihtiyacýmýz var
            float maxHeight = (forceInit.y * forceInit.y) / 2 * (-Physics.gravity.y);
            MaxHeightTxt.text = maxHeight.ToString();

        }

        if (firstJump == true && touchedGround == true)
        {
            if (gameObject.transform.position.y < ThresholdValue)
            {
                //i azaltýlacak
                ObliqueShot.i--;
                if (gameObject.tag == "InstaBall")
                    ObliqueShot.instaBallCounter--;
                Destroy(gameObject);
            }

        }

    }

    void OnCollisionStay(Collision temasedilen)
    {

        if (temasedilen.gameObject.tag == "JumpGround")
        {
            touchedGround = true;
        }

    }

    void OnCollisionExit(Collision temasedilen)
    {
        if (temasedilen.gameObject.tag == "JumpGround")
        {
            touchedGround = false;
        }
    }

    private void OnMouseUp()
    {
        DrawTrajectory.Instance.HideLine();                 //çizgiyi gizle
        mouseRelasePos = Input.mousePosition;
        Shoot(mouseRelasePos - mousePressDownPos);

        rb.AddForce(forceV * ShootingSpeed);        //top burada atýlýyor
        firstJump = true;

        ObliqueShot.singleOrDouble++;
        jumpability = true;
        ballThrowed++;
    }

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;
    }

}
