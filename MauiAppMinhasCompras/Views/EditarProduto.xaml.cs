using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    // Produto que está sendo editado.
    private Produto _produto;


    public EditarProduto(Produto produto)
    {
        InitializeComponent();

        // Recebe o produto selecionado na lista.
        _produto = produto;


        // Mostra o código do produto.
        lbl_codigo.Text =
            $"Código: {_produto.Id}";


        // Preenche os campos com os dados atuais.
        txt_descricao.Text =
            _produto.Descricao;

        txt_quantidade.Text =
            _produto.Quantidade.ToString();

        txt_preco.Text =
            _produto.Preco.ToString();
    }


    // =========================================================
    // SALVAR ALTERAÇÕES
    // =========================================================

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            // Validação da descrição.
            if (string.IsNullOrWhiteSpace(
                txt_descricao.Text))
            {
                await DisplayAlert(
                    "Atenção",
                    "Informe a descrição do produto.",
                    "OK");

                return;
            }


            // Validação da quantidade.
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


            // Validação do preço.
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


            // Atualiza os dados do objeto.
            _produto.Descricao =
                txt_descricao.Text.Trim();

            _produto.Quantidade =
                quantidade;

            _produto.Preco =
                preco;


            // =================================================
            // ALTERAÇÃO:
            // Atualiza o registro no SQLite.
            // =================================================

            await App.Db.Update(_produto);


            await DisplayAlert(
                "Sucesso!",
                "Produto atualizado com sucesso.",
                "OK");


            // Volta para a lista.
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