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
    public Text BallsInSceneTxt;        //Ball tagine sahip top say�s�n� tutuyor

    public static int i = 0;        //Ball tagine sahip top say�s� (d��ar�dan m�dehale ediliyor)
    int j = 0;      //instantiate edilen top say�s� 

    Vector3 BallInstantiatePoint;       //���nlanacak toplar�n ���nlanma noktas�

    public GameObject[] InstaBall = new GameObject[8];       //Instantiate edilen toplar� tutan dizi
    public static int instaBallCounter = 0;     //Instantiate top sayisi (d��ar�dan m�dehale ediliyor)

    public Texture2D cursorTextureSelected;
    public Texture2D cursorTextureMain;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public static int singleOrDouble = 0;       //teklik ve �iftlik durumuna g�re mouse imleci de�i�ecek ve at�� olay� ger�ekle�ecek

    public Vector3 initalVelocity;
    LineRenderer lineRenderer;
    Rigidbody rb;

    public GameObject BallPrefab;        //Instatiate edilecek top 

    public Text NumberOfBallsWithBallTag;   //Ball tagine sahip top say�s�
    public Text actionToBeTakenTxt;

    void Start()
    {
        BallInstantiatePoint = GameObject.FindGameObjectWithTag("BallInstantiatePoint").transform.position;     //her top objesine tek tek bu objeleri atmak istemedi�im i�in tag bularak yapt�m. ama iste�e g�re public olarak da yap�labilir tabi
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
        BallsInSceneTxt.text = Ball.Length.ToString();       //top tagine sahip top say�s�n� tutuyor

        if (singleOrDouble % 2 == 0)
        {
            Cursor.SetCursor(cursorTextureSelected, hotSpot, cursorMode);   //cursor de�i�ecek
        }

        else if (singleOrDouble % 2 == 1)
        {
            Cursor.SetCursor(cursorTextureMain, hotSpot, cursorMode);   //cursor eski haline d�necek
        }

        GameObject[] InstaBalls = GameObject.FindGameObjectsWithTag("InstaBall");

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_ray, out _hit) && _hit.transform.tag == "JumpGround")
        {
            Vector3 MousePozisyon = new Vector3(_hit.point.x, _hit.point.y + 0.55f, _hit.point.z);

            if (singleOrDouble % 2 == 0)
            {
                if (BallThrow.ballThrowed > 0 && BallThrow.jumpability == true)
                {
                    //TOPU ZEM�NE YERLE�T�RME
                    if (BallThrow.ballThrowed <= 15 && GameObject.FindGameObjectsWithTag("Ball") != null)                        //at�lan top say�s� 16'dan k���kse Top[] dizisindeki toplar� atacak
                    {
                        Ball[i].transform.position = MousePozisyon;
                    }

                    else if (BallThrow.ballThrowed > 15 && BallThrow.ballThrowed <= 23 && GameObject.FindGameObjectsWithTag("InstaBall") != null)        //at�lan top say�s� 16'dan b�y�kse InstaTop[] dizisindeki toplar� atacak
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
                        i++;                          //kullan�lan top say�s� arts�n

                    singleOrDouble++;
                }

                if (BallThrow.ballThrowed == 0)
                {
                    //TOP ZEM�NE YERLE�T�RME

                    Ball[i].transform.position = MousePozisyon;

                    if (BallThrow.ballThrowed < 24)
                        i++;                          //kullan�lan top say�s� arts�n
                    singleOrDouble++;
                }

            }
        }

    }

    bool oneTime = true;         //birkez �al��mas� amac�yla bool de�i�ken kulland�k
    public void InstantiateBall()       //16'dan sonraki toplar� instantiate etmeye yarar
    {
        if (oneTime == true)
        {

            Instantiate(BallPrefab, BallInstantiatePoint, Quaternion.identity);
            BallPrefab.tag = "InstaBall";        //yeni �retilen toplar "InstaBall tagine sahip olacak"

            BallInstantiatePoint.z -= 1.1f;

            InstaBall[j] = BallPrefab;

            j++;
        }
        oneTime = false;
    }

}
