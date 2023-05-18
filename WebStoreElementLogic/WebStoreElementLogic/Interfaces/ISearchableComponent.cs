using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface ISearchableComponent
    {
        int GetPageIndexFromQueryString();

        void FilterProducts();

        void SetPagerSize(string direction);
    }
}
