using UnityEngine;

namespace Log
{
    [CreateAssetMenu(fileName = "LogSetting",menuName = "LogSetting")]
    public class LogWinScriptObject : ScriptableObject
    {

        public LogMode logMode = LogMode.DEV;
    }


    public enum LogMode
    {
        DEV, //开发模式
        BETA, //测试环境
        PRODUCTION , //生产环境
    }
}
