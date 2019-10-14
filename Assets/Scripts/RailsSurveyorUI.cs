using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RailsSurveyorUI : MonoBehaviour
{
    public Button btn;
    public Dropdown dropdown;
    public InputField inputFileField;


    private Action<string> onRequest;

    private void Start()
    {
        btn.onClick.AddListener(() => { StartCoroutine(WaitReq()); });

        onRequest = s =>
        {
            var pages = JArray.Parse(s);
            var ps = JsonConvert.DeserializeObject<Page[]>(s);
            Debug.Log(ps.Length);
        };
    }
    

    private IEnumerator WaitReq()
    {
        var client = FindObjectOfType<HttpClient>();
        yield return StartCoroutine(client.GetRailList(RailsSurveyorUI.GetUrl(Urllist.PageUrl)+inputFileField.text));
        onRequest?.Invoke(client.downloadHandler.text);
    }


    [Serializable]
    public class Page
    {
        public uint PageID;
        public string Title;
    }
    
    public enum Urllist
    {
        RailsSurveyorUrl ,
        PageUrl,
    }
    
    public static List<string> url = new List<string>()
    {
        /*
         * 问卷调查请求问卷页面URL
         */
        "/user/RailsSurveyor/",
        /*
         * 问卷调查请求单个页面页面信息
         */
        "/user/RailsSurveyorPage/",
    };

    public static string GetUrl( Urllist types)
    {
        return RailsSurveyorUI.url[(int)types];
    }
}