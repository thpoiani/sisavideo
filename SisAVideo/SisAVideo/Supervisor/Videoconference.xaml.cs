using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;
using SisAVideo.Google;

namespace SisAVideo.Supervisor
{
    public partial class Videoconference : MetroWindow
    {
        private DatabaseConnection connection;
        private LoginRole usuario;
        private Videoconferencia videoconferencia;
        private Dictionary<string, string> conferencistas;
        private List<string> polos;

        private string targetUpdate;
        private int verificador_target;

        private List<string> polosUpdate;
        private int verificador_polos;

        private Dictionary<string, string> conferencistasUpdate;
        private int verificador_conferencista;

        private int contador_erros;

        public Videoconference()
        {
            InitializeComponent();
        }

        private void tabCadastrar_Loaded(object sender, RoutedEventArgs e)
        {
            videoconferencia = new Videoconferencia();
            conferencistas = new Dictionary<string, string>();
            polos = new List<string>();
            
            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql_pk = "SELECT COUNT(*) + 1 FROM vc.videoconferencia";

                codigoBox.Text = connection.ReturnString(sql_pk);
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void cadastrarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            contador_erros = 0;

            try
            {
                videoconferencia.Titulo = tituloBox.Text;
                lblTitulo.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblTitulo.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Codigo = Int32.Parse(codigoBox.Text);
                lblCodigo.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblCodigo.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Data_ativacao = dataPicker.Text;
                lblData.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblData.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Horario_inicio = horaInicioBox.Text;
                lblHoraInicio.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblHoraInicio.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Horario_fim = horaFimBox.Text;
                lblHoraFim.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblHoraFim.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Solicitante = solicitanteBox.Text;
                lblSolicitante.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSolicitante.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Secretaria = secretariaBox.Text;
                lblSecretaria.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblSecretaria.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Orgao = OrgaoBox.Text;
                lblOrgao.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblOrgao.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Streaming = streamingBox.Text;
                lblStreaming.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblStreaming.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                videoconferencia.Estudio = estudioBox.Text;
                lblEstudio.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblEstudio.Foreground = Brushes.Red;
                contador_erros++;
            }

            if (contador_erros != 0)
            {
                Mouse.OverrideCursor = null;
                return;
            }

            try
            {
                usuario = new LoginRole();
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                string sql_insert = "EXEC vc.cadastrar_videoconferencia "
                                  + "  '" + videoconferencia.Titulo
                                  + "', " + videoconferencia.Codigo
                                  + ", '" + videoconferencia.Data_ativacao
                                  + "', '" + videoconferencia.Horario_inicio
                                  + "', '" + videoconferencia.Horario_fim
                                  + "', '" + videoconferencia.Solicitante
                                  + "', '" + videoconferencia.Secretaria
                                  + "', '" + videoconferencia.Orgao
                                  + "', '" + videoconferencia.Streaming
                                  + "', '" + videoconferencia.Estudio
                                  + "'; ";

                if (!string.IsNullOrEmpty(videoconferencia.Target))
                {
                    sql_insert += "INSERT INTO vc.publicoalvo (codigo, publico) "
                    + "VALUES (" +videoconferencia.Codigo+ ", '" +videoconferencia.Target+ "'); ";
                }

                foreach (KeyValuePair<string, string> conferencista in conferencistas)
                {
                    sql_insert += "INSERT INTO vc.conferencista (videoconferencia, cpf, nome) VALUES "
                                + "(" + videoconferencia.Codigo
                                + ", '" + conferencista.Key
                                + "', '" + conferencista.Value
                                + "'); ";
                }

                foreach (string polo in polos)
                {
                    sql_insert += "INSERT INTO vc.ativado (video, polo) VALUES "
                                + "(" + videoconferencia.Codigo
                                + ", (SELECT id FROM vc.polo WHERE nome = '" + polo
                                + "')); ";
                }
                
                connection.SQL(sql_insert);

                CalendarioInsert(connection.ReturnString("SELECT CONVERT(varchar(10), data_ativacao, 103) FROM vc.videoconferencia WHERE codigo = " + videoconferencia.Codigo + ""));
            }
            catch (SqlException)
            {
                Mouse.OverrideCursor = null;
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();

            Mouse.OverrideCursor = null;
            Xceed.Wpf.Toolkit.MessageBox.Show("Videoconferência cadastrada.", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);

            tituloBox.Text = "";
            codigoBox.Text = "";
            dataPicker.Text = "";
            horaInicioBox.Text = "";
            horaFimBox.Text = "";
            solicitanteBox.Text = "";
            secretariaBox.Text = "";
            OrgaoBox.Text = "";
            streamingBox.Text = "";
            estudioBox.Text = "";

            tabCadastrar_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);            
            tabVisualizar_Loaded(null, null);
            tabAtivacao_Loaded(null, null);
        }

        private void CalendarioInsert(string data)
        {
            Calendario calendar = new Calendario(ConfigurationManager.AppSettings["username"],
                                                 ConfigurationManager.AppSettings["password"]);

            calendar.GoogleCalendar();

            calendar.Event(videoconferencia.Titulo, data, videoconferencia.Horario_inicio, videoconferencia.Horario_fim);

            calendar.Insert();
        }

        private void tabAtualizar_Enable()
        {
            tituloBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(tituloBoxAtualizar, 0);
            Panel.SetZIndex(lblOverTitulo, 1);

            dataPickerAtualizar.IsEnabled = false;
            Panel.SetZIndex(dataPickerAtualizar, 0);
            Panel.SetZIndex(lblOverData, 1);

            horaInicioBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(horaInicioBoxAtualizar, 0);
            Panel.SetZIndex(lblOverHoraInicio, 1);

            horaFimBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(horaFimBoxAtualizar, 0);
            Panel.SetZIndex(lblOverHoraFim, 1);

            solicitanteBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(solicitanteBoxAtualizar, 0);
            Panel.SetZIndex(lbOverSolicitante, 1);

            secretariaBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(secretariaBoxAtualizar, 0);
            Panel.SetZIndex(lblOverSecretaria, 1);

            orgaoBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(orgaoBoxAtualizar, 0);
            Panel.SetZIndex(lvlOverOrgao, 1);

            streamingBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(streamingBoxAtualizar, 0);
            Panel.SetZIndex(lblOverStreaming, 1);

            estudioBoxAtualizar.IsEnabled = false;
            Panel.SetZIndex(estudioBoxAtualizar, 0);
            Panel.SetZIndex(lblOverEstudio, 1);
        }

        private void lblOverTitulo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            tituloBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(tituloBoxAtualizar, 1);
            Panel.SetZIndex(lblOverTitulo, 0);
        }

        private void lblOverData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dataPickerAtualizar.IsEnabled = true;
            Panel.SetZIndex(dataPickerAtualizar, 1);
            Panel.SetZIndex(lblOverData, 0);
        }

        private void lblOverHoraInicio_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            horaInicioBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(horaInicioBoxAtualizar, 1);
            Panel.SetZIndex(lblOverHoraInicio, 0);
        }

        private void lblOverHoraFim_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            horaFimBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(horaFimBoxAtualizar, 1);
            Panel.SetZIndex(lblOverHoraFim, 0);
        }

        private void lbOverSolicitante_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            solicitanteBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(solicitanteBoxAtualizar, 1);
            Panel.SetZIndex(lbOverSolicitante, 0);
        }

        private void lblOverSecretaria_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            secretariaBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(secretariaBoxAtualizar, 1);
            Panel.SetZIndex(lblOverSecretaria, 0);
        }

        private void lvlOverOrgao_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            orgaoBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(orgaoBoxAtualizar, 1);
            Panel.SetZIndex(lvlOverOrgao, 0);
        }

        private void lblOverStreaming_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            streamingBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(streamingBoxAtualizar, 1);
            Panel.SetZIndex(lblOverStreaming, 0);
        }

        private void lblOverEstudio_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            estudioBoxAtualizar.IsEnabled = true;
            Panel.SetZIndex(estudioBoxAtualizar, 1);
            Panel.SetZIndex(lblOverEstudio, 0);
        }

        private void tituloBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (tituloBoxAtualizar.IsEnabled == true)
            {
                tituloBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(tituloBoxAtualizar, 0);
                Panel.SetZIndex(lblOverTitulo, 1);
            }
        }

        private void dataPickerAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataPickerAtualizar.IsEnabled == true)
            {
                dataPickerAtualizar.IsEnabled = false;
                Panel.SetZIndex(dataPickerAtualizar, 0);
                Panel.SetZIndex(lblOverData, 1);
            }
        }

        private void horaInicioBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (horaInicioBoxAtualizar.IsEnabled == true)
            {
                horaInicioBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(horaInicioBoxAtualizar, 0);
                Panel.SetZIndex(lblOverHoraInicio, 1);
            }
        }

        private void horaFimBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (horaFimBoxAtualizar.IsEnabled == true)
            {
                horaFimBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(horaFimBoxAtualizar, 0);
                Panel.SetZIndex(lblOverHoraFim, 1);
            }
        }

        private void solicitanteBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (solicitanteBoxAtualizar.IsEnabled == true)
            {
                solicitanteBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(solicitanteBoxAtualizar, 0);
                Panel.SetZIndex(lbOverSolicitante, 1);
            }
        }

        private void secretariaBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (secretariaBoxAtualizar.IsEnabled == true)
            {
                secretariaBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(secretariaBoxAtualizar, 0);
                Panel.SetZIndex(lblOverSecretaria, 1);
            }
        }

        private void orgaoBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (orgaoBoxAtualizar.IsEnabled == true)
            {
                orgaoBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(orgaoBoxAtualizar, 0);
                Panel.SetZIndex(lvlOverOrgao, 1);
            }
        }

        private void streamingBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (streamingBoxAtualizar.IsEnabled == true)
            {
                streamingBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(streamingBoxAtualizar, 0);
                Panel.SetZIndex(lblOverStreaming, 1);
            }
        }

        private void estudioBoxAtualizar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (estudioBoxAtualizar.IsEnabled == true)
            {
                estudioBoxAtualizar.IsEnabled = false;
                Panel.SetZIndex(estudioBoxAtualizar, 0);
                Panel.SetZIndex(lblOverEstudio, 1);
            }
        }


        private void tabAtualizar_Loaded(object sender, RoutedEventArgs e)
        {
            tabAtualizar_Enable();

            verificador_target = 0;
            verificador_polos = 0;
            verificador_conferencista = 0;

            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT codigo FROM vc.videoconferencia ORDER BY codigo DESC");

                List <string> videoconferencias = new List<string>();

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    videoconferencias.Add(linha["codigo"].ToString());

                codigoBoxAtualizar.ItemsSource = videoconferencias;
            }
            catch
            {
                return;
            }
            connection.Close();
        }

        private void codigoBoxAtualizar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tabAtualizar_Enable();

            usuario = new LoginRole();
            try
            {
                int videoconferencia_cod = Int32.Parse(codigoBoxAtualizar.SelectedValue.ToString());

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("EXEC vc.visualizar_videoconferencia_especifica " + videoconferencia_cod+ "");

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                {
                    tituloBoxAtualizar.Text = linha["titulo"].ToString();
                    dataPickerAtualizar.Text = linha["data_ativacao"].ToString();
                    horaInicioBoxAtualizar.Text = linha["horario_inicio"].ToString();
                    horaFimBoxAtualizar.Text = linha["horario_fim"].ToString();
                    solicitanteBoxAtualizar.Text = linha["solicitante"].ToString();
                    secretariaBoxAtualizar.Text = linha["secretaria"].ToString();
                    orgaoBoxAtualizar.Text = linha["orgao"].ToString();
                    streamingBoxAtualizar.Text = linha["streaming"].ToString();
                    estudioBoxAtualizar.Text = linha["estudio"].ToString();
                }

                targetUpdate = connection.ReturnString("SELECT p.publico FROM vc.videoconferencia v INNER JOIN vc.publicoalvo p ON v.codigo = p.codigo WHERE v.codigo = " + videoconferencia_cod + ";");

                polosUpdate = new List<string>();

                connection.SQL("SELECT p.nome FROM vc.videoconferencia v INNER JOIN vc.ativado a ON v.codigo = a.video INNER JOIN vc.polo p ON a.polo = p.id WHERE v.codigo = " + videoconferencia_cod + ";");

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    polosUpdate.Add(linha["nome"].ToString());


                conferencistasUpdate = new Dictionary<string, string>();

                connection.SQL("SELECT c.cpf, c.nome FROM vc.videoconferencia v INNER JOIN vc.conferencista c ON v.codigo = c.videoconferencia WHERE v.codigo = " + videoconferencia_cod + "");

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    conferencistasUpdate.Add(linha["cpf"].ToString(), linha["nome"].ToString());
            }
            catch
            {
                return;
            }
            connection.Close();
        }

        private void atualizarSendButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string titulo = null;
            string data_ativacao = null;
            string horario_inicio = null;
            string horario_fim = null;

            videoconferencia = new Videoconferencia();
            contador_erros = 0;
            int contador_mudancas = 0;

            string sql_update = null;

            int Codigo = 0;

            try
            {
                Codigo = Int32.Parse(codigoBoxAtualizar.SelectedValue.ToString());
                lblCodigoAtualizar.Foreground = Brushes.Black;
            }
            catch
            {
                lblCodigoAtualizar.Foreground = Brushes.Red;
                Mouse.OverrideCursor = null;
                return;
            }

            if (tituloBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Titulo = tituloBoxAtualizar.Text;
                    lblTituloAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET titulo = '" + videoconferencia.Titulo + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblTituloAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (dataPickerAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Data_ativacao = dataPickerAtualizar.Text;
                    lblDataAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET data_ativacao = '" + videoconferencia.Data_ativacao + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblDataAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (horaInicioBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Horario_inicio = horaInicioBoxAtualizar.Text;
                    lblHoraInicioAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET horario_inicio = '" + videoconferencia.Horario_inicio + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblHoraInicioAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (horaFimBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Horario_fim = horaFimBoxAtualizar.Text;
                    lblHoraFimAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET horario_fim = '" + videoconferencia.Horario_fim + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblHoraFimAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (solicitanteBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Solicitante = solicitanteBoxAtualizar.Text;
                    lblSolicitanteAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET solicitante = '" + videoconferencia.Solicitante + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblSolicitanteAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (secretariaBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Secretaria = secretariaBoxAtualizar.Text;
                    lblSecretariaAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET secretaria = '" + videoconferencia.Secretaria + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblSecretariaAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (orgaoBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Orgao = orgaoBoxAtualizar.Text;
                    lblOrgaoAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET orgao = '" + videoconferencia.Orgao + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblOrgaoAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (streamingBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Streaming = streamingBoxAtualizar.Text;
                    lblStreamingAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET streaming = '" + videoconferencia.Streaming + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblStreamingAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }
            
            if (estudioBoxAtualizar.IsEnabled == true)
            {
                try
                {
                    videoconferencia.Estudio = estudioBoxAtualizar.Text;
                    lblEstudioAtualizar.Foreground = Brushes.Black;
                    sql_update += "UPDATE vc.videoconferencia "
                                + "SET estudio = '" + videoconferencia.Estudio + "' " 
                                + "WHERE codigo = " + Codigo + ";";
                    contador_mudancas++;
                }
                catch (Exception)
                {
                    lblEstudioAtualizar.Foreground = Brushes.Red;
                    contador_erros++;
                }
            }

            if (contador_erros != 0)
            {
                sql_update = null;
                Mouse.OverrideCursor = null;
                return;
            }

            if (verificador_target == 1)
            {
                sql_update += "UPDATE vc.publicoalvo "
                            + "SET publico = '" + targetUpdate + "' "
                            + "WHERE codigo = " + Codigo + ";";
                contador_mudancas++;
            }

            if (verificador_conferencista == 1)
            {
                sql_update += "DELETE FROM vc.conferencista WHERE videoconferencia = " + Codigo + ";";

                foreach (KeyValuePair<string, string> conferencista in conferencistasUpdate)
                {
                    sql_update += "INSERT INTO vc.conferencista (videoconferencia, cpf, nome) VALUES "
                                + "(" + Codigo
                                + ", '" + conferencista.Key
                                + "', '" + conferencista.Value
                                + "'); ";
                }

                contador_mudancas++;
            }

            if (verificador_polos == 1)
            {
                sql_update += "DELETE FROM vc.ativado WHERE video = " + Codigo + ";";

                foreach (string polo in polosUpdate)
                {   
                    sql_update += "INSERT INTO vc.ativado (video, polo) VALUES "
                                + "(" + Codigo
                                + ", (SELECT id FROM vc.polo WHERE nome = '" + polo
                                + "')); ";
                }

                contador_mudancas++;
            }

            if (contador_mudancas == 0)
            {
                Mouse.OverrideCursor = null;
                Xceed.Wpf.Toolkit.MessageBox.Show("Nenhuma atualização registrada.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                titulo = connection.ReturnString("SELECT titulo FROM vc.videoconferencia WHERE codigo = " + Codigo + "");

                data_ativacao = connection.ReturnString("SELECT convert(varchar(10),data_ativacao,103) FROM vc.videoconferencia WHERE codigo = " + Codigo + "");

                horario_inicio = connection.ReturnString("SELECT horario_inicio FROM vc.videoconferencia WHERE codigo = " + Codigo + "");

                horario_fim = connection.ReturnString("SELECT horario_fim FROM vc.videoconferencia WHERE codigo = " + Codigo + "");

                connection.SQL(sql_update);
            }
            catch (SqlException)
            {
                Mouse.OverrideCursor = null;
                erro();
                Application.Current.Shutdown();
            }

            connection.Close();

            try
            {
                CalendarioUpdate(titulo, data_ativacao, horario_inicio, horario_fim);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }

            Mouse.OverrideCursor = null;
            Xceed.Wpf.Toolkit.MessageBox.Show("Videoconferencia atualizada.", "Atualização", MessageBoxButton.OK, MessageBoxImage.Information);

            tabCadastrar_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabVisualizar_Loaded(null, null);
            tabAtivacao_Loaded(null, null);
        }

        private void CalendarioUpdate(string titulo, string data_ativacao, string horario_inicio, string horario_fim)
        {
            
            string _titulo = null;
            string _data_ativacao = null;
            string _horario_inicio = null;
            string _horario_fim = null;
            

            Calendario calendar = new Calendario(ConfigurationManager.AppSettings["username"],
                                                 ConfigurationManager.AppSettings["password"]);

            calendar.GoogleCalendar();
            calendar.Event(titulo, data_ativacao, horario_inicio, horario_fim);
            calendar.Delete();

            
            if (string.IsNullOrEmpty(videoconferencia.Titulo))
                _titulo = titulo;
            else
                _titulo = videoconferencia.Titulo;

            if (string.IsNullOrEmpty(videoconferencia.Data_ativacao))
                _data_ativacao = data_ativacao;
            else
                _data_ativacao = videoconferencia.Data_ativacao;

            if (string.IsNullOrEmpty(videoconferencia.Horario_inicio))
                _horario_inicio = horario_inicio;
            else
                _horario_inicio = videoconferencia.Horario_inicio;

            if (string.IsNullOrEmpty(videoconferencia.Horario_fim))
                _horario_fim = horario_fim;
            else
                _horario_fim = videoconferencia.Horario_fim;

            Calendario calendario = new Calendario(ConfigurationManager.AppSettings["username"],
                                                   ConfigurationManager.AppSettings["password"]);

            calendario.GoogleCalendar();

            calendario.Event(_titulo, _data_ativacao, _horario_inicio, _horario_fim);
            calendario.Insert();
            
        }

        private void tabRemover_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT codigo FROM vc.videoconferencia ORDER BY codigo DESC");

                List<string> videoconferencias = new List<string>();
                
                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    videoconferencias.Add(linha["codigo"].ToString());

                tituloBoxRemover.Text = null;
                codigoComboBox.ItemsSource = videoconferencias;
            }
            catch (Exception)
            {
                return;
            }
            connection.Close();
        }

        private void codigoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                int videoconferencia = Int32.Parse(codigoComboBox.SelectedValue.ToString());

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT titulo FROM vc.videoconferencia WHERE codigo = "+videoconferencia+"");

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    tituloBoxRemover.Text = linha["titulo"].ToString();
            }
            catch
            {
                return;
            }
            connection.Close();
        }

        private void removerSendButton_Click(object sender, RoutedEventArgs e)
        {
            string titulo = null;
            string data_ativacao = null;
            string horario_inicio = null;
            string horario_fim = null;

            try
            {
                int videoconferencia = Int32.Parse(codigoComboBox.SelectedValue.ToString());
                
                if (Xceed.Wpf.Toolkit.MessageBox.Show("Deseja remover a videoconferencia \n"
                    + tituloBoxRemover.Text + " (" + videoconferencia + ") ?", "Remoção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    connection = new DatabaseConnection(usuario.Username, usuario.Password);
                    connection.Open();

                    string sql_delete = "DELETE FROM vc.videoconferencia "
                                      + "WHERE codigo = " + videoconferencia + "";

                    titulo = connection.ReturnString("SELECT titulo FROM vc.videoconferencia WHERE codigo = " + videoconferencia + "");

                    data_ativacao = connection.ReturnString("SELECT convert(varchar(10), data_ativacao, 103) FROM vc.videoconferencia WHERE codigo = "+videoconferencia + "");

                    horario_inicio = connection.ReturnString("SELECT horario_inicio FROM vc.videoconferencia WHERE codigo = "+videoconferencia + "");

                    horario_fim = connection.ReturnString("SELECT horario_fim FROM vc.videoconferencia WHERE codigo = " + videoconferencia + "");

                    CalendarioDelete(titulo, data_ativacao, horario_inicio, horario_fim);

                    connection.SQL(sql_delete);

                    Mouse.OverrideCursor = null;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Videoconferência removida com sucesso.");
                }
            }
            catch
            {
                return;
            }

            connection.Close();
            
            tabCadastrar_Loaded(null, null);
            tabAtualizar_Loaded(null, null);
            tabRemover_Loaded(null, null);
            tabVisualizar_Loaded(null, null);
            tabAtivacao_Loaded(null, null);            
        }

        private void CalendarioDelete(string titulo, string dataAtivacao, string horaInicio, string horaFim)
        {
            Calendario calendar = new Calendario(ConfigurationManager.AppSettings["username"],
                                                 ConfigurationManager.AppSettings["password"]);

            calendar.GoogleCalendar();

            calendar.Event(titulo, dataAtivacao, horaInicio, horaFim);

            calendar.Delete();
        }

        private void tabVisualizar_Loaded(object sender, RoutedEventArgs e)
        {
            string email = ConfigurationManager.AppSettings["username"];

            string html = "<html><head><style> html {overflow: auto;}  "
                        + "body {overflow: hidden;} </style></head><body>"
			            + "<iframe src=\"https://www.google.com/calendar/embed?"
                        + "showTitle=0&amp;showNav=0&amp;showDate=0&amp;showPrint=0&amp;"
			            + "showTabs=0&amp;showCalendars=0&amp;showTz=0&amp;"
                        + "height=370&amp;wkst=1&amp;hl=pt_BR&amp;bgcolor=%23FFFFFF&amp;"
			            + "src=" + email + "&amp;"
			            + "color=%232952A3&amp;color=%232F6309&amp;"
                        + "ctz=America%2FSao_Paulo\" style=\" border-width:0 \""
			            + "width=\"580\" height=\"370\" frameborder=\"0\" scrolling=\"no\">"
                        + "</iframe></body></html>";

            browser.NavigateToString(html);
        }
        
        private void publicoalvo_Click(object sender, RoutedEventArgs e)
        {            
            Target publicoalvo = new Target(videoconferencia.Target);

            if (publicoalvo.ShowDialog() == true)
                videoconferencia.Target = publicoalvo.retorno;
        }

        private void conferencista_Click(object sender, RoutedEventArgs e)
        {
            Lecture lecture = new Lecture(conferencistas);

            if (lecture.ShowDialog() == true)
                conferencistas = lecture.conferencistas;
        }

        private void poloativo_Click(object sender, RoutedEventArgs e)
        {
            ActivatedPolo activatedpolo = new ActivatedPolo(polos);

            if (activatedpolo.ShowDialog() == true)
                polos = activatedpolo.polos;
        }

        private void publicoalvoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string videoconferencia_cod = codigoBoxAtualizar.SelectedValue.ToString();

                Target publicoalvo = new Target(targetUpdate);

                if (publicoalvo.ShowDialog() == true)
                {
                    targetUpdate = publicoalvo.retorno;
                    verificador_target = 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void conferencistaAtualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string videoconferencia_cod = codigoBoxAtualizar.SelectedValue.ToString();

                Lecture lecture = new Lecture(conferencistasUpdate);

                if (lecture.ShowDialog() == true)
                {
                    conferencistasUpdate = lecture.conferencistas;
                    verificador_conferencista = 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void poloativoAtualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string videoconferencia_cod = codigoBoxAtualizar.SelectedValue.ToString();

                ActivatedPolo activatedpolo = new ActivatedPolo(polosUpdate);

                if (activatedpolo.ShowDialog() == true)
                {
                    polosUpdate = activatedpolo.polos;
                    verificador_polos = 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void tabAtivacao_Loaded(object sender, RoutedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT codigo FROM vc.videoconferencia WHERE data_ativacao >= CURRENT_TIMESTAMP ORDER BY codigo DESC");

                List<string> videoconferencias = new List<string>();
                
                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    videoconferencias.Add(linha["codigo"].ToString());

                tituloBoxAtivacao.Text = null;
                codigoComboBoxAtivacao.ItemsSource = videoconferencias;
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            connection.Close();
        }

        private void codigoComboBoxAtivacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuario = new LoginRole();
            try
            {
                int videoconferencia = Int32.Parse(codigoComboBoxAtivacao.SelectedValue.ToString());

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT titulo FROM vc.videoconferencia WHERE codigo = " +videoconferencia+ "");

                foreach (DataRow linha in connection.View("videoconferencia").Tables["videoconferencia"].Rows)
                    tituloBoxAtivacao.Text = linha["titulo"].ToString();

                connection.SQL("SELECT p.nome from vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo INNER JOIN vc.polo p ON a.polo = p.id WHERE v.codigo = " + videoconferencia + "");

                List<string> polos = new List<string>();

                foreach (DataRow linha in connection.View("ativado").Tables["ativado"].Rows)
                    polos.Add(linha["nome"].ToString());

                polosAtivacao.ItemsSource = polos;
            }
            catch
            {

            }
            connection.Close();
        }

        private void ativacaoSendButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Email();
        }


        private void Email()
        {
            List<string> emails;
            int videoconferencia = 0;
            string titulo = null;
            string solicitante = null;
            string secretaria = null;
            string data = null;
            string orgao = null;
            string publicoalvo = null;
            List<string> conferencistas;
            string conferencista = null;
            string streaming = null;
            string streaming_check = null;
            List<string> polos;
            string polo = null;
            string horario_inicio = null;
            string horario_fim = null;
            string estudio = null;

            try
            {
                videoconferencia = Int32.Parse(codigoComboBoxAtivacao.SelectedValue.ToString());

                connection = new DatabaseConnection(usuario.Username, usuario.Password);
                connection.Open();

                connection.SQL("SELECT p.email FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo INNER JOIN vc.polo p ON a.polo = p.id WHERE v.codigo = " + videoconferencia + "");

                emails = new List<string>();

                foreach (DataRow linha in connection.View("ativado").Tables["ativado"].Rows)
                    emails.Add(linha["email"].ToString());

                titulo = connection.ReturnString ("SELECT v.titulo FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = " + videoconferencia +"");

                solicitante = connection.ReturnString ("SELECT v.solicitante FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                secretaria = connection.ReturnString ("SELECT v.secretaria FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                orgao = connection.ReturnString ("SELECT v.orgao FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                publicoalvo = connection.ReturnString ("SELECT p.publico FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo INNER JOIN vc.publicoalvo p ON v.codigo = p.codigo WHERE v.codigo = "+videoconferencia+"");

                connection.SQL("SELECT c.nome as conferencista FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo INNER JOIN vc.conferencista c ON v.codigo = c.videoconferencia WHERE v.codigo = "+videoconferencia+"");

                conferencistas = new List<string>();

                foreach (DataRow linha in connection.View("ativado").Tables["ativado"].Rows)
                    conferencistas.Add(linha["conferencista"].ToString());

                streaming = connection.ReturnString("SELECT v.streaming FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                connection.SQL("SELECT p.nome as polo from vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo INNER JOIN vc.polo p ON a.polo = p.id WHERE v.codigo = " + videoconferencia + "");

                polos = new List<string>();

                foreach (DataRow linha in connection.View("ativado").Tables["ativado"].Rows)
                    polos.Add(linha["polo"].ToString());

                data = connection.ReturnString ("SELECT convert(varchar(10), v.data_ativacao, 103) FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                horario_inicio = connection.ReturnString ("SELECT convert(varchar(5), v.horario_inicio, 108) FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                horario_fim = connection.ReturnString ("SELECT convert(varchar(5), v.horario_fim, 108) FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");

                estudio = connection.ReturnString("SELECT v.estudio FROM vc.ativado a INNER JOIN vc.videoconferencia v ON a.video = v.codigo WHERE v.codigo = "+videoconferencia+"");
            }
            catch
            {
                return;
            }
            
            connection.Close();

            foreach (string conferencistan in conferencistas)
            {
                conferencista += "<p>" + conferencistan + "</p>";
            }

            foreach (string polon in polos)
            {
                polo += polon + "<br>";
            }

            if (!string.IsNullOrEmpty(streaming))
                streaming_check = "<p>Sim </p><h4 style=\"color:#119EDA\"></h4><p>"+streaming+"</p>";
            else
                streaming_check = "<p>Nao </p><h4 style=\"color:#119EDA\"></h4>";
                
            MailMessage email = new MailMessage();
            email.From = new MailAddress(ConfigurationManager.AppSettings["username"]);
            
            foreach (string emailn in emails)
                email.To.Add(new MailAddress(emailn));

            email.Subject = "SisAVideo - Ativação de VC nº" +videoconferencia+ " - " +titulo+ " - " +data;

            email.Body =
    "<html><head></head><body> "
  + "<table width=\"580\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"> "
  + "<tbody><tr><td align=\"center\"> "
  + "<img src=\"https://dl.dropbox.com/u/49252928/header_email_script_comunicado.png\"> "
  + "</td></tr><tr><td style=\"padding:15px\"> "
  + "<h1>Script - Ativação</h1>"
  + "<h5>Código: "
  + "<span>" + videoconferencia + "</span></h5> "
  + "<h2>Solicitação de Recurso</h2> "
  + "<h4 style=\"color:#119EDA\">Título do Evento</h4> "
  + "<p>" + titulo + "</p> "
  + "<h4 style=\"color:#119EDA\">Solicitante</h4> "
  + "<p>" + solicitante + "</p> "
  + "<h4 style=\"color:#119EDA\">Secretaria</h4> "
  + "<p>" + secretaria + "</p>"
  + "<h4 style=\"color:#119EDA\">Órgão</h4>"
  + "<p>" + orgao + "</p>"
  + "<h4 style=\"color:#119EDA\">Público-Alvo</h4>"
  + "<p>" + publicoalvo + "</p>"
  + "<h2>Solicitação - Uso de Polo de Recepção de Videoconferência</h2>"
  + "<h4 style=\"color:#119EDA\">Título da videoconferência</h4>"
  + "<p>" + titulo + "</p>"
  + "<h4 style=\"color:#119EDA\">Videoconferencista(s)</h4>"
  + conferencista
  + "<h4 style=\"color:#119EDA\">A videoconferência será transmitida por streaming?</h4>"
  + streaming_check
  + "<h4 style=\"color:#119EDA\">Polos Ativados</h4><p>"
  + polo
  + "</p><h4 style=\"color:#119EDA\">Agenda</h4> "
  + "<table><tbody> "
  + "<tr style=\"border:1px solid #666\"><th style=\"border:1px solid #666;font-size:12px\">Data</th> "
  + "<th style=\"border:1px solid #666;font-size:12px\">Horário</th></tr> "
  + "<tr><td style=\"border:1px solid #666;text-align:center;font-size:12px\"> "
  + data
  + "</td><td style=\"border:1px solid #666;text-align:center;font-size:12px\">"
  + horario_inicio + " às " + horario_fim
  + "</td></tr></tbody></table>"
  + "<h4 style=\"color:#119EDA\">Local de Geração</h4>"
  + "<p>" + estudio + "</p>"
  + "</td></tr><tr><td>"
  + "<img src=\"http://controles.escolasdegoverno.sp.gov.br/ativacao/App_Themes/Default/imagens/rodape_email_script_comunicado.jpg\">"
  + "</td></tr></tbody></table></body></html>";

            email.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["username"],
                                                     ConfigurationManager.AppSettings["password"]);

            try
            {
                smtp.Send(email);
            }
            catch (SmtpException ex)
            {
                Mouse.OverrideCursor = null;
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Mouse.OverrideCursor = null;
            Xceed.Wpf.Toolkit.MessageBox.Show("Email de Ativação de Videoconferência encaminhado aos polos ativos.", "Ativação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void erro()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                                            + "Entre em contato com o desenvolvedor.", "Erro",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
