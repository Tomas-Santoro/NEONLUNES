using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManagerMenu : MonoBehaviour
{
    [Header("Configuración de las Solapas")]
    public GameObject Menu;
    public GameObject Options;
    public GameObject Credits;
    public GameObject Exit;
    public Animator Camera;
    [Header("Barra de Carga")]
    [SerializeField]
    public GameObject loadingScreen;
    [SerializeField]
    private Image slider;
    public float loadDuration=3;
    [Header("Configuración del Delay")]
    [SerializeField]
    private float delay; //lo voy a hacer asi porque si lo hago por animacion, es mucho mas tiempo y ta me da paja
    [SerializeField]
    private float miniDelay;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Menu.activeSelf)
        {
            BackButtonUI();
        }
    }

    void Go()
    {
        Camera.SetBool("Go", true);
    }
    void GoBack()
    {
        Camera.SetBool("Go", false);
    }
    void GoExit()
    {
        Camera.SetBool("Exit", true);
    }
    public void PlayUI()
    {
        Go();
        Menu.SetActive(false);
    }
    public void OptionsUI()
    {
        Go();
        StartCoroutine(DelayMenu(Options, delay));
        Menu.SetActive(false);
    }
    public void CreditsUI()
    {
        Go();
        StartCoroutine(DelayMenu(Credits, delay));
        Menu.SetActive(false);
    }
    public void ExitUI()
    {
        StartCoroutine(DelayMenu(Exit,0));
        Menu.SetActive(false);
    }
    public void EXITUICONFIRM()
    {
        GoExit();
        Exit.SetActive(false);
        StartCoroutine(ExitGame());
    }
    public void BackButtonUI()
    {
        GoBack();
        StartCoroutine(DelayMenu(Menu, miniDelay));
        //Menu.SetActive(true);
        Options.SetActive(false);
        Credits.SetActive(false);
        Exit.SetActive(false);
    }


    //Corrutina para generar un delay
    IEnumerator DelayMenu(GameObject Ui,float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Ui.SetActive(true);
    }
    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public void LoadScene(int sceneIndex) //proximo a usar
    {
        StartCoroutine(LoadingScreenSlider(sceneIndex));
    }

    IEnumerator LoadingScreenSlider(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        float elapsed = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadDuration);

            // Simulate the load progress
            slider.fillAmount = Mathf.Clamp01(operation.progress / 0.9f) * progress;

            if (elapsed >= loadDuration && operation.progress >= 0.9f)
            {
                slider.fillAmount = 1f;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

