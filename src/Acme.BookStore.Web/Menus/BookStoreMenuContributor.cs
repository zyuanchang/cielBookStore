﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Acme.BookStore.Localization;
using Acme.BookStore.MultiTenancy;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Acme.BookStore.Permissions;

namespace Acme.BookStore.Web.Menus
{
    public class BookStoreMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            if (!MultiTenancyConsts.IsEnabled)
            {
                var administration = context.Menu.GetAdministration();
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            var l = context.GetLocalizer<BookStoreResource>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem(BookStoreMenus.Home, l["Menu:Home"], "~/"));

            var bookStoreMenu = new ApplicationMenuItem(
                    "BookStore",
                    l["Menu:BookStore"],
                    icon: "fa fa-book"
                );
            context.Menu.AddItem(bookStoreMenu);
            
            if (await context.IsGrantedAsync(BookStorePermissions.Books.Default))
            {
                bookStoreMenu.AddItem(
                    new ApplicationMenuItem(
                        "BookStore.Books",
                        l["Menu:Books"],
                        url: "/Books"
                        ));
            }
            //context.Menu.AddItem(
            //    new ApplicationMenuItem(
            //        "BooksStore",
            //        l["Menu:BookStore"],
            //        icon: "fa fa-book"
            //    ).AddItem(
            //        new ApplicationMenuItem(
            //            "BooksStore.Books",
            //            l["Menu:Books"],
            //            url: "/Books"
            //        )
            //    )
            //);
        }
    }
}
