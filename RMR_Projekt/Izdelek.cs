using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt
{
    public class Izdelek : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string src_slike { get; set; }
       public string ime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }


        public Izdelek (string slika,string ime)
        {
            this.src_slike = slika;
            this.ime = ime;
            this.IsSelected = false;
        }


    }

}
