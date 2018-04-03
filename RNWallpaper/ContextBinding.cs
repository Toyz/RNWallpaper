using System.ComponentModel;
using System.Runtime.CompilerServices;
using RNWallpaper.Annotations;

namespace RNWallpaper
{
    public class ContextBinding : INotifyPropertyChanged
    {
        #region Purity Filters
            private bool _puritySfw = true;
            public bool PuritySfw
            {
                get => _puritySfw;
                set
                {
                    _puritySfw = value;
                    OnPropertyChanged();
                }
            }

            private bool _puritySketchy;
            public bool PuritySketchy
            {
                get => _puritySketchy;
                set
                {
                    _puritySketchy = value;
                    OnPropertyChanged();
                }
            }
        #endregion

        #region  Catergoeis
        private bool _catAnime = true;
        public bool CatAnime
        {
            get => _catAnime;
            set
            {
                _catAnime = value;
                OnPropertyChanged();
            }
        }

        private bool _catPeople =  true;
        public bool CatPeople
        {
            get => _catPeople;
            set
            {
                _catPeople = value;
                OnPropertyChanged();
            }
        }

        private bool _catGeneral = true;
        public bool CatGeneral
        {
            get => _catGeneral;
            set
            {
                _catGeneral = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
