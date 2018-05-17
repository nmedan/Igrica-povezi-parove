using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGameExample.Commands;
using System.Windows.Input;
using MemoryGameExample.Model;
using System.Data.Entity;
namespace MemoryGameExample.ViewModels
{
    public class EnterHighscoreViewModel : ViewModelBase
    {
        EnterHighscore enterHighscore;
        Highscores highscore;
        #region constructor
        public EnterHighscoreViewModel(EnterHighscore EnterHighscore, Highscores Highscore)
        {
            enterHighscore = EnterHighscore;
            highscore = Highscore;
            enterHighscore.EnterName.Text = highscore.Name;
            enterHighscore.EnterName.SelectAll();
            enterHighscore.EnterName.Focus();
        }
        #endregion

        #region commands
        private ICommand saveHighscore;
        public ICommand SaveHighscore
        {
            get
            {
                if (saveHighscore == null)
                {
                    saveHighscore = new RelayCommand(param => SaveHighscoreExecute(), param => CanSaveHighscoreExecute());
                }
                return saveHighscore;
            }
        }

        public void SaveHighscoreExecute()
        {
            highscore.Name = enterHighscore.EnterName.Text;
            using (HighscoresContext db = new HighscoresContext())
            {

                db.Entry(highscore).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            enterHighscore.Close();
            HighscoreDialog hd = new HighscoreDialog();
            hd.ShowDialog();
        }

        public bool CanSaveHighscoreExecute()
        {
            if (String.IsNullOrEmpty(enterHighscore.EnterName.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
#endregion