// =============================================================
// ALTERAÇÃO:
// Importamos ObservableCollection para permitir que a interface
// acompanhe automaticamente as alterações na coleção.
// =============================================================

using System.Collections.ObjectModel;

using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // =========================================================
    // ALTERAÇÃO:
    // Coleção que contém TODOS os produtos vindos do SQLite.
    // =========================================================

    private ObservableCollection<Produto> produtos =
        new ObservableCollection<Produto>();


    // =========================================================
    // ALTERAÇÃO:
    // Coleção utilizada pela CollectionView.
    //
    // Essa coleção recebe somente os produtos que correspondem
    // ao texto digitado no SearchBar.
    // =========================================================

    private ObservableCollection<Produto> produtosFiltrados =
        new ObservableCollection<Produto>();


    // =========================================================
    // CONSTRUTOR
    // =========================================================

    public ListaProduto()
    {
        InitializeComponent();

        // =====================================================
        // ALTERAÇÃO:
        // A CollectionView passa a utilizar a coleção filtrada.
        // =====================================================

        collectionViewProdutos.ItemsSource =
            produtosFiltrados;
    }


    // =========================================================
    // QUANDO A PÁGINA APARECE
    // =========================================================

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarProdutos();
    }


    // =========================================================
    // CARREGAR PRODUTOS DO SQLITE
    // =========================================================

    private async Task CarregarProdutos()
    {
        try
        {
            // Busca todos os produtos no SQLite.
            List<Produto> lista =
                await App.Db.GetAll();


            // Limpa a coleção atual.
            produtos.Clear();


            // Adiciona os produtos vindos do banco.
            foreach (Produto produto in lista)
            {
                produtos.Add(produto);
            }


            // =================================================
            // ALTERAÇÃO:
            // Atualiza a lista que aparece na tela.
            // =================================================

            AtualizarLista();


            // Mostra a quantidade total de produtos cadastrados.
            lblQuantidadeProdutos.Text =
                produtos.Count.ToString();
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


    // =========================================================
    // ALTERAÇÃO:
    // FILTRAGEM DA LISTA
    // =========================================================

    private void AtualizarLista()
    {
        // Obtém o texto digitado no SearchBar.
        string pesquisa =
            searchBarProdutos?.Text?.Trim() ?? "";


        // Limpa a coleção que está sendo mostrada.
        produtosFiltrados.Clear();


        // Percorre todos os produtos armazenados.
        foreach (Produto produto in produtos)
        {
            // =================================================
            // ALTERAÇÃO:
            //
            // Se o campo de pesquisa estiver vazio,
            // mostramos todos os produtos.
            //
            // Caso contrário, verificamos se a descrição
            // contém o texto pesquisado.
            // =================================================

            if (string.IsNullOrWhiteSpace(pesquisa) ||
                produto.Descricao.Contains(
                    pesquisa,
                    StringComparison.OrdinalIgnoreCase))
            {
                produtosFiltrados.Add(produto);
            }
        }
    }


    // =========================================================
    // ALTERAÇÃO:
    // EVENTO TEXTCHANGED DO SEARCHBAR
    // =========================================================

    private void SearchBar_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        // Este método é executado automaticamente
        // sempre que o usuário altera o texto da pesquisa.

        AtualizarLista();
    }


    // =========================================================
    // BOTÃO ADICIONAR
    // =========================================================

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


    // =========================================================
    // BOTÃO SOMAR
    // =========================================================

    private async void ToolbarItem_Somar(
        object sender,
        EventArgs e)
    {
        try
        {
            // Busca os produtos diretamente do banco.
            List<Produto> produtos =
                await App.Db.GetAll();


            // Calcula:
            //
            // Quantidade × Preço
            //
            // para cada produto.

            double total = produtos.Sum(
                p => p.Quantidade * p.Preco
            );


            // O TOTAL aparece somente quando o usuário
            // clica no botão Somar.

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


    // =========================================================
    // MENU DE CONTEXTO DO PRODUTO
    // =========================================================

    private async void Produto_Tapped(
        object sender,
        TappedEventArgs e)
    {
        try
        {
            if (sender is Grid grid &&
                grid.BindingContext is Produto produto)
            {
                // =================================================
                // Ao tocar no produto, abre o menu.
                // =================================================

                string opcao = await DisplayActionSheet(
                    produto.Descricao,
                    "Cancelar",
                    null,
                    "Editar",
                    "Excluir");


                // =================================================
                // EDITAR
                // =================================================

                if (opcao == "Editar")
                {
                    await Navigation.PushAsync(
                        new Views.EditarProduto(produto));
                }


                // =================================================
                // EXCLUIR
                // =================================================

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


    // =========================================================
    // EXCLUIR PRODUTO
    // =========================================================

    private async Task ExcluirProduto(
        Produto produto)
    {
        // Confirmação antes da exclusão.
        bool confirmar = await DisplayAlert(
            "Excluir produto",
            $"Deseja excluir o produto:\n\n" +
            $"{produto.Descricao}\n\n" +
            $"Código: {produto.Id}",
            "Sim",
            "Não");


        // Se o usuário escolher Não,
        // não fazemos nada.
        if (!confirmar)
            return;


        // =====================================================
        // O Id é utilizado como código do produto.
        // =====================================================

        int resultado =
            await App.Db.Delete(produto.Id);


        if (resultado > 0)
        {
            await DisplayAlert(
                "Sucesso",
                "Produto excluído.",
                "OK");


            // =================================================
            // ALTERAÇÃO:
            // Recarrega os produtos depois da exclusão.
            // =================================================

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