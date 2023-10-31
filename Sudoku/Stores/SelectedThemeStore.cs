using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Stores
{
    public class SelectedThemeStore
    {
        private IBaseTheme? _currentTheme;
        public IBaseTheme? CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                CurrentThemeChanged?.Invoke();
            }
        }

        public event Action? CurrentThemeChanged;

        public string ThemeString { get => CurrentTheme == Theme.Light ? "Light" : "Dark"; }
        public bool IsDark { get => CurrentTheme == Theme.Dark; }

        public void LoadTheme()
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            CurrentTheme = currentTheme.GetBaseTheme().GetBaseTheme();
        }
    }
}
