using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObliqueShot : MonoBehaviour
{

    public Text UsedBallTxt;
    public Text CreatedBallTxt;
    public Text BallsInSceneTxt;        //Ball tagine sahip top sayýsýný tutuyor

    public static int i = 0;        //Ball tagine sahip top sayýsý (dýþarýdan müdehale ediliyor)
    int j = 0;      //instantiate edilen top sayýsý 

    Vector3 BallInstantiatePoint;       //ýþýnlanacak toplarýn ýþýnlanma noktasý

    public GameObject[] InstaBall = new GameObject[8];       //Instantiate edilen toplarý tutan dizi
    public static int instaBallCounter = 0;     //Instantiate top sayisi (dýþarýdan müdehale ediliyor)

    public Texture2D cursorTextureSelected;
    public Texture2D cursorTextureMain;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public static int singleOrDouble = 0;       //teklik ve çiftlik durumuna göre mouse imleci deðiþecek ve atýþ olayý gerçekleþecek

    public Vector3 initalVelocity;
    LineRenderer lineRenderer;
    Rigidbody rb;

    public GameObject BallPrefab;        //Instatiate edilecek top 

    public Text NumberOfBallsWithBallTag;   //Ball tagine sahip top sayýsý
    public Text actionToBeTakenTxt;

    void Start()
    {
        BallInstantiatePoint = GameObject.FindGameObjectWithTag("BallInstantiatePoint").transform.position;     //her top objesine tek tek bu objeleri atmak istemediðim için tag bularak yaptým. ama isteðe göre public olarak da yapýlabilir tabi
        instaBallCounter = 0;
        j = 0;
    }

    void Update()
    {
        actionToBeTakenTxt.text = singleOrDouble.ToString();
        NumberOfBallsWithBallTag.text = i.ToString();

        UsedBallTxt.text = BallThrow.ballThrowed.ToString();
        CreatedBallTxt.text = j.ToString();

        RaycastHit _hit;
        var _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (BallThrow.ballThrowed == 16)
            InstantiateBall();

        GameObject[] Ball = GameObject.FindGameObjectsWithTag("Ball");
        BallsInSceneTxt.text = Ball.Length.ToString();       //top tagine sahip top sayýsýný tutuyor

        if (singleOrDouble % 2 == 0)
        {
            Cursor.SetCursor(cursorTextureSelected, hotSpot, cursorMode);   //cursor deðiþecek
        }

        else if (singleOrDouble % 2 == 1)
        {
            Cursor.SetCursor(cursorTextureMain, hotSpot, cursorMode);   //cursor eski haline dönecek
        }

        GameObject[] InstaBalls = GameObject.FindGameObjectsWithTag("InstaBall");

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit) && _hit.transform.tag == "JumpGround")
        {
            Vector3 MousePozisyon = new Vector3(_hit.point.x, _hit.point.y + 0.55f, _hit.point.z);

            if (singleOrDouble % 2 == 0)
            {
                if (BallThrow.ballThrowed > 0 && BallThrow.jumpability == true)
                {
                    //TOPU ZEMÝNE YERLEÞTÝRME
                    if (BallThrow.ballThrowed <= 15 && GameObject.FindGameObjectsWithTag("Ball") != null)                        //atýlan top sayýsý 16'dan küçükse Top[] dizisindeki toplarý atacak
                    {
                        Ball[i].transform.position = MousePozisyon;
                    }

                    else if (BallThrow.ballThrowed > 15 && BallThrow.ballThrowed <= 23 && GameObject.FindGameObjectsWithTag("InstaBall") != null)        //atýlan top sayýsý 16'dan büyükse InstaTop[] dizisindeki toplarý atacak
                    {

                        InstaBalls[instaBallCounter].transform.position = MousePozisyon;
                        //InstaTop[instaTopSayi].tag = "AlinmisTop";
                        instaBallCounter++;

                        if (BallThrow.ballThrowed < 23)
                        {
                            oneTime = true;
                            InstantiateBall();
                        }
                    }

                    if (BallThrow.ballThrowed < 16)
                        i++;                          //kullanýlan top sayýsý artsýn

                    singleOrDouble++;
                }

                if (BallThrow.ballThrowed == 0)
                {
                    //TOP ZEMÝNE YERLEÞTÝRME

                    Ball[i].transform.position = MousePozisyon;

                    if (BallThrow.ballThrowed < 24)
                        i++;                          //kullanýlan top sayýsý artsýn
                    singleOrDouble++;
                }

            }
        }

    }

    bool oneTime = true;         //birkez çalýþmasý amacýyla bool deðiþken kullandýk
    public void InstantiateBall()       //16'dan sonraki toplarý instantiate etmeye yarar
    {
        if (oneTime == true)
        {

            Instantiate(BallPrefab, BallInstantiatePoint, Quaternion.identity);
            BallPrefab.tag = "InstaBall";        //yeni üretilen toplar "InstaBall tagine sahip olacak"

            BallInstantiatePoint.z -= 1.1f;

            InstaBall[j] = BallPrefab;

            j++;
        }
        oneTime = false;
    }

}
