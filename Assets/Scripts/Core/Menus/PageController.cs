using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController Instance;

            [SerializeField] private bool _debug;
            public bool MyDebug { get { return _debug; } set { _debug = value; } }

            [SerializeField] private PageType _entryPage;
            public PageType EntryPage { get { return _entryPage; } set { _entryPage = value; } }

            [SerializeField] private List<Page> _pages;
            //public List<Page> Pages { get { return _entryPage; } set { _entryPage = value; } }

            private Hashtable m_Pages;

            #region Unity Functions

            private void Awake()
            {
                if (!Instance)
                {
                    Instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    if (_entryPage != PageType.None)
                    {
                        TurnPageOn(_entryPage);
                    }

                    foreach(Page page in _pages)
                    {
                        if (page.StartOn)
                        {
                            TurnPageOn(page.Type);
                        }
                        else if (page.Type != _entryPage)
                        {
                            page.gameObject.SetActive(false);
                        }
                    }
                }
            }

            #endregion

            #region Public Functions

            public void TurnPageOn(PageType type)
            {
                if (type == PageType.None)
                {
                    return;
                }

                if (!PageExists(type)) 
                {
                    LogWarning("You have decided to turn a page [" + type + "] that has not been registered !!!");
                    return;
                }

                Page page = GetPage(type);
                page.gameObject.SetActive(true);
                page.Animate(true);
            }

            public void TurnPageOff(PageType off, PageType on = PageType.None, bool waitForExit = false, bool animate = true)
            {
                if (off == PageType.None)
                {
                    return;
                }

                if (!PageExists(off))
                {
                    LogWarning("You have decided to turn a page off [" + off + "] that has not been registered !!!");
                    return;
                }

                Page offPage = GetPage(off);
                if (offPage.gameObject.activeSelf)
                {
                    offPage.Animate(false, animate);
                }

                if (on == PageType.None)
                {
                    return;
                }

                Page onPage = GetPage(on);
                if (waitForExit)
                {
                    StopCoroutine(nameof(WaitForPageExit)); // May want to leave it out, lets check later
                    StartCoroutine(WaitForPageExit(onPage, offPage));
                }
                else
                {
                    TurnPageOn(on);
                }
            }

            public void TurnAllPagesOff()
            {
                foreach(Page page in m_Pages.Values)
                {
                    if (page.gameObject.activeSelf)
                    {
                        TurnPageOff(page.Type, PageType.None, false, false);
                    }
                }
            }

            #endregion

            #region Private Functions

            private IEnumerator WaitForPageExit(Page on, Page off)
            {
                while (off.TargetState != Page.FLAG_NONE)
                {
                    yield return null;
                }

                TurnPageOn(on.Type);
            }

            private void RegisterAllPages()
            {
                foreach (Page page in _pages)
                {
                    RegisterPage(page);
                }
            }

            private void RegisterPage(Page page)
            {
                if (PageExists(page.Type))
                {
                    LogWarning("You are trying to register a page [" + page.Type + "] that has already been registered !!!! : " + page.gameObject.name);
                }

                m_Pages.Add(page.Type, page);
                page.CheckAnimatiorIntergrity();
                Log("Successfully registered new page [" + page.Type + "]");
            }

            private Page GetPage(PageType type)
            {
                if (!PageExists(type))
                {
                    LogWarning("You are trying to get a page [" + type + "] that has yet to be registered !!!!");
                    return null;
                }

                return (Page)m_Pages[type];
            }

            private bool PageExists(PageType type)
            {
                return m_Pages.ContainsKey(type);
            }

            private void Log(string msg) 
            {
                if (!_debug)
                {
                    return;
                }
                Debug.Log("[PageController] : " + msg);
            }

            private void LogWarning(string msg) 
            {
                if (!_debug)
                {
                    return;
                }
                Debug.LogWarning("[PageController] : " + msg);
            }
            #endregion
        }
    }
}