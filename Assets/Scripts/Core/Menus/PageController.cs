using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController Instance;

            private bool _debug;
            public bool Debug { get { return _debug; } set { _debug = value; } }

            private PageType _entryPage;
            public PageType EntryPage { get { return _entryPage; } set { _entryPage = value; } }

            private List<Page> _pages;
            //public List<Page> Pages { get { return _entryPage; } set { _entryPage = value; } }

            private Hashtable m_Pages;

            #region Unity Functions

            private void Awake()
            {
                
            }

            #endregion

            #region Public Functions

            public void TurnPageOn(PageType type)
            {

            }

            public void TurnPageOff(PageType off, PageType on = PageType.None, bool waitForExit = false)
            {

            }

            #endregion

            #region Private Functions

            private IEnumerator WaitForPageExit(Page on, Page off)
            {
                yield return null;
            }

            private void RegisterAllPages()
            {

            }

            private void RegisterPage(Page page)
            {

            }

            private Page GetPage(PageType type)
            {
                return null;
            }

            private bool PageExists(PageType type)
            {
                return false;
            }

            private void Log(string msg) 
            { 

            }

            private void LogWarning(string msg) 
            { 
            
            }
            #endregion
        }
    }
}