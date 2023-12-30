using System.Net.Http.Json;
using System.Text.Json;

namespace womansweb.Pages
{
    public partial class Products
    {
        private IEnumerable<Productdto> productlist = Array.Empty<Productdto>();
        private string Api_Url = "https://localhost:7291/";
        private string searchtext = "";

        Productdto newProduct = new Productdto
        {
            ProductID = "",
            ProductName = "",
            NetPrice = "",
            SellingPrice = "",
            EnteredTime = null,
            LastupdatedTime = null,
            Quantity = ""
        };

        private bool IsmodalOpen = false;

        // Method to show the Add Product modal
        private async Task ShowAddProductModal()
        {
            IsmodalOpen = true;
        }

        private async Task CloseAddProductModal()
        {
            IsmodalOpen = false;
        }

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(Api_Url + "api/product/getproducts" );
            httpClient.Dispose();
            using var responseStream = await response.Content.ReadAsStreamAsync();
            productlist = await JsonSerializer.DeserializeAsync<IEnumerable<Productdto>>(responseStream);
        }

        private async Task InsertProduct()
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                newProduct.EnteredTime = DateTime.Now;
                newProduct.LastupdatedTime = DateTime.Now;

                HttpResponseMessage responses = await httpClient.PostAsJsonAsync($"{Api_Url}api/product/InsertProduct", newProduct);

                if (responses.IsSuccessStatusCode)
                {
                    // Product inserted successfully
                    // You can perform any additional actions here after successful insertion
                    // For example, refreshing the product list
                    await RefreshProductList();
                    httpClient.Dispose();
                    newProduct.ProductID = "";
                    newProduct.ProductName = "";
                    newProduct.NetPrice = "";
                    newProduct.SellingPrice = "";
                    newProduct.EnteredTime = null;
                    newProduct.LastupdatedTime = null;
                    newProduct.Quantity = "";
                }
                else
                {
                    // Handle unsuccessful insertion
                    // You may want to display an error message or perform other actions
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async Task RefreshProductList()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(Api_Url + "api/product/getproducts?search="+searchtext);
            httpClient.Dispose();
            using var responseStream = await response.Content.ReadAsStreamAsync();
            productlist = await JsonSerializer.DeserializeAsync<IEnumerable<Productdto>>(responseStream);
        }
    }
}
