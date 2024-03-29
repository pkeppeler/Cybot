﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class RandomMap : MonoBehaviour {

    public GameObject star;
    public GameObject[] Cybots;
    private FirstPersonController fpc;
    private TextMeshProUGUI winGameTxt;
    private Button newGame;
    private Button mainMenu;
    private GameObject g;
    private bool isGameOver;
    public AudioClip winSound;
    public AudioClip loseSound;
    private int numCaptured;
    public int numChasing;
    public GameObject laser;

    private GameObject[] botsToUse;

    public List<GameObject> objects = new List<GameObject>();

    // Use this for initialization
    void Start () {
        int numBots = 0;
        for(int i = 0; i < GameOptions.cybots.Length; i++)
        {
            if (GameOptions.cybots[i])
                numBots++;
        }

        botsToUse = new GameObject[numBots];

        int j = 0;
        for (int i = 0; i < GameOptions.cybots.Length; i++)
        {
            if (GameOptions.cybots[i])
            {
                botsToUse[j] = Cybots[i];
                j++;
            }
        }


        winGameTxt = GameObject.FindGameObjectWithTag("WinGameText").GetComponent<TextMeshProUGUI>();
        newGame = GameObject.FindGameObjectWithTag("BnRestart").GetComponent<Button>();
        mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Button>();
        fpc = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        g = GameObject.FindGameObjectWithTag("Goal");

        NewGame();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Numcaptured = " + numCaptured);
	}

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Capture()
    {
        numCaptured++;
        if(numCaptured == 2)
        {
            LoseGame();
        }
    }

    public void Free()
    {
        numCaptured--;
        if(numCaptured < 0)
        {
            numCaptured = 0;
        }
    }

    public void LoseGame()
    {
        if (!isGameOver)
        {
            winGameTxt.enabled = true;
            winGameTxt.SetText("You lose!");
            newGame.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(true);
            Cursor.visible = true;
            Screen.lockCursor = false;
            isGameOver = true;
            fpc.GetComponent<AudioSource>().PlayOneShot(loseSound);
        }
    }

    public void WinGame()
    {
        if (!isGameOver)
        {
            winGameTxt.enabled = true;
            winGameTxt.SetText("You win!");
            newGame.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(true);
            Cursor.visible = true;
            Screen.lockCursor = false;
            isGameOver = true;
            fpc.GetComponent<AudioSource>().PlayOneShot(winSound);
        }
    }

    public void NewGame()
    {
        winGameTxt.enabled = false;
        newGame.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        Cursor.visible = false;
        Screen.lockCursor = true;
        isGameOver = false;
        numCaptured = 0;

        if (objects.Count > 0)
        {
            for(int i = 0; i < objects.Count; i++)
            {
                Destroy(objects[i]);
            }
            objects = new List<GameObject>();
        }

        fpc.sprintLeft = 6;
        fpc.canRun = true;
        fpc.frozenLeft = 0;
        fpc.isFrozen = false;

        int numStars = (int)Random.Range(12, 30);
        for (int i = 0; i < numStars; i++)
        {
            GameObject go = Instantiate(star);
            objects.Add(go);
            go.transform.SetParent(transform);
            go.transform.position = new Vector3(Random.Range(-57.4f, 57.4f), 1.5f, Random.Range(-57.4f, 57.4f));
        }

        if(botsToUse.Length > 0)
        {
            int numEnemies = (int)Random.Range(10, 20);
            for (int i = 0; i < numEnemies; i++)
            {
                GameObject npc;
                if (i > botsToUse.Length)
                    npc = botsToUse[Random.Range(0, botsToUse.Length)];
                else
                {
                    npc = botsToUse[i % botsToUse.Length];
                }
                Vector3 pos = new Vector3(Random.Range(-57.4f, 57.4f), 1.5f, Random.Range(-57.4f, 57.4f));
                GameObject go = Instantiate(npc, pos, Quaternion.identity);
                objects.Add(go);
                go.transform.SetParent(transform);
            }
        }
        

        Vector3 randStart = new Vector3(Random.Range(-57.4f, 57.4f), 2.63f, Random.Range(-57.4f, 57.4f));
        Vector3 randGoal = new Vector3(Random.Range(-57.4f, 57.4f), 2.63f, Random.Range(-57.4f, 57.4f));

        float dist = Vector3.Distance(randGoal, randStart);
        while (dist < 91)
        {
            randStart = new Vector3(Random.Range(-57.4f, 57.4f), 2.63f, Random.Range(-57.4f, 57.4f));
            randGoal = new Vector3(Random.Range(-57.4f, 57.4f), 2.63f, Random.Range(-57.4f, 57.4f));

            dist = Vector3.Distance(randGoal, randStart);
        }

        g.transform.SetParent(transform);
        g.transform.position = randGoal;
        
        fpc.transform.position = randStart;


        print("Created a new game!");
    }

}
