using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ApiHandler : MonoBehaviour
{
    public const string QUERY_HEAD = "http://www.omdbapi.com/?apikey=6367da97&s=";
    public const string QUERY_EXTRA_INFO_HEAD = "http://www.omdbapi.com/?apikey=6367da97&i=";

    public InputField inputField;

    public GameObject thumbnailPrefab;

    public Transform thumbnailParent;

    public Text resultStatusText;

    public ExtraInfoPanel extraInfoPanel;

    public void ShowResult()
    {
        string keyword  = inputField.text;

        if (string.IsNullOrEmpty(keyword))
        {
            Debug.Log("no keyword entered.");
            return;
        }
        //in case twice the button is pressed.
        StopCoroutine(GetResult(string.Empty));
        StartCoroutine(GetResult(QUERY_HEAD + keyword));
    }

    IEnumerator GetResult(string query)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(query))

        {
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log("Data not fetched : " + webRequest.error);
                resultStatusText.text = "Data not fetched : " + webRequest.error;
            }
            else
            {
                Movies movies = JsonUtility.FromJson<Movies>(webRequest.downloadHandler.text);

                if (movies.Response)
                {
                    DeletePreviousThumbnail();
                    if (movies.Search != null && movies.Search.Length == 0)
                    {
                        Debug.Log("No Data");
                        resultStatusText.text="No Data";
                    }
                    else
                    {
                        resultStatusText.text = "Total movies : " + movies.totalResults;
                        InitThumbnails(movies);
                    }
                }
                else
                {
                    DeletePreviousThumbnail();
                    resultStatusText.text = movies.Error;
                }
            }
        }
    }

    private void DeletePreviousThumbnail()
    {
        for(int i = 0; i < thumbnailParent.childCount; i++)
        {
            Destroy(thumbnailParent.GetChild(i).gameObject);
        }
    }

    private void InitThumbnails(Movies movies)
    {
        Thumbnail thumbnail;
        GameObject thumbnailGO;

        for (int i = 0; i < movies.Search.Length; i++)
        {
             thumbnailGO=Instantiate(thumbnailPrefab, thumbnailParent);
            thumbnail=thumbnailGO.GetComponent<Thumbnail>();
            thumbnail.Init(movies.Search[i],OnThumbnailClick);
        }
    }

    private void OnThumbnailClick(string movieID)
    {
        if
            (string.IsNullOrEmpty(movieID))
        {
            return;
        }
        StopCoroutine(GetExtraMovieInfo(string.Empty));
        StartCoroutine(GetExtraMovieInfo(QUERY_EXTRA_INFO_HEAD+movieID));
    }

    IEnumerator GetExtraMovieInfo(string query)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(query))

        {
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log("Data not fetched : " + webRequest.error);
            }
            else
            {
                MovieInformation movie = JsonUtility.FromJson<MovieInformation>(webRequest.downloadHandler.text);

                if (movie==null)
                {
                    Debug.Log("No Data");
                }
                else
                {
                    this.extraInfoPanel.Show(movie);
                }
            }
        }
    }
}
