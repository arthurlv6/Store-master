using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.Client.Components
{
    public class ShopComponentBase : ComponentBase
    {
        //[Inject]
        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public string UserEmail { get; set; }
        [Inject]
        public GlobalMessage GlobalMsg { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            //UserEmail = authState.User.Identity.Name;
            GlobalMsg.SetMessage();
        }
    }
}
