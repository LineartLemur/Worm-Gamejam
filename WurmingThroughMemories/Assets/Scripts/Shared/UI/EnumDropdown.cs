using System;
using System.Linq;
using PepijnWillekens.ManagerSystem;
using TMPro;
using UniRx;

namespace Utility  {
    public class EnumDropdown<T> : AutoInit {

        public TMP_Dropdown dropdown;
        
        [NonSerialized]
        public ReactiveProperty<T> valueProperty = new ReactiveProperty<T>();
        
        protected override void OnInit() {
            base.OnInit();
            RefreshOptions();
            valueProperty.Subscribe(e => {
                dropdown.value = EnumLists.GetAllValues<T>().IndexOf(e);
            }).AddTo(this);
            dropdown.onValueChanged.AddListener((i) => {
                valueProperty.Value = EnumLists.GetAllValues<T>()[i];
            });
        }

        private void RefreshOptions() {
            
            dropdown.ClearOptions();
            dropdown.AddOptions(EnumLists.GetAllValues<T>().Select(e=>e.ToString()).ToList());
            valueProperty.Value = EnumLists.GetAllValues<T>()[0];
        }

        public bool HasValue() {
            return dropdown.value >= 0 && dropdown.options.Count > 0;
        }
        
        public T GetSelected() {
            return valueProperty.Value;
        }

    }
}