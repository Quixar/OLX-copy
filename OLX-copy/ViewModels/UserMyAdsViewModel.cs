using OLX_copy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.ViewModels
{
    public class UserMyAdsViewModel
    {
        private readonly CurrentUserService _currentUserService;

        public UserMyAdsViewModel(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
    }
}
