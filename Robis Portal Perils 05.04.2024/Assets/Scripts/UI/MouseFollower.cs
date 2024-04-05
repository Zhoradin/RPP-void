using UnityEngine;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        private Canvas canvas;
        private UIInventoryItem item;

        public void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            item = GetComponentInChildren<UIInventoryItem>();
        }

        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
            );
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool val)
        {
            gameObject.SetActive(val);
        }
    }
}
