using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManagementSystem.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _id = "";
        private string _name = "";
        private string _category = "Coffee";
        private decimal _price;
        private bool _isAvailable = true;
        private string _icon = "☕";

        public string Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public string Icon { get => _icon; set { _icon = value; OnPropertyChanged(); } }

        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                _isAvailable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public string StatusText => IsAvailable ? "Available" : "Out of Stock";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}