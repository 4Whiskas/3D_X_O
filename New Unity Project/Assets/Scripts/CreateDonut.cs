using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = System.Random;

public class CreateDonut : MonoBehaviour
{
    public Material winMat;
    public byte curentLine;
    public byte curentRow;
    public GameObject restartButton;
    public GameObject whoWins;
    public GameObject reddonut;
    public GameObject bluedonut;
    private Stopwatch watch;
    public GameObject palka;
    private static GameObject [][][]allDonuts=new GameObject[3][][];
    private static ChangeSkyBox change;
    private static bool donutColor = true;
    private static byte [][][]map;
    public Material redSky;
    public Material blueSky;

    private int maxDonuts = 0;

    //private Touch touch;
    // Start is called before the first frame update
    void Start()
    {
        watch = new Stopwatch();
        FillMap();
        watch.Start();
        for (int i = 0; i < 3; i++)
        {
            allDonuts[i] = new GameObject[3][];
            for (int j = 0; j < 3; j++)
            {
                allDonuts[i][j] = new GameObject[3];
            }
        }
        //change = gameObject.AddComponent<ChangeSkyBox>();
        RenderSettings.skybox = redSky;
    }

    public void FillMap()
    {
        map = new byte[3][][];
        for (int i = 0; i < 3; i++)
        {
            map[i] = new byte[3][];
            for (int j = 0; j < 3; j++)
            {
                map[i][j] = new byte[3];
                for (int k = 0; k < 3; k++)
                {
                    map[i][j][k] = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
       
        if (watch.ElapsedMilliseconds > 1000)
        {
            if (maxDonuts < 3)
            {
                if (donutColor)
                {
                    allDonuts[curentLine][curentRow][maxDonuts] = Instantiate(reddonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), reddonut.transform.rotation);
                    donutColor = false;
                    map[curentLine][curentRow][maxDonuts] = 2;
                    RenderSettings.skybox = blueSky;
                }
                else
                {
                    allDonuts[curentLine][curentRow][maxDonuts] = Instantiate(bluedonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), bluedonut.transform.rotation);
                    donutColor = true;
                    map[curentLine][curentRow][maxDonuts] = 1;
                    RenderSettings.skybox = redSky;
                }
                if (CheckForNoboyWins(map))
                {
                    donutColor = true;
                    whoWins.SetActive(true);
                    whoWins.GetComponent<Text>().text = "Nobody wins";
                    whoWins.GetComponent<Text>().color = Color.magenta;
                    restartButton.SetActive(true);
                }
                SeparateOnPlates(map,allDonuts);
                maxDonuts++;
            }
            watch.Restart();
        }

    }

    private bool CheckForNoboyWins(byte[][][] gameBoard)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (gameBoard[i][j][k] == 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void SeparateOnPlates(byte[][][] gameBoard, GameObject[][][] donuts)
    {
        for (int i = 0; i < 3; i++)
        {
            SeparateOnRows(new byte[][] {gameBoard[i][0], gameBoard[i][1],gameBoard[i][2]}, new GameObject[][]{donuts[i][0],donuts[i][1],donuts[i][2]});
            SeparateOnRows(new byte[][] { gameBoard[0][i], gameBoard[1][i], gameBoard[2][i]}, new GameObject[][]{donuts[0][i],donuts[1][i],donuts[2][i]} );
        }
        SeparateOnRows(new byte[][] { gameBoard[0][0], gameBoard[1][1], gameBoard[2][2] },new GameObject[][]{donuts[0][0],donuts[1][1],donuts[2][2]});
        SeparateOnRows(new byte[][] { gameBoard[0][2], gameBoard[1][1], gameBoard[2][0]},new GameObject[][]{donuts[0][2],donuts[1][1],donuts[2][0]} );
    }

    public void CheckRow(byte[] row,GameObject[] donuts)
    {
        if (row[0] == 2 && row[1] == 2 && row[2] == 2)
        {
            donutColor = true;
            whoWins.GetComponent<Text>().text = "Red wins";
            whoWins.GetComponent<Text>().color = Color.red;
            //if(donuts[0]!=null)
                donuts[0].GetComponent<Renderer>().material = winMat;
            //if(donuts[1]!=null)
                donuts[1].GetComponent<Renderer>().material = winMat;
                    //if(donuts[2]!=null)
                donuts[2].GetComponent<Renderer>().material = winMat;
            //palka.GetComponent<Renderer>().material = winMat;
            restartButton.SetActive(true);
        }
        else if (row[0] == 1 && row[1] == 1 && row[2] == 1)
        {
            donutColor = true;
            whoWins.GetComponent<Text>().text = "Blue wins";
            whoWins.GetComponent<Text>().color = Color.blue;
            //if(donuts[0]!=null)
                donuts[0].GetComponent<Renderer>().material = winMat;
            //if(donuts[1]!=null)
                donuts[1].GetComponent<Renderer>().material = winMat;
            //if(donuts[2]!=null)
                donuts[2].GetComponent<Renderer>().material = winMat;
            //palka.GetComponent<Renderer>().material = winMat;
            restartButton.SetActive(true);
        }
    }

    public void SeparateOnRows(byte[][] toSep, GameObject[][] donuts)
    {
        for (int i = 0; i < 3; i++)
        {
            CheckRow(new byte[] { toSep[i][0], toSep[i][1], toSep[i][2]},new GameObject[]{donuts[i][0],donuts[i][1],donuts[i][2]});
        }
        for (int i = 0; i < 3; i++)
        {
            CheckRow(new byte[] { toSep[0][i], toSep[1][i], toSep[2][i] },new GameObject[]{donuts[0][i],donuts[1][i],donuts[0][i]});
        }
        CheckRow(new byte[] { toSep[0][0], toSep[1][1], toSep[2][2]},new GameObject[]{donuts[0][0],donuts[1][1],donuts[2][2]});
        CheckRow(new byte[] { toSep[0][2], toSep[1][1], toSep[2][0]},new GameObject[]{donuts[0][2],donuts[1][1],donuts[2][0]});
        /*for (int i = 0; i < 3; i++)
        {
            CheckPlayerWins(new byte[] { toSep[i][0], toSep[i][1], toSep[i][2]},(byte)i,(byte)i,(byte)i);
        }
        for (int i = 0; i < 3; i++)
        {
            CheckPlayerWins(new byte[] { toSep[0][i], toSep[1][i], toSep[2][i] },(byte)i,(byte)i,(byte)i);
        }
        CheckPlayerWins(new byte[] { toSep[0][0], toSep[1][1], toSep[2][2]},0,1,2);
        CheckPlayerWins(new byte[] { toSep[0][2], toSep[1][1], toSep[2][0]},0,1,2);*/
    }

    public void CheckPlayerWins(byte[] row,byte curLine,byte cur2Line,byte cur3Line)
    {
        if ((row[0] == 2 && row[1] == 2 && row[2]==0))
        {
            allDonuts[curLine][2][maxDonuts] = Instantiate(bluedonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), bluedonut.transform.rotation);
            donutColor = true;
            map[curLine][2][maxDonuts] = 1;
            RenderSettings.skybox = redSky;
        }
        else if(row[0] == 0 && row[1] == 2 && row[2]==2)
        {
            allDonuts[curLine][0][maxDonuts] = Instantiate(bluedonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), bluedonut.transform.rotation);
            donutColor = true;
            map[curLine][0][maxDonuts] = 1;
            RenderSettings.skybox = redSky;
        }
        else if (row[0] == 2 && row[1] == 0 && row[2] == 2)
        {
            allDonuts[curLine][1][maxDonuts] = Instantiate(bluedonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), bluedonut.transform.rotation);
            donutColor = true;
            map[curLine][1][maxDonuts] = 1;
            RenderSettings.skybox = redSky;
        }
        else
        {
            Random r = new Random();
            int i = r.Next(0, 2);
            int j = r.Next(0, 2);
            allDonuts[i][j][maxDonuts] = Instantiate(bluedonut, new Vector3(palka.transform.position.x, palka.transform.position.y + 2.5f, palka.transform.position.z), bluedonut.transform.rotation);
            donutColor = true;
            map[curLine][1][maxDonuts] = 1;
            RenderSettings.skybox = redSky;
        }
    }

}
