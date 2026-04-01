using UnityEngine;

namespace Core
{
    namespace Menu
    {
        public class TestMenu : MonoBehaviour
        {
            private PageController _pageController;

#if UNITY_EDITOR

            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    _pageController.TurnPageOn(PageType.Loading);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    _pageController.TurnPageOff(PageType.Loading);
                }
                if (Input.GetKeyUp(KeyCode.H))
                {
                    _pageController.TurnPageOff(PageType.Loading, PageType.Menu);
                }
                if (Input.GetKeyUp(KeyCode.J))
                {
                    _pageController.TurnPageOff(PageType.Loading, PageType.Menu, true);
                }
            }

#endif
        }
    }
}


