using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICredits : MonoBehaviour
{
    [Header("Class objects")]
    [SerializeField] TextMeshProUGUI plainTextPref;
    [SerializeField] Button textButtonPref;
    [SerializeField] Transform container;
    [SerializeField] Button backButton;
    [SerializeField] UIPopup mainMenuPopup;

    [Space(10)]
    [Header("List of developers")]
    [SerializeField] List<Category> categories = new List<Category>();

    UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();
        backButton.onClick.AddListener(ToMainMenu);
    }

    private void Start() => CreateInfoToShow();

    private void ToMainMenu()
    {
        popup.Hide();
        mainMenuPopup.Show();
    }

    private void CreateInfoToShow()
    {
        if(categories.Count > 0)
        {
            foreach(var category in categories)
            {
                var tempCategory = Instantiate(plainTextPref, container);
                tempCategory.text = category.name;

                foreach(var person in category.people)
                {
                    var textButton = Instantiate(textButtonPref, container);
                    textButton.GetComponent<TextMeshProUGUI>().text = $"<u>{person.name}</u>";
                    textButton.onClick.AddListener(() => OpenUrl(person.url));
                }
            }
        }
    }

    private void OpenUrl(string url) => Application.OpenURL(url);

}

[System.Serializable]
public struct Category
{
    public string name;
    public List<Person> people;
}

[System.Serializable]
public struct Person
{
    public string name;
    public string url;
}