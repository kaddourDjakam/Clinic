using Clinik.Commands;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Clinik.ViewModel.Rendez_vous.PatientOptions
{
    class SelectPatientViewModel : ViewModelBase
    {
        private readonly Action<Person> personClickHandler;

        public ICommand PersonClickCommand { get; }
        public Person PersonEnst {get;}
        public PatientModel PatientEnst { get; }
        public SelectPatientViewModel(Person person, Action<Person> personClickHandler)
        {
            PersonEnst =    person;
            PatientEnst = person?.Patient;
            this.personClickHandler = personClickHandler;
            PersonClickCommand = new RelayCommand(OnPersonClick, ()=> true);
        }
        private void OnPersonClick()
        {
            personClickHandler?.Invoke(PersonEnst);
        }

    }
}
