using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Thumbnail : MonoBehaviour
{
    public Image image;

    public Text text;

    private MovieInformation movieInformation;

    private Action<string> onThumbnailClick;

    public void Init(MovieInformation movieInformation,Action<string> onThumbnailClick)
    {
        this.movieInformation = movieInformation;

        this.text.text = movieInformation.Title;

        this.onThumbnailClick = onThumbnailClick;

        StartCoroutine(FetchImage(movieInformation.Poster));
    }

    private IEnumerator FetchImage(string url)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Data not fetched : " + webRequest.error);
            }
            else
            {
                Texture2D texture = new Texture2D(1,1,TextureFormat.ARGB32,false);
                texture.LoadImage(webRequest.downloadHandler.data);
                texture.Apply();
                image.sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public void OnThumbnailClick()
    {
        onThumbnailClick(movieInformation.imdbID);
    }
}
