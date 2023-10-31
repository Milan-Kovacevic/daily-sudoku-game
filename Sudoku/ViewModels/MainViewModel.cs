using CommunityToolkit.Mvvm.ComponentModel;
using Sudoku.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    public partial class MainViewModel : ObservableObject, IViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            currentViewModel = _navigationStore.CurrentViewModel;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChangedAction;
        }

        [ObservableProperty]
        private IViewModelBase currentViewModel;

        private void OnCurrentViewModelChangedAction()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
        }

        public void Dispose()
        {
            CurrentViewModel?.Dispose();
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChangedAction;
        }
    }
}
