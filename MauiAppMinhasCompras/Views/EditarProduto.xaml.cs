using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    private Produto _produto;

    public EditarProduto(Produto produto)
    {
        InitializeComponent();

        _produto = produto;

        lbl_codigo.Text = $"Código: {_produto.Id}";

        txt_descricao.Text = _produto.Descricao;
        txt_quantidade.Text = _produto.Quantidade.ToString();
        txt_preco.Text = _produto.Preco.ToString();
    }

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert(
                    "Atenção",
                    "Informe a descrição do produto.",
                    "OK");

                return;
            }

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

            _produto.Descricao = txt_descricao.Text.Trim();
            _produto.Quantidade = quantidade;
            _produto.Preco = preco;

            await App.Db.Update(_produto);

            await DisplayAlert(
                "Sucesso!",
                "Produto atualizado com sucesso.",
                "OK");

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