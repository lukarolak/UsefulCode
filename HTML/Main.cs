using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Text;  
using System.IO;    

public class Main : MonoBehaviour {
	void PostHTTP(){
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create ("http://memorygameapi20171230021939.azurewebsites.net/api/score");
		string SendData = "{\"name\":\""+GameObject.FindGameObjectWithTag("PlayerName").GetComponent<Text>().text+"\",\"clicks\":"+Klikovi.ToString()+",\"pairs\":8,\"time\":\""+string.Format("{0:0}:{1:0}:{2:00}",Vrijeme/3600,(Vrijeme/60)%60,Vrijeme%60)+"\"}";
		print (SendData);
		byte[] data = Encoding.ASCII.GetBytes (SendData);
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		request.GetRequestStream ().Write (data, 0, data.Length);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
		string responsestring = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
		print (responsestring);

	}
	public GameObject InputField; //UI elementi koji se dodaju panelima
	public GameObject Button;
	void GetHTTP(){
		WebClient wb = new WebClient ();
		string data = wb.DownloadString ("http://memorygameapi20171230021939.azurewebsites.net/api/score"); //uzima string s adrese
		char[] SplitChars = { '[', '{', '"', ':', ',', ' ', '}', ']' }; //do kraja for petlje čistim string od neželjenih znakova
		string[] Data = data.Split (SplitChars);
		//string name; string clicks; string time;
		List<string> CistaData = new List<string>();
		for (int i = 0; i < Data.Length; i++) {
			if (Data [i] != "" && Data [i] != "id" && Data [i] != "name" && Data [i] != "clicks" && Data [i] != "pairs" && Data [i] != "time"){
				CistaData.Add (Data [i]);
			}
		}
		GameObject ButtonIns = Button; //dodavanje UI elemenata
		ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Ime";
		Instantiate (ButtonIns, GameObject.Find ("Left").transform);
		ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Klikovi";
		Instantiate (ButtonIns, GameObject.Find ("Center").transform);
		ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "Vrijeme";
		Instantiate (ButtonIns, GameObject.Find ("Right").transform);
		List<int> PoredakData = new List<int>();
		List<int> Poredak = new List<int>();
		for (int i = 0; i < CistaData.Count / 7; i++) { //Sortiranje podataka po broju klikova
			Poredak.Add(i);
			PoredakData.Add(System.Int32.Parse(CistaData [7 * i + 2]));
		}
		for (int i = 0; i < Poredak.Count-1; i++) {
			for (int j = 0; j < Poredak.Count-1; j++) {
				if (PoredakData [j] < PoredakData [j + 1]) {
					int temp = PoredakData [j];
					PoredakData [j] = PoredakData [j + 1];
					PoredakData [j + 1] = temp;

					temp = Poredak[j];
					Poredak[j] = Poredak [j + 1];
					Poredak [j + 1] = temp;
				}
			}
		}
		for (int i = 0; i < 10 /*i < CistaData.Count/7*/; i++) { //Dodavanje elemenata na panele prema sortiranoj listi Poredak(komentar unutar petlje prikauje sve podatke s Rest servisa)
			string ime = CistaData [7 * Poredak[i] + 1];
			string klikovi = CistaData [7 * Poredak[i] + 2];
			string vrijeme = CistaData [7 * Poredak[i] + 4]+":"+CistaData [7 * Poredak[i] + 5]+":"+CistaData [7 * Poredak[i] + 6];
			ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = ime;
			Instantiate (ButtonIns, GameObject.Find ("Left").transform);
			ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = klikovi;
			Instantiate (ButtonIns, GameObject.Find ("Center").transform);
			ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = vrijeme;
			Instantiate (ButtonIns, GameObject.Find ("Right").transform);
		}
		Instantiate (InputField, GameObject.Find ("Left").transform);
		ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = Klikovi.ToString();
		Instantiate (ButtonIns, GameObject.Find ("Center").transform);
		ButtonIns.transform.GetChild (0).gameObject.GetComponent<Text> ().text = string.Format("{0:0}:{1:0}:{2:00}",Vrijeme/3600,(Vrijeme/60)%60,Vrijeme%60); //broj sekunda u h/m/ss oblik
		Instantiate (ButtonIns, GameObject.Find ("Right").transform);
	}

	private int Klikovi = 0; private float Vrijeme = 0;

	public GameObject Tile; //GameObject u Prefab folderu
	public Material[] Materjali; //Array materijala napravljenih of spritova u Pictures folderu

	void PrefabPlacer(){ //Funkcija koja instance-a tile-ove u game
		List<int> ListaTekstura = new List<int> (); //Lista iz koje nasumično uzimam materijale za pozadinu tile-a
		for (int i = 0; i < 8; i++) {
			ListaTekstura.Add (i);ListaTekstura.Add (i);
		}

		for (int i = 2; i > -2; i--) {
			for (int j = 2; j > -2; j--) {
				Vector3 Position = new Vector3 (j, i, 1f); //pozicija tile-a u world space-u
				GameObject PlaceObject = Tile;

				int RngNumber = Random.Range (0, ListaTekstura.Count); //Stavljanje tekstura na pozadinu svakoga tile-a
				int Tex = ListaTekstura [RngNumber];
				ListaTekstura.Remove (Tex);
				PlaceObject.transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = Materjali [Tex];

				Instantiate (PlaceObject, Position, Quaternion.identity); 
			}
		}
		
	}
	void Start () {
		//PostHTTP();
		PrefabPlacer ();
	}

	public Canvas MainCanvas;
	private List<GameObject> OtvoreniObjekti = new List<GameObject>(); //lista koja sadrži GameObjekte koji su trenutno 'otvoreni'
	private bool CanClick = true; //Bool koji zabranjuje otvaranje novih tile-ova dok sve animacije nisu zavrsene
	private int BrojParova = 0; private bool Flag = true;
	void Update () {
		Vrijeme = Time.timeSinceLevelLoad;
		if (BrojParova == 8 && Flag) { //ako su svi parovi pronađeni
			Flag = false;
			MainCanvas.transform.GetChild (0).gameObject.SetActive (true); //uključivanje UI elemenata
			MainCanvas.transform.GetChild (1).gameObject.SetActive (true);
			GetHTTP (); //Popunjavanje UI elemenata
		}else if (Input.GetKey(KeyCode.Space) && Flag == false) { //Kada se pritisne Space
			PostHTTP();
			MainCanvas.transform.GetChild(0).gameObject.SetActive(false); //Ugasi sav UI
			MainCanvas.transform.GetChild(1).gameObject.SetActive(false);
			SceneManager.LoadScene( SceneManager.GetActiveScene().name ); //Reset scene
			Flag = true;
		}
		for (int i = 0; i < OtvoreniObjekti.Count; i++) {
			if (OtvoreniObjekti [i].GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsTag ("NotPlaying")) {
				CanClick = true; //Ako nema aktivnih animacija, dopusti otvaranje novih tile-ova
			} else {
				CanClick = false;
				break;
			}
		}
		if (OtvoreniObjekti.Count == 2 ){
			if(CanClick){
				if (OtvoreniObjekti [0].transform.GetChild (0).gameObject.GetComponent<Renderer> ().material.name == OtvoreniObjekti [1].transform.GetChild (0).gameObject.GetComponent<Renderer> ().material.name) {
					//Ako je ostvaren par, mičem BoxCollidere kako vise ne bih aktivirao animacije s Physics.Raycast
					for (int i = 0; i < 2; i++) {
						OtvoreniObjekti [i].GetComponent<BoxCollider> ().enabled = false; 
					}
					OtvoreniObjekti = new List<GameObject>(); //Ostvaren je par, stoga resetiram listu
					BrojParova++;
				} else {
					for (int i = 0; i < 2; i++) {
						OtvoreniObjekti [i].GetComponent<Animator> ().SetTrigger("ChangeState"); //Zatvaram otvorene tile-ove ako nisu par
					}
					OtvoreniObjekti = new List<GameObject>(); //Reset liste
				}
			}
		}
		else if (Input.GetMouseButtonDown(0) && CanClick){
			RaycastHit Hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out Hit)) { //Detektiram klik na tile
				Klikovi++;
				Hit.transform.GetComponent<Animator>().SetTrigger("ChangeState"); //Pokretanje animacije otvaranja tile-a
				CanClick = false;
				OtvoreniObjekti.Add (Hit.transform.gameObject); //Dodajem otvoreni tile u OtvoreneObjekte
			}
		}
	}
}
