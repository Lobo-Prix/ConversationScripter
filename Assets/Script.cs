using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Script : MonoBehaviour {

    public Image player_img;
    public Image npc_img;
    public Text player_name;
    public Text npc_name;
    public Text text;

    StreamReader f = new StreamReader("script.txt");
    int line_cnt = 1;

    Sprite LoadSprite(string path)
    {
        //Texture size does not matter. Texture2D.LoadImage deals with it.
        Texture2D ttr = new Texture2D(2, 2);
        byte[] byteArray = File.ReadAllBytes(path);
        if (!ttr.LoadImage(byteArray))
            throw new Exception("Image file not found(Line: " + line_cnt + ")");
        return Sprite.Create(ttr, new Rect(0, 0, ttr.width, ttr.height), Vector2.zero);
    }

    public void RunLine()
    {
        while (true)
        {
            string s = f.ReadLine();
            if (s == null)
                return;
            string[] toks = s.Split(new char[] { ' ' }, 2);
            switch (toks[0])
            {
                case "#":
                    continue;
                case "player_img":
                    if (toks.Length < 2)
                        throw new Exception("Error: Too few argument in player_img operator(Line: " + line_cnt + ")");
                    player_img.overrideSprite = LoadSprite(toks[1]);
                    break;
                case "npc_img":
                    if (toks.Length < 2)
                        throw new Exception("Error: Too few argument in npc_img operator(Line: " + line_cnt + ")");
                    npc_img.overrideSprite = LoadSprite(toks[1]);
                    break;
                case "npc_name":
                    if (toks.Length < 2)
                        throw new Exception("Error: Too few argument in npc_name operator(Line: " + line_cnt + ")");
                    npc_name.text = toks[1];
                    break;
                case "text":
                    if (toks.Length < 2)
                        throw new Exception("Error: Too few argument in text operator(Line: " + line_cnt + ")");
                    text.text = toks[1];
                    break;
                case "branch":
                    break;
                case "next":
                    if (toks.Length < 2)
                        throw new Exception("Error: Too few argument in next operator(Line: " + line_cnt + ")");
                    f.Close();
                    f = new StreamReader(toks[1]);
                    continue;
                case "end":
                    Application.Quit();
                    return;
                case "":
                    return;
                default:
                    throw new Exception("Error: Invalid operator. (Line: " + line_cnt + ")");
            }
            line_cnt++;
        }
    }

	void Start ()
    {
        RunLine();
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                RunLine();
            }
            catch (Exception e)
            {
                text.text = e.Message;
            }
        }
		
	}

    private void OnDestroy()
    {
        f.Close();
    }
}
