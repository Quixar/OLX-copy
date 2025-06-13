using OLX_copy.Data;
using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.ViewModels
{
    public class UserCustomizationViewModel
    {
        private readonly CurrentUserService _currentUserService;
        public UserCustomizationViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
    }
}
