using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }


    // =========================================================
    // SALVAR PRODUTO
    // =========================================================

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            // =================================================
            // VALIDAÇÃO DA DESCRIÇÃO
            // =================================================

            if (string.IsNullOrWhiteSpace(
                txt_descricao.Text))
            {
                await DisplayAlert(
                    "Atenção",
                    "Informe a descrição do produto.",
                    "OK");

                return;
            }


            // =================================================
            // VALIDAÇÃO DA QUANTIDADE
            // =================================================

            if (!double.TryParse(
                    txt_quantidade.Text,
                    out double quantidade))
            {
                await DisplayAlert(
                    "Atenção",
                    "Informe uma quantidade válida.",
                    "OK");

                return;
            }


            // =================================================
            // VALIDAÇÃO DO PREÇO
            // =================================================

            if (!double.TryParse(
                    txt_preco.Text,
                    out double preco))
            {
                await DisplayAlert(
                    "Atenção",
                    "Informe um preço válido.",
                    "OK");

                return;
            }


            // =================================================
            // VALIDAÇÃO DA QUANTIDADE
            // =================================================

            if (quantidade <= 0)
            {
                await DisplayAlert(
                    "Atenção",
                    "A quantidade deve ser maior que zero.",
                    "OK");

                return;
            }


            // =================================================
            // VALIDAÇÃO DO PREÇO
            // =================================================

            if (preco < 0)
            {
                await DisplayAlert(
                    "Atenção",
                    "O preço não pode ser negativo.",
                    "OK");

                return;
            }


            // =================================================
            // CRIAÇÃO DO OBJETO PRODUTO
            // =================================================

            Produto p = new Produto
            {
                Descricao = txt_descricao.Text.Trim(),
                Quantidade = quantidade,
                Preco = preco
            };


            // =================================================
            // INSERÇÃO NO SQLITE
            // =================================================

            await App.Db.Insert(p);


            // Mensagem de confirmação.
            await DisplayAlert(
                "Sucesso!",
                "Registro inserido com sucesso.",
                "OK");


            // =================================================
            // ALTERAÇÃO:
            // Volta para a tela principal.
            //
            // Ao voltar, o OnAppearing() da ListaProduto
            // será executado e a nova lista será carregada.
            // =================================================

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }
}