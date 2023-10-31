using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public class ThemeChangeService
    {
        private readonly SelectedThemeStore _selectedThemeStore;

        public ThemeChangeService(SelectedThemeStore selectedThemeStore)
        {
            _selectedThemeStore = selectedThemeStore;
        }

        public void SetLightTheme()
        {
            _selectedThemeStore.CurrentTheme = Theme.Light;
            ApplyCurrentTheme();
        }

        public void SetDarkTheme()
        {
            _selectedThemeStore.CurrentTheme = Theme.Dark;
            ApplyCurrentTheme();
        }

        public void ApplyCurrentTheme()
        {
            if (_selectedThemeStore.CurrentTheme == null)
                return;
            PaletteHelper paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(_selectedThemeStore.CurrentTheme);
            paletteHelper.SetTheme(currentTheme);
        }
    }
}
