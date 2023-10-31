using CommunityToolkit.Mvvm.ComponentModel;
using Sudoku.Stores;
using Sudoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<IViewModelBase> createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<IViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = createViewModel();
        }
    }

}
