using ServiceReference1;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace redeservice_questions
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonQ1Q2_Click(object sender, EventArgs e)
        {
            groupBox_intro.Visible = false;
            groupBoxQ1Q2.Visible = true;
            groupBoxQ3Q4Q5.Visible = false;
            groupBoxQ6.Visible = false;
            groupBoxQ7.Visible = false;
            groupBoxQ8.Visible = false;
        }

        private void buttonQ3Q4Q5_Click(object sender, EventArgs e)
        {
            groupBox_intro.Visible = false;
            groupBoxQ1Q2.Visible = false;
            groupBoxQ3Q4Q5.Visible = true;
            groupBoxQ6.Visible = false;
            groupBoxQ7.Visible = false;
            groupBoxQ8.Visible = false;
        }

        private void buttonQ6_Click(object sender, EventArgs e)
        {
            groupBox_intro.Visible = false;
            groupBoxQ1Q2.Visible = false;
            groupBoxQ3Q4Q5.Visible = false;
            groupBoxQ6.Visible = true;
            groupBoxQ7.Visible = false;
            groupBoxQ8.Visible = false;
        }

        private void buttonQ7_Click(object sender, EventArgs e)
        {
            groupBox_intro.Visible = false;
            groupBoxQ1Q2.Visible = false;
            groupBoxQ3Q4Q5.Visible = false;
            groupBoxQ6.Visible = false;
            groupBoxQ7.Visible = true;
            groupBoxQ8.Visible = false;
        }

        private void buttonQ8_Click(object sender, EventArgs e)
        {
            groupBox_intro.Visible = false;
            groupBoxQ1Q2.Visible = false;
            groupBoxQ3Q4Q5.Visible = false;
            groupBoxQ6.Visible = false;
            groupBoxQ7.Visible = false;
            groupBoxQ8.Visible = true;
        }

        private void button_addListQ1Q2_Click(object sender, EventArgs e)
        {
            string numeroDigitado = textBoxQ1Q2.Text;

            if (!string.IsNullOrWhiteSpace(numeroDigitado) && int.TryParse(numeroDigitado, out int numero))
            {
                // Adiciona o número ao ListBox
                listBoxQ1Q2.Items.Add(numero);

                // Limpa o TextBox após adicionar o número
                label_flashMessage_Q1Q2.Text = "";
                textBoxQ1Q2.Clear();
            }
            else
            {
                label_flashMessage_Q1Q2.ForeColor = Color.Red;
                label_flashMessage_Q1Q2.Text = "Digite um número válido.";
                MessageBox.Show("Digite um número válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_recordListQ1Q2_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("numeros_ordenar.txt"))
            {
                foreach (var item in listBoxQ1Q2.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }

            label_flashMessage_Q1Q2.ForeColor = Color.Green;
            label_flashMessage_Q1Q2.Text = "Arquivo salvo com sucesso!";
            MessageBox.Show("Arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_openFileQ1Q2_Click(object sender, EventArgs e)
        {
            // Caminho completo para o arquivo
            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "numeros_ordenar.txt");

            if (File.Exists(caminhoArquivo))
            {
                // Abrindo o arquivo com o aplicativo padrão associado a arquivos .txt
                Process.Start("notepad.exe", caminhoArquivo);
            }
            else
            {
                MessageBox.Show("O arquivo não existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class clsTeste
        {
            public int Codigo { get; set; }
            public string? Descricao { get; set; }
        }

        private void button_createJson_Q3Q4Q5_Click(object sender, EventArgs e)
        {
            List<clsTeste> lista = new List<clsTeste>();

            for (int i = 1; i <= 100; i++)
            {
                lista.Add(new clsTeste
                {
                    Codigo = i,
                    Descricao = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")
                });
            }

            string jsonString = System.Text.Json.JsonSerializer.Serialize(lista);

            File.WriteAllText("data.json", jsonString);
            label_flashMessage_Q3Q4Q5.ForeColor = Color.Green;
            label_flashMessage_Q3Q4Q5.Text = "Arquivo criado com sucesso!";
        }

        private void button_loadGrid_Q3Q4Q5_Click(object sender, EventArgs e)
        {
            string caminhoArquivo = "data.json";

            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText("data.json");
                List<clsTeste> lista = System.Text.Json.JsonSerializer.Deserialize<List<clsTeste>>(json);
                dataGridView_Q3Q4Q5.DataSource = lista;
                label_flashMessage_Q3Q4Q5.ForeColor = Color.Green;
                label_flashMessage_Q3Q4Q5.Text = "Lista carregada com sucesso!";
            }
            else
            {
                label_flashMessage_Q3Q4Q5.ForeColor = Color.Red;
                label_flashMessage_Q3Q4Q5.Text = "O arquivo 'data.json' não existe.";
                MessageBox.Show("O arquivo 'data.json' não existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_getCep_Q6_Click(object sender, EventArgs e)
        {
            string cep = textBox_getCep_Q6.Text;
            label_flashMessage_Q6.ForeColor = Color.YellowGreen;
            label_flashMessage_Q6.Text = "Consultando..";

            if (!string.IsNullOrWhiteSpace(cep))
            {
                using (var ws = new AtendeClienteClient())
                {
                    try
                    {
                        // usando consultaCEPAsync ao invés do Método: consultaCEP
                        var endereco = ws.consultaCEPAsync(cep);
                        label_viewCep_Q6.Text = $"Bairro: {endereco.Result.@return.bairro}," +
                            $" Endereço: {endereco.Result.@return.end}," +
                            $" CEP: {endereco.Result.@return.cep}," +
                            $" Cidade: {endereco.Result.@return.cidade}," +
                            $" UF: {endereco.Result.@return.uf}";
                        label_flashMessage_Q6.ForeColor = Color.Green;
                        label_flashMessage_Q6.Text = "Cep Encontrado!";


                    }
                    catch (System.Exception ex)
                    {
                        label_flashMessage_Q6.ForeColor = Color.Red;
                        label_flashMessage_Q6.Text = "Cep Não Encontrado!";
                        MessageBox.Show($"Erro ao consultar o CEP: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                label_flashMessage_Q6.ForeColor = Color.Red;
                label_flashMessage_Q6.Text = "Em branco!";
                MessageBox.Show($"Em branco! Digite um valor Correto!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class Banco
        {
            public string? ispb { get; set; }
            public string? name { get; set; }
            public int? code { get; set; }
            public string? fullName { get; set; }
        }

        private async void button_getBanks_Q7_Click(object sender, EventArgs e)
        {
            label_flashMessage_Q7.ForeColor = Color.YellowGreen;
            label_flashMessage_Q7.Text = "Consultando Dados..";
            try
            {
                string apiUrl = "https://brasilapi.com.br/api/banks/v1";

                // Criando um HttpClient para fazer a solicitação HTTP
                using (HttpClient httpClient = new HttpClient())
                {
                    // Solicitação GET para a URL da API
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Desserializando o JSON em uma lista de objetos
                        var bancos = JsonConvert.DeserializeObject<Banco[]>(jsonContent);

                        dataGridView_showBanks_Q7.DataSource = bancos;
                        label_flashMessage_Q7.ForeColor = Color.Green;
                        label_flashMessage_Q7.Text = "Consulta na API com sucesso!";
                    }
                    else
                    {
                        label_flashMessage_Q7.ForeColor = Color.Red;
                        label_flashMessage_Q7.Text = "Falha ao obter dados da API de bancos.";
                        MessageBox.Show("Falha ao obter dados da API de bancos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (System.Exception ex)
            {
                label_flashMessage_Q7.ForeColor = Color.Red;
                label_flashMessage_Q7.Text = "Erro ao consultar a API";
                MessageBox.Show($"Erro ao consultar a API: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_getImg_Q8_Click(object sender, EventArgs e)
        {
            label_flashMessage_Q8.ForeColor = Color.YellowGreen;
            label_flashMessage_Q8.Text = "Baixando...";
            try
            {
                string urlImagem = "https://redeservice.com.br/wp-content/uploads/2020/07/redeservice-logo.png";

                string nomeArquivo = "logo.png";

                // WebClient para baixar a imagem
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(urlImagem, nomeArquivo);
                }

                // Carregando a imagem a partir do arquivo local
                Image imagem = Image.FromFile(nomeArquivo);

                string base64Imagem = ConvertImageToBase64(imagem);

                pictureBox_Q8.Image = imagem;

                textBox_show64_Q8.Text = base64Imagem;
                label_flashMessage_Q8.ForeColor = Color.Green;
                label_flashMessage_Q8.Text = "Download e exibição da imagem com Suceso!";
            }
            catch (System.Exception ex)
            {
                label_flashMessage_Q8.ForeColor = Color.YellowGreen;
                label_flashMessage_Q8.Text = "Erro ao baixar e exibir a imagem";
                MessageBox.Show($"Erro ao baixar e exibir a imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertImageToBase64(Image imagem)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Salvando a imagem em formato Base64 na memória
                imagem.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
    }
}

