using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //TODO: ADD MAX NUMBER OF REPETITIONS OPTION, AND MAKE IT POSSIBLE TO HOLD BUTTONS

    #region variables
    public Text Textseconds;
    public Camera mainCam;
    public Text Textreps;
    public Slider progress;
    public InputField mins;
    public InputField secs;
    private Animation anim;
    public Button pauseButton;
    public GameObject resetPanel;
    public Text resetTime;
    private int minInt;
    private int secInt;
    public float seconds;
    public int reps = 1;
    public int maxTime = 20;
    public float restartTime = 3;
    public float restartCountdown;
    public bool i = false;
    public bool lightMode = false;
    public bool showMS = false;
    public bool countDown = false;
    public bool pause = false;
    public Text msText;
    public Text lightModeText;
    public Text countDirText;
    public Text pauseText;
    public Text[] colorChangeText;
    public Color camBackgroundDark;
    public Color camBackgroundLight;
    public Color textDark;
    public Color textLight;
    #endregion 

    void Start()
    {
        resetPanel.SetActive(false);
        lightMode = PlayerPrefs.GetInt("lightMode") == 1;
        showMS = PlayerPrefs.GetInt("showMS") == 1;
        countDown = PlayerPrefs.GetInt("countDown") == 1;

        UpdateColorMode();
        msText.text = showMS == true ? "Hide milliseconds" : "Show milliseconds";
        countDirText.text = countDown == true ? "Count Up" : "Count Down";
        progress.maxValue = maxTime;
        anim = GetComponent<Animation>();
        mins.text = "0";
        secs.text = "20";
    }

    void Update()
    {
        if (!i)
        {
            if (!pause)
            {
                if (!countDown)
                {
                    if (seconds < maxTime)
                    {
                        seconds += Time.deltaTime;
                    }
                    else
                    {
                        seconds = 0;
                        reps += 1;
                    }
                }
                else
                {
                    if (seconds > 0)
                    {
                        seconds -= Time.deltaTime;
                    }
                    else
                    {
                        seconds = maxTime;
                        reps += 1;
                    }
                }
            }
        }
        if (showMS)
        {
            Textseconds.text = Mathf.Abs(Mathf.FloorToInt(seconds / 60)).ToString() + ":" + Mathf.Abs((float)Math.Round(seconds % 60, 1)).ToString();
        }
        else
        {
            Textseconds.text = Mathf.Abs(Mathf.FloorToInt(seconds / 60)).ToString() + ":" + Mathf.Abs(Mathf.FloorToInt(seconds % 60)).ToString();
        }
        Textreps.text = "Rep " + reps.ToString();

        progress.value = seconds;
    }

    public void Reset()
    {
        StartCoroutine(countdown());
    }
    public void UpdatePP()
    {
        if (int.TryParse(mins.text, out minInt) && int.TryParse(secs.text, out secInt))
        {
            if(minInt == 0 && secInt == 0)
            {
                secInt = Mathf.Clamp(secInt, 1, 59);
            }
            else
            {
                minInt = Mathf.Clamp(minInt, 0, 59);
                secInt = Mathf.Clamp(secInt, 0, 59);
            }
            mins.text = minInt.ToString();
            secs.text = secInt.ToString();
            maxTime = minInt * 60 + secInt;
        }
        else
        {
            minInt = 0;
            secInt = 1;
            maxTime = minInt * 60 + secInt;
        }

        progress.maxValue = maxTime; 
        Reset();
    }
    #region
    public void MinUp()
    {
        if(int.TryParse(mins.text, out minInt))
        {
            mins.text = (minInt + 1).ToString();
        }
        else
        {
            mins.text = "1";
        }
        UpdatePP();
    }

    public void MinDown()
    {
        if (int.TryParse(mins.text, out minInt))
        {
            mins.text = (minInt - 1).ToString();
        }
        else
        {
            mins.text = "1";
        }
        UpdatePP();
    }

    public void SecUp()
    {
        if (int.TryParse(secs.text, out secInt))
        {
            secs.text = (secInt + 1).ToString();
        }
        else
        {
            secs.text = "1";
        }
        UpdatePP();
    }

    public void SecDown()
    {
        if (int.TryParse(secs.text, out secInt))
        {
            secs.text = (secInt - 1).ToString();
        }
        else
        {
            secs.text = "1";
        }
        UpdatePP();
    }

    #endregion
    public void SettingsMenu()
    {
        if (i)
        {
            anim.Play("settingsOut");
        }
        else
        {
            anim.Play("settingsIn");
        }
        i = !i;
    }

    public void ColorMode()
    {
        lightMode = !lightMode;
        PlayerPrefs.SetInt("lightMode", lightMode ? 1 : 0);
        UpdateColorMode();
    }

    public void Pause()
    {
        pause = !pause;
        pauseText.text = pause == true ? "Resume" : "Pause";
    }

    public void UpdateColorMode()
    {
        if (lightMode)
        {
            mainCam.backgroundColor = camBackgroundLight;
            resetPanel.GetComponent<Image>().color = camBackgroundDark;
            resetTime.color = textDark;
            foreach (Text textBox in colorChangeText)
            {
                textBox.color = textLight;
            }
            lightModeText.text = "Dark Mode";
        }
        else
        {
            mainCam.backgroundColor = camBackgroundDark;
            resetPanel.GetComponent<Image>().color = camBackgroundLight;
            resetTime.color = textLight;
            foreach (Text textBox in colorChangeText)
            {
                textBox.color = textDark;
            }
            lightModeText.text = "Light Mode";
        }
    }

    public void MsecMode()
    {
        showMS = !showMS;
        PlayerPrefs.SetInt("showMS", showMS ? 1 : 0);
        msText.text = showMS == true ? "Hide milliseconds" : "Show milliseconds";
    } 

    public void CountDir()
    {
        countDown = !countDown;
        PlayerPrefs.SetInt("countDown", countDown ? 1 : 0);
        countDirText.text = countDown == true ? "Count Up" : "Count Down";
        Reset();
    }

    public void About()
    {
        Application.OpenURL("https://kna27.github.io/");
    }

    public void FeedbackForm()
    {
        Application.OpenURL("mailto:krisharora27@gmail.com?subject=Feedback%20Form");
    }
    IEnumerator countdown()
    {
        if (!i)
        {
            pauseButton.interactable = false;
        }
        pause = true;
        if (!countDown)
        {
            seconds = 0;
        }
        else
        {
            seconds = maxTime;
        }
        reps = 1;
        if (!i)
        {
            resetPanel.SetActive(true);
            resetTime.text = "3...";
            yield return new WaitForSeconds(1);
            resetTime.text = "2...";
            yield return new WaitForSeconds(1);
            resetTime.text = "1...";
            yield return new WaitForSeconds(1);
            resetPanel.SetActive(false);
        }
        pause = false;
        pauseButton.interactable = true;
    }
}
