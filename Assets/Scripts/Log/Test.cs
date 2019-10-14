using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Log
{
    public class Test : MonoBehaviour
    {
        public GameObject obj;
        public string str = "123";

        private void Start()
        {
            var page = new Page
            {
                Title = "111",
                PageID = 123
            };

            var args = new Dictionary<int ,string>() {[100]="123",[200] = "2003"};
//            var args = new Queue<int>() { };
//            args.Enqueue(1);
//            args.Enqueue(3);
//            args.Enqueue(5);
//            Debug.Log(LogWin.GetObejctInfo(args));
//
//            var v3 = new Vector3(0,1,2);


            Debug.Log(args.GetType().IsGenericParameter);
            var gens = args.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

//            var fields = args.GetType().GetMembers(BindingFlags.Instance  | BindingFlags.Public |BindingFlags.FlattenHierarchy);
//            Debug.Log(args.GetType().GetProperties());

//            var pros =  v3.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
//


            Debug.Log(LogWin.GetObejctInfo(args));

//            
//            
//            Debug.Log(LogWin.GetObejctInfo(v3));
        }

        [Serializable]
        public class Page
        {
            private uint num;
            public uint PageID;
            public string Title;
        }
    }
}