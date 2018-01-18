//example of post and get method

void PostHTTP(){
		//string istog formata kao i podatci na rest servisu
		string SendData = "{\"name\":\""+GameObject.FindGameObjectWithTag("PlayerName").GetComponent<Text>().text+"\",\"clicks\":"+Klikovi.ToString()+",\"pairs\":8,\"time\":\""+string.Format("{0:0}:{1:0}:{2:00}",Vrijeme/3600,(Vrijeme/60)%60,Vrijeme%60)+"\"}";

		File.AppendAllText(Application.dataPath+"/Resources/"+"TextData.txt",SendData);//Sprema podatke u txt file

		if (UseREST) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create ("http://memorygameapi20171230021939.azurewebsites.net/api/score");
			byte[] data = Encoding.ASCII.GetBytes (SendData);
			request.Method = "POST";
			request.ContentType = "application/json";
			request.ContentLength = data.Length;
			request.GetRequestStream ().Write (data, 0, data.Length);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			string responsestring = new StreamReader (response.GetResponseStream ()).ReadToEnd ();
			print (responsestring);
		}

	}
  void GetHTTP(bool PlayerInput){
		string data;

		/*
		TextAsset TextData = (TextAsset)Resources.Load ("TextData"); //uzima string s local txt file-a
		data = TextData.text;
		*/
		data = File.ReadAllText (Application.dataPath + "/Resources/" + "TextData.txt");

		WebClient wb = new WebClient ();
		if(UseREST) data = wb.DownloadString ("http://memorygameapi20171230021939.azurewebsites.net/api/score"); //uzima string s adrese
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
