using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Services;
using Sudoku.Stores;

namespace Sudoku.ViewModels
{
    public interface IViewModelBase
    {
        /// <summary>
        /// Provides a way individual view models can unsubscribe from certain events,<br/> so it effectively doesn't cause memory leak.<br/>
        /// It is called after each individual view change by <see cref="NavigationService"/> to clean up memory.<br/>
        /// Call to the Dispose method is happening inside <seealso cref="NavigationStore"/>.<br/>
        /// Nested (child) view models won't be disposed automatically, so it is the responsibility of the parent view model to do so.
        /// </summary>
        void Dispose() { }
    }
}
