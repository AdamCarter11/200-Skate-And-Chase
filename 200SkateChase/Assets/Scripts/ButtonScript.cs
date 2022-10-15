using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private GameObject diffPanel;
    private GameObject statsPanel;
    private GameObject AiButtonVar;
    private GameObject playsText;
    private GameObject winsText;

    private void Awake() {
        //the reason why I'm doing it like this is so that we could use this scripts in other scenes if we wanted
        if(AiButtonVar == null){
            AiButtonVar = GameObject.Find("AIButton");
        }
        if(playsText == null){
            playsText = GameObject.Find("NumberOfPlays");
        }
        if(winsText == null){
            winsText = GameObject.Find("NumberOfWins");
        }
        if(diffPanel == null){
            diffPanel = GameObject.Find("AI Difficulty panel");
            if(diffPanel != null){
                diffPanel.SetActive(false);
            }
        }
        if(statsPanel == null){
            statsPanel = GameObject.Find("StatsPanel");
            if(statsPanel != null){
                statsPanel.SetActive(false);
            }
        }
    }

    //activates the difficulty panel so we can select difficulty
    public void AiButton(){
        diffPanel.SetActive(true);
        AiButtonVar.SetActive(false);
    }
    //will be used for when we have a coop mode in
    public void CoopButton(){

    }

    //used to go back to the menu
    public void menuButton(){
        SceneManager.LoadScene("MainMenu");
    }

    //displays stats
    public void StatsButton(){
        statsPanel.SetActive(true);
        playsText.GetComponent<Text>().text = "Plays: " + PlayerPrefs.GetInt("Plays");
        winsText.GetComponent<Text>().text = "Wins: " + PlayerPrefs.GetInt("Wins");
    }
    //closes the stats panel
    public void CloseStats(){
        statsPanel.SetActive(false);
    }
    //clears the stats saved in player prefs
    public void ClearStats(){
        PlayerPrefs.DeleteAll();
        playsText.GetComponent<Text>().text = "Plays: " + PlayerPrefs.GetInt("Plays");
        winsText.GetComponent<Text>().text = "Wins: " + PlayerPrefs.GetInt("Wins");
    }

    //difficulty buttons for AI mode
    // **************************************************************************
    public void EasyButton(){
        //Change var to make game easier/harder (AI spawn objects faster, etc)
        Enemy.minSpawnTime = 4;
        Enemy.maxSpawnTime = 6;
        Obstacles.obsSpeed = 5f;

        savingData();

        //Loading the scene
        SceneManager.LoadScene("AiScene");
    }
    public void MediumButton(){
        //Change var to make game easier/harder (AI spawn objects faster, etc)
        Enemy.minSpawnTime = 2;
        Enemy.maxSpawnTime = 4;
        Obstacles.obsSpeed = 10f;

        savingData();

        //Loading the scene
        SceneManager.LoadScene("AiScene");
    }
    public void HardButton(){
        //Change var to make game easier/harder (AI spawn objects faster, etc)
        Enemy.minSpawnTime = 1;
        Enemy.maxSpawnTime = 2;
        Obstacles.obsSpeed = 15f;

        savingData();

        //Loading the scene
        SceneManager.LoadScene("AiScene");
    }
    // **************************************************************************

    //Super simple way of saving data (not secure, they could easily change the file)
    private void savingData(){
        int tempPlayHolder = PlayerPrefs.GetInt("Plays");
        tempPlayHolder++;
        PlayerPrefs.SetInt("Plays", tempPlayHolder);
    }
}
