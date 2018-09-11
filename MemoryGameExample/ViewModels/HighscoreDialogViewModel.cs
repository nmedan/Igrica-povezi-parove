using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGameExample.Model;
using System.Windows.Input;
using MemoryGameExample.Commands;

namespace MemoryGameExample.ViewModels
{
    public class HighscoreDialogViewModel : ViewModelBase
    {
        const int scoresToShow = 3;
        HighscoreDialog highscoreDialog;
        #region constructor
        public HighscoreDialogViewModel(HighscoreDialog HighscoreDialog)
        {
            highscoreDialog = HighscoreDialog;
            using (HighscoresContext db = new HighscoresContext())
            {
                HighscoresList = db.Highscores.OrderBy(x => x.Time).ThenBy(x => x.Moves).Take(3).ToList();
            }
        }
        #endregion

        #region properties
        private Highscores highscore;
        public Highscores Highscore
        {
            get
            {
                return highscore;
            }
            set
            {
                highscore = value;
                OnPropertyChanged("Highscore");
            }
        }

        private List<Highscores> highscoresList;
        public List<Highscores> HighscoresList
        {
            get
            {
                return highscoresList;
            }
            set
            {
                highscoresList = value;
                OnPropertyChanged("HighscoresList");
            }
        }
        #endregion

        #region commands
        private ICommand ok;
        public ICommand Ok
        {
            get
            {
                if (ok == null)
                {
                    ok = new RelayCommand(param => OkExecute(), param => CanOkExecute());
                }
                return ok;
            }
        }

        public void OkExecute()
        {
            highscoreDialog.Close();

        }

        public bool CanOkExecute()
        {
            return true;
        }

        private ICommand reset;
        public ICommand Reset
        {
            get
            {
                if (reset == null)
                {
                    reset = new RelayCommand(param => ResetExecute(), param => CanResetExecute());
                }
                return reset;
            }
        }

        public void ResetExecute()
        {
            using (HighscoresContext db = new HighscoresContext())
            {
                foreach (var score in db.Highscores)
                {
                    db.Highscores.Remove(score);
                }
                db.SaveChanges();
                HighscoresList = db.Highscores.ToList();
            }
            
        }

        public bool CanResetExecute()
        {
            if (HighscoresList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
#endregion