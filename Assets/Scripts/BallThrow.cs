using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallThrow : MonoBehaviour
{

    private Vector3 mousePressDownPos;  //mouse basma pozisyonu
    private Vector3 mouseRelasePos;     //mouse serbest b�rakma pozisyonu

    private Rigidbody rb;
    private bool isShoot;   //topa t�kland� m�

    Vector3 forceInit;
    Vector3 forceV;    //bu h�zla top g�nderilecek
    public bool touchedGround;
    public bool firstJump = false;      //ilk at��dan sonra tekrar top at�lmas�n

    public PhysicMaterial material;

    public int MaxHeightValue;
    public float ShootingSpeed = 1;
    public float Bounce;
    public float ThresholdValue;

    Slider ShootingSpeedSlider;
    Slider BounceSlider;
    Slider ThresholdSlider;

    [SerializeField]
    public float forceMultipler = 2;   //at�� b�y�kl���-g�� �arpan�-topun gidece�i mesafeyi ayarlar

    public static bool jumpability;     //z�plama durumu uygunlu�u

    Text MaxHeightTxt;

    public static int ballThrowed = 0;        //ka� top at�ld���n�n say�s�n� tutar

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
        forceInit = (Input.mousePosition - mousePressDownPos);      //ba�lang�� h�z�

        if (!isShoot)
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);   //Burada lineRenderer scriptine �izilecek linerenderer i�in h�z ve konum g�nderiliyor

    }

    //TOPUN E��K ATI� AN�MASYONU GER�EKLE�T�R�L�YOR

    void Update()
    {

        ShootingSpeedSlider = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Slider>();
        BounceSlider = GameObject.FindGameObjectWithTag("Bounce").GetComponent<Slider>();
        ThresholdSlider = GameObject.FindGameObjectWithTag("Threshold").GetComponent<Slider>();

        MaxHeightTxt = GameObject.FindGameObjectWithTag("MaxHeight").GetComponent<Text>();

        //BU B�LG�LERE G�RE ATI� ��LEM� GER�EKLE�ECEK
        ShootingSpeed = float.Parse(ShootingSpeedSlider.value.ToString());
        Bounce = float.Parse(BounceSlider.value.ToString());
        ThresholdValue = float.Parse(ThresholdSlider.value.ToString());

        material.bounciness = Bounce;

        if (firstJump == false)
        {
            forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultipler;         //istedi�imiz hedef noktaya topu g�nderebilmek i�in vector3 cinsinden h�z vekt�r�ne ihtiyac�m�z var
            float maxHeight = (forceInit.y * forceInit.y) / 2 * (-Physics.gravity.y);
            MaxHeightTxt.text = maxHeight.ToString();

        }

        if (firstJump == true && touchedGround == true)
        {
            if (gameObject.transform.position.y < ThresholdValue)
            {
                //i azalt�lacak
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
        DrawTrajectory.Instance.HideLine();                 //�izgiyi gizle
        mouseRelasePos = Input.mousePosition;
        Shoot(mouseRelasePos - mousePressDownPos);

        rb.AddForce(forceV * ShootingSpeed);        //top burada at�l�yor
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
