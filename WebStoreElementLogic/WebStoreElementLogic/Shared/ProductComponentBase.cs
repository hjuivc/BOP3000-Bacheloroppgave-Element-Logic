using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using WebStoreElementLogic.Entities;
using WebStoreElementLogic.Interfaces;

namespace WebStoreElementLogic.Shared
{
    public class ProductComponentBase : ComponentBase, ISearchableComponent
    {
        // Injected dependencies
        [Inject]
        protected IProductService ProductService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        // Product related variables
        public int SelectedIndex = -1;
        public List<Product> Products = new List<Product>();

        // Product related methods
        public virtual void SelectProduct(Product product)
        {
            //AddNew = false;
            SelectedIndex = FilteredProducts.IndexOf(product);
        }

        // Search related variables
        public string searchTerm;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                searchTerm = value;
                FilterProducts();
            }
        }

        public int totalPages;
        public int totalRecords;
        public int curPage;
        public int pagerSize;
        public int pageSize;
        public int startPage;
        public int endPage;
        public string sortColumnName = "Id";
        public string sortDirection = "ASC";
        public bool isSortedAscending;
        public string activeSortColumn;
        public List<Product> FilteredProducts = new List<Product>();

        // Search related methods
        public async Task RefreshRecords(int currentPage)
        {
            var products = await ProductService.ListAllRefresh(SearchTerm, currentPage, pageSize, sortColumnName, sortDirection);
            Products = products;
        }

        public void FilterProducts()
        {
            SelectedIndex = -1;

            Console.WriteLine("FilterProducts called");
            endPage = 0;
            Console.WriteLine($"SearchTerm = {SearchTerm}");
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // If the search term is empty or null, show all products
                FilteredProducts = Products;
            }
            else
            {
                // Otherwise, filter the products based on the search term
                FilteredProducts = Products.Where(p => p.Name.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            Console.WriteLine($"FilteredProducts = {FilteredProducts.Count}");
        }

        public int GetPageIndexFromQueryString()
        {
            var uri = new Uri(NavigationManager.Uri);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query);
            var pageIndex = 0;
            if (queryParameters.ContainsKey("pageIndex"))
            {
                int.TryParse(queryParameters["pageIndex"], out pageIndex);
            }
            return pageIndex;
        }

        public void SetPagerSize(string direction)
        {
            if (direction == "forward" && endPage < totalPages)
            {
                startPage = endPage + 1;
                if (endPage + pagerSize < totalPages)
                {
                    endPage = startPage + pagerSize - 1;
                }
                else
                {
                    endPage = totalPages;
                }
                this.StateHasChanged();
            }
            else if (direction == "back" && startPage > 1)
            {
                endPage = startPage - 1;
                startPage = startPage - pagerSize;
            }
            else
            {
                startPage = 1;
                endPage = totalPages;
            }
        }

    }
}
