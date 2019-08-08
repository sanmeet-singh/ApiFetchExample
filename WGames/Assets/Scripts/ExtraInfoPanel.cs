using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraInfoPanel : MonoBehaviour
{
    public Text text;
    public void Show(MovieInformation movieInformation)
    {
        this.gameObject.SetActive(true);

        this.text.text = "Title : " + movieInformation.Title + "\n" + "Released : " + movieInformation.Released + "\n" +
            "Rated : " + movieInformation.Rated + "\n" + "Genre : " + movieInformation.Genre + "\n" +
            "Director : " + movieInformation.Director + "\n" + "Language : " + movieInformation.Language;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
