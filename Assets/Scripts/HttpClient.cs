using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour
{
    private UnityWebRequest req;


    public DownloadHandler downloadHandler => req.downloadHandler;


    public IEnumerator GetRailList(string router)
    {
        req = UnityWebRequest.Get($"http://127.0.0.1:8080/user/{router}");
        yield return req.SendWebRequest();
        Debug.Log(req.downloadHandler.text);
    }
}