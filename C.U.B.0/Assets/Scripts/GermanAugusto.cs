using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermanAugusto : MonoBehaviour
{
    public static GermanAugusto me;

    //PERSONAL INFORMATION
    public string fullName = "Germán Augusto";
    public string city = "Madrid";
    public string profile = "Video Games and Apps Programmer";

    //SKILLS
    public string skills = "Unity and C# > 1 year as hired programmer and > 2 years as indie developer";
    public string improvingSkills = "Unreal Engine, C++ and Blueprints";
    public enum Platform { PC, Android, IOS, HTML5 }

    //LANGUAGES
    public string nativeLanguage = "spanish";
    public string otherLanguage = "english";

    //CONTACT INFO
    public string email = "gdemiurgo@gmail.com";
    public string linkedInProfile = "https://www.linkedin.com/in/germanaugusto";

    
    private void Awake()
    {
        //I'm a singleton. Only one at a time!
        if(me != null)
        {
            Destroy(gameObject);
        }
        else
        {
            me = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //Welcome to my portfolio!
        Welcome();

        //A resume of my skills
        MySkills();

        //My languages
        MyLanguages();
    }

    private void Welcome()
    {
        string welcome = "Hi, my name is " + fullName + " and I'm a " + profile + " from " + city + "./n" +
                         "This is my online portfolio and here you will find some of my works and prototypes./n" +
                         "You can contact me in " + email + " and " + linkedInProfile;

        print(welcome);
    }

    private void MySkills()
    {
        string mySkills = "These are my skills: +/n" +
                          skills + " ./n" +
                          "And I'm learning " + improvingSkills;

        print(mySkills);
    }

    private void MyLanguages()
    {
        string languagesInfo = "I am native " + nativeLanguage + " speaker and I can write/read and mantain a fluid professional conversation in " + otherLanguage;

        print(languagesInfo);
    }

    //MY FUNCTIONS
    public void RapidPrototyping()
    {
        //Make a prototype in Unity with C# testing different mechanics and making changes quickly.
    }

    public void MakeAVideoGame(Platform platform)
    {
        switch(platform)
        {
            case Platform.PC:
                MakePCGame();
                break;
            case Platform.Android:
                MakeAndroidGame();
                break;
            case Platform.IOS:
                MakeIOSGame();
                break;
            case Platform.HTML5:
                MakeHTML5Game();
                break;
        }
    }

    private void MakePCGame()
    {
        //Make a PC game in Unity with C#
        //Make a PC game in Unreal Engine with Blueprints and C++
    }

    private void MakeAndroidGame()
    {
        //Make an Android app in Unity with C#
    }

    private void MakeIOSGame()
    {
        //Make an IOS app in Unity with C#
    }

    private void MakeHTML5Game()
    {
        //Make an HTML5 game in Unity with C#
    }
}
