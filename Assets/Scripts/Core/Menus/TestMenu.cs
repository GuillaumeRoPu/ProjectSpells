using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    namespace Menu
    {
        public class TestMenu : MonoBehaviour
        {
            private PageController _pageController;

#if UNITY_EDITOR

            [SerializeField] InputActionReference _F;
            [SerializeField] InputActionReference _G;
            [SerializeField] InputActionReference _H;
            [SerializeField] InputActionReference _J;

            private void Update()
            {
                if (_F.action.triggered)
                {
                    _pageController.TurnPageOn(PageType.Loading);
                }
                if (_G.action.triggered)
                {
                    _pageController.TurnPageOff(PageType.Loading);
                }
                if (_H.action.triggered)
                {
                    _pageController.TurnPageOff(PageType.Loading, PageType.Menu);
                }
                if (_J.action.triggered)
                {
                    _pageController.TurnPageOff(PageType.Loading, PageType.Menu, true);
                }
            }

#endif
        }
    }
}


