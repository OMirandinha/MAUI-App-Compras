using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    public ListaProduto()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        try
        {
            List<Produto> produtos = await App.Db.GetAll();

            collectionViewProdutos.ItemsSource = produtos;

            lblQuantidadeProdutos.Text = produtos.Count.ToString();

           
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                "Não foi possível carregar os produtos.\n\n" +
                ex.Message,
                "OK");
        }
    }

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(
                new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    private async void ToolbarItem_Somar(
        object sender,
        EventArgs e)
    {
        try
        {
            List<Produto> produtos = await App.Db.GetAll();

            double total = produtos.Sum(
                p => p.Quantidade * p.Preco
            );

            await DisplayAlert(
                "Total da compra",
                $"Valor total: R$ {total:F2}",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    private async void Produto_Tapped(
        object sender,
        TappedEventArgs e)
    {
        try
        {
            if (sender is Grid grid &&
                grid.BindingContext is Produto produto)
            {
                string opcao = await DisplayActionSheet(
                    produto.Descricao,
                    "Cancelar",
                    null,
                    "Editar",
                    "Excluir");

                if (opcao == "Editar")
                {
                    await Navigation.PushAsync(
                        new Views.EditarProduto(produto));
                }
                else if (opcao == "Excluir")
                {
                    await ExcluirProduto(produto);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    private async Task ExcluirProduto(Produto produto)
    {
        bool confirmar = await DisplayAlert(
            "Excluir produto",
            $"Deseja excluir o produto:\n\n" +
            $"{produto.Descricao}\n\n" +
            $"Código: {produto.Id}",
            "Sim",
            "Não");

        if (!confirmar)
            return;

        int resultado = await App.Db.Delete(produto.Id);

        if (resultado > 0)
        {
            await DisplayAlert(
                "Sucesso",
                "Produto excluído.",
                "OK");

            await CarregarProdutos();
        }
        else
        {
            await DisplayAlert(
                "Ops",
                "O produto não foi encontrado.",
                "OK");
        }
    }
}