using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace SisAVideo.Intern
{
    public partial class Evaluation : MetroWindow
    {
        private ExcelConnection connection;
        private DatabaseConnection connection_db;
        private LoginRole usuario;
        private Avaliacao avaliacao;
        private int contador_erros;
        private string estagiario;
        private List<string> polos;
        private List<string> sql_update;
        private string caminho;

        public Evaluation()
        {
            InitializeComponent();
            avaliacao = new Avaliacao();
            usuario = new LoginRole();
        }

        private void Avaliacao_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection_db = new DatabaseConnection(usuario.Username, usuario.Password);
                connection_db.Open();

                connection_db.SQL("SELECT v.codigo FROM vc.videoconferencia v INNER JOIN vc.ativado a "
                              + "ON v.codigo = a.video WHERE a.polo = (SELECT e.polo FROM rh.funcionario f "
                              + "INNER JOIN rh.estagiario e ON f.id = e.id INNER JOIN rh.login l "
                              + "ON f.id = l.id WHERE l.username = '" + usuario.Username + "') "
                              + "ORDER BY v.codigo DESC;");

                polos = new List<string>();

                foreach (DataRow linha in connection_db.View("videoconferencia").Tables["videoconferencia"].Rows)
                    polos.Add(linha["codigo"].ToString());

                codigoBox.ItemsSource = polos;
                
                estagiario = connection_db.ReturnString 
                            ("SELECT nome FROM rh.funcionario f INNER JOIN rh.login l "
                           + " ON f.id = l.id WHERE l.username = '" + usuario.Username + "';");
                }
                catch (SqlException)
                {
                    erro();
                    Application.Current.Shutdown();
                }

            connection_db.Close();
        }

        private void codigoBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                connection_db = new DatabaseConnection(usuario.Username, usuario.Password);
                connection_db.Open();

                string titulo = connection_db.ReturnString("SELECT titulo FROM vc.videoconferencia "
                  + "WHERE codigo = " + codigoBox.SelectedValue.ToString() + ";");

                string publico = connection_db.ReturnString("SELECT p.publico FROM vc.publicoalvo p "
                  + "INNER JOIN vc.videoconferencia v ON v.codigo = p.codigo WHERE p.codigo = "
                  + codigoBox.SelectedValue.ToString() + ";");

                tituloBox.Text = titulo;
                participantesBox.Text = publico;
            }
            catch (SqlException)
            {
                erro();
                Application.Current.Shutdown();
            }

            connection_db.Close();
        }

        private void SelecionarArquivo()
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.DefaultExt = ".xls|.xlsx";
            openfiledialog.Filter = "Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx";

            Nullable<bool> resultado = openfiledialog.ShowDialog();

            if (resultado == true)
                caminho = openfiledialog.FileName;
        }

        private void LimparParticipantes()
        {
            string tecnicos = "(   ) Técnicos";
            string assistentes = "(   ) Assistentes";
            string profissional = "(   ) Profissional da área";
            string coordenadores = "(   ) Coordenadores";
            string diretores = "(   ) Diretores";
            string outro = "(   ) Outro: _____________";


            sql_update.Add("UPDATE [Plan1$B17:B17] SET F1 = '" + tecnicos + "'; ");
            sql_update.Add("UPDATE [Plan1$B18:B18] SET F1 = '" + assistentes + "'; ");
            sql_update.Add("UPDATE [Plan1$D17:D17] SET F1 = '" + profissional + "'; ");
            sql_update.Add("UPDATE [Plan1$D18:D18] SET F1 = '" + coordenadores + "'; ");
            sql_update.Add("UPDATE [Plan1$G17:G17] SET F1 = '" + diretores + "'; ");
            sql_update.Add("UPDATE [Plan1$G18:G18] SET F1 = '" + outro + "'; ");
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            sql_update = new List<string>();

            contador_erros = 0;
            
            try
            {
                avaliacao.Titulo = tituloBox.Text;
                lblTitulo.Foreground = Brushes.Black;
                sql_update.Add("UPDATE [Plan1$A8:A8] SET F1 = '" + avaliacao.Titulo + "'; ");

            }
            catch (Exception)
            {
                lblTitulo.Foreground = Brushes.Red;
                contador_erros++;
            }
            
            try
            {
                avaliacao.Codigo = Int32.Parse(codigoBox.SelectedValue.ToString());
                lblCodigo.Foreground = Brushes.Black;
                string codigo = "Nº: " + avaliacao.Codigo;
                sql_update.Add("UPDATE [Plan1$I8:I8] SET F1 = '" + codigo + "'; ");
            }
            catch (Exception)
            {
                lblCodigo.Foreground = Brushes.Red;
                contador_erros++;
            }
            
            try
            {
                avaliacao.Estagiario = estagiario;
                sql_update.Add("UPDATE [Plan1$A11:A11] SET F1 = '" + avaliacao.Estagiario + "'; ");
            }
            catch (Exception)
            {
                erro();
                Application.Current.Shutdown();
            }
            
            try
            {
                avaliacao.Data = dataBox.Text;
                lblData.Foreground = Brushes.Black;
                sql_update.Add("UPDATE [Plan1$H11:H11] SET F1 = '" + avaliacao.Data + "'; ");
            }
            catch (Exception)
            {
                lblData.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                avaliacao.TotalParticipantes = Int32.Parse(totalParticipantesBox.Text);
                lblTotalParticipantes.Foreground = Brushes.Black;
                sql_update.Add("UPDATE [Plan1$A14:A14] SET F1 = '" + avaliacao.TotalParticipantes + "'; ");
            }
            catch (Exception)
            {
                lblTotalParticipantes.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                avaliacao.TotalRespostas = Int32.Parse(totalRespostasBox.Text);
                lblTotalRespostas.Foreground = Brushes.Black;
                sql_update.Add("UPDATE [Plan1$F14:F14] SET F1 = '" + avaliacao.TotalRespostas + "'; ");
            }
            catch (Exception)
            {
                lblTotalRespostas.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                string participante = null;

                avaliacao.Participantes = participantesBox.Text;

                LimparParticipantes();
                if (avaliacao.Participantes == "Assistentes")
                {
                    participante = "( X ) Assistentes";
                    sql_update.Add("UPDATE [Plan1$B18:B18] SET F1 = '" + participante + "'; ");
                }
                else
                {
                    if (avaliacao.Participantes == "Coordenadores")
                    {
                        participante = "( X ) Coordenadores";
                        sql_update.Add("UPDATE [Plan1$D18:D18] SET F1 = '" + participante + "'; ");
                    }
                    else
                    {
                        if (avaliacao.Participantes == "Diretores")
                        {
                            participante = "( X ) Diretores";
                            sql_update.Add("UPDATE [Plan1$G17:G17] SET F1 = '" + participante + "'; ");
                        }
                        else
                        {
                            if (avaliacao.Participantes == "Profissionais da área")
                            {
                                participante = "( X ) Profissional da área";
                                sql_update.Add("UPDATE [Plan1$D17:D17] SET F1 = '" + participante + "'; ");
                            }
                            else
                            {
                                if (avaliacao.Participantes == "Técnicos")
                                {
                                    participante = "( X ) Técnicos";
                                    sql_update.Add("UPDATE [Plan1$B17:B17] SET F1 = '" +participante+ "'; ");
                                }
                                else
                                {
                                    participante = "( X ) Outro: " + avaliacao.Participantes;
                                    sql_update.Add("UPDATE [Plan1$G18:G18] SET F1 = '" +participante+ "'; ");
                                }                                
                            }
                        }
                    }
                }

                
                lblParticipantes.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lblParticipantes.Foreground = Brushes.Red;
                contador_erros++;
            }

            if ((manhaRadio.IsChecked == false) && (tardeRadio.IsChecked == false))
            {
                lblPeriodo.Foreground = Brushes.Red;
                contador_erros++;
            }
            else
            {
                if (manhaRadio.IsChecked == true)
                {
                    lblPeriodo.Foreground = Brushes.Black;
                    string manha = "( X ) M";
                    string tarde = "(   ) T";
                    sql_update.Add("UPDATE [Plan1$E11:E11] SET F1 = '" + manha + "'; ");
                    sql_update.Add("UPDATE [Plan1$F11:F11] SET F1 = '" + tarde + "'; ");
                }

                if (tardeRadio.IsChecked == true)
                {
                    lblPeriodo.Foreground = Brushes.Black;
                    string manha = "(   ) M";
                    string tarde = "( X ) T";
                    sql_update.Add("UPDATE [Plan1$E11:E11] SET F1 = '" + manha + "'; ");
                    sql_update.Add("UPDATE [Plan1$F11:F11] SET F1 = '" + tarde + "'; ");
                }
            }

            try
            {
                if (string.IsNullOrEmpty(b1100.Text))
                    b1100.Text = "0";
               
                if (string.IsNullOrEmpty(b1125.Text))
                    b1125.Text = "0";
               
                if (string.IsNullOrEmpty(b1150.Text))
                    b1150.Text = "0";
                
                if (string.IsNullOrEmpty(b1175.Text))
                    b1175.Text = "0";

                if (string.IsNullOrEmpty(b1110.Text))
                    b1110.Text = "0";

                if (string.IsNullOrEmpty(b11na.Text))
                    b11na.Text = "0";
                
                avaliacao.Lbl11 = Int32.Parse(b1100.Text) +
                                  Int32.Parse(b1125.Text) + Int32.Parse(b1150.Text) +
                                  Int32.Parse(b1175.Text) + Int32.Parse(b1110.Text) +
                                  Int32.Parse(b11na.Text);

                sql_update.Add("UPDATE [Plan1$D22:D22] SET F1 = '" + b1100.Text + "'; ");
                sql_update.Add("UPDATE [Plan1$E22:E22] SET F1 = '" + b1125.Text + "'; ");
                sql_update.Add("UPDATE [Plan1$F22:F22] SET F1 = '" + b1150.Text + "'; ");
                sql_update.Add("UPDATE [Plan1$G22:G22] SET F1 = '" + b1175.Text + "'; ");
                sql_update.Add("UPDATE [Plan1$H22:H22] SET F1 = '" + b1110.Text + "'; ");
                sql_update.Add("UPDATE [Plan1$I22:I22] SET F1 = '" + b11na.Text + "'; ");

                lbl11.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl11.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b1200.Text))
                    b1200.Text = "0";

                if (string.IsNullOrEmpty(b1225.Text))
                    b1225.Text = "0";

                if (string.IsNullOrEmpty(b1250.Text))
                    b1250.Text = "0";

                if (string.IsNullOrEmpty(b1275.Text))
                    b1275.Text = "0";
                
                if (string.IsNullOrEmpty(b1210.Text))
                    b1210.Text = "0";

                if (string.IsNullOrEmpty(b12na.Text))
                    b12na.Text = "0";

                avaliacao.Lbl12 = Int32.Parse(b1200.Text) + 
                                  Int32.Parse(b1225.Text) + Int32.Parse(b1250.Text) +
                                  Int32.Parse(b1275.Text) + Int32.Parse(b1210.Text) +
                                  Int32.Parse(b12na.Text);

                sql_update.Add("UPDATE [Plan1$D23:D23] SET F1 = " + b1200.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E23:E23] SET F1 = " + b1225.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F23:F23] SET F1 = " + b1250.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G23:G23] SET F1 = " + b1275.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H23:H23] SET F1 = " + b1210.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I23:I23] SET F1 = " + b12na.Text + "; ");

                lbl12.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl12.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b1300.Text))
                    b1300.Text = "0";
               
                if (string.IsNullOrEmpty(b1325.Text))
                    b1325.Text = "0";
               
                if (string.IsNullOrEmpty(b1350.Text))
                    b1350.Text = "0";
              
                if (string.IsNullOrEmpty(b1375.Text))
                    b1375.Text = "0";
               
                if (string.IsNullOrEmpty(b1310.Text))
                    b1310.Text = "0";
              
                if (string.IsNullOrEmpty(b13na.Text))
                    b13na.Text = "0";
               
                avaliacao.Lbl13 = Int32.Parse(b1300.Text) + 
                                  Int32.Parse(b1325.Text) + Int32.Parse(b1350.Text) +
                                  Int32.Parse(b1375.Text) + Int32.Parse(b1310.Text) +
                                  Int32.Parse(b13na.Text);

                sql_update.Add("UPDATE [Plan1$D24:D24] SET F1 = " + b1300.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E24:E24] SET F1 = " + b1325.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F24:F24] SET F1 = " + b1350.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G24:G24] SET F1 = " + b1375.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H24:H24] SET F1 = " + b1310.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I24:I24] SET F1 = " + b13na.Text + "; ");

                lbl13.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl13.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b1400.Text))
                    b1400.Text = "0";

                if (string.IsNullOrEmpty(b1425.Text))
                    b1425.Text = "0";

                if (string.IsNullOrEmpty(b1450.Text))
                    b1450.Text = "0";

                if (string.IsNullOrEmpty(b1475.Text))
                    b1475.Text = "0";

                if (string.IsNullOrEmpty(b1410.Text))
                    b1410.Text = "0";                    

                if (string.IsNullOrEmpty(b14na.Text))
                    b14na.Text = "0";

                avaliacao.Lbl14 = Int32.Parse(b1400.Text) + 
                                  Int32.Parse(b1425.Text) + Int32.Parse(b1450.Text) +
                                  Int32.Parse(b1475.Text) + Int32.Parse(b1410.Text) +
                                  Int32.Parse(b14na.Text);

                sql_update.Add("UPDATE [Plan1$D25:D25] SET F1 = " + b1400.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E25:E25] SET F1 = " + b1425.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F25:F25] SET F1 = " + b1450.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G25:G25] SET F1 = " + b1475.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H25:H25] SET F1 = " + b1410.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I25:I25] SET F1 = " + b14na.Text + "; ");

                lbl14.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl14.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b2100.Text))
                    b2100.Text = "0";

                if (string.IsNullOrEmpty(b2125.Text))
                    b2125.Text = "0";

                if (string.IsNullOrEmpty(b2150.Text))
                    b2150.Text = "0";

                if (string.IsNullOrEmpty(b2175.Text))
                    b2175.Text = "0";

                if (string.IsNullOrEmpty(b2110.Text))
                    b2110.Text = "0";

                if (string.IsNullOrEmpty(b21na.Text))
                    b21na.Text = "0";

                avaliacao.Lbl21 = Int32.Parse(b2100.Text) + 
                                  Int32.Parse(b2125.Text) + Int32.Parse(b2150.Text) +
                                  Int32.Parse(b2175.Text) + Int32.Parse(b2110.Text) +
                                  Int32.Parse(b21na.Text);

                sql_update.Add("UPDATE [Plan1$D30:D30] SET F1 = " + b2100.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E30:E30] SET F1 = " + b2125.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F30:F30] SET F1 = " + b2150.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G30:G30] SET F1 = " + b2175.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H30:H30] SET F1 = " + b2110.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I30:I30] SET F1 = " + b21na.Text + "; ");

                lbl21.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl21.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b2200.Text))
                    b2200.Text = "0";                

                if (string.IsNullOrEmpty(b2225.Text))
                    b2225.Text = "0";
      
                if (string.IsNullOrEmpty(b2250.Text))
                    b2250.Text = "0";

                if (string.IsNullOrEmpty(b2275.Text))
                    b2275.Text = "0";
                
                if (string.IsNullOrEmpty(b2210.Text))
                    b2210.Text = "0";
                
                if (string.IsNullOrEmpty(b22na.Text))
                    b22na.Text = "0";

                avaliacao.Lbl22 = Int32.Parse(b2200.Text) +
                                  Int32.Parse(b2225.Text) + Int32.Parse(b2250.Text) +
                                  Int32.Parse(b2275.Text) + Int32.Parse(b2210.Text) +
                                  Int32.Parse(b22na.Text);
                
                sql_update.Add("UPDATE [Plan1$D31:D31] SET F1 = " + b2200.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E31:E31] SET F1 = " + b2225.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F31:F31] SET F1 = " + b2250.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G31:G31] SET F1 = " + b2275.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H31:H31] SET F1 = " + b2210.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I31:I31] SET F1 = " + b22na.Text + "; ");

                lbl22.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl22.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b2300.Text))
                    b2300.Text = "0";
                
                if (string.IsNullOrEmpty(b2325.Text))
                    b2325.Text = "0";                

                if (string.IsNullOrEmpty(b2350.Text))
                    b2350.Text = "0";
                
                if (string.IsNullOrEmpty(b2375.Text))
                    b2375.Text = "0";
                
                if (string.IsNullOrEmpty(b2310.Text))
                    b2310.Text = "0";
                
                if (string.IsNullOrEmpty(b23na.Text))
                    b23na.Text = "0";
                
                avaliacao.Lbl23 = Int32.Parse(b2300.Text) + 
                                  Int32.Parse(b2325.Text) + Int32.Parse(b2350.Text) +
                                  Int32.Parse(b2375.Text) + Int32.Parse(b2310.Text) +
                                  Int32.Parse(b23na.Text);

                sql_update.Add("UPDATE [Plan1$D32:D32] SET F1 = " + b2300.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E32:E32] SET F1 = " + b2325.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F32:F32] SET F1 = " + b2350.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G32:G32] SET F1 = " + b2375.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H32:H32] SET F1 = " + b2310.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I32:I32] SET F1 = " + b23na.Text + "; ");

                lbl23.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl23.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b31100.Text))
                    b31100.Text = "0";

                if (string.IsNullOrEmpty(b31125.Text))
                    b31125.Text = "0";

                if (string.IsNullOrEmpty(b31150.Text))
                    b31150.Text = "0";
                
                if (string.IsNullOrEmpty(b31175.Text))
                    b31175.Text = "0";
                
                if (string.IsNullOrEmpty(b31110.Text))
                    b31110.Text = "0";

                if (string.IsNullOrEmpty(b311na.Text))
                    b311na.Text = "0";
                
                avaliacao.Lbl311 = Int32.Parse(b31100.Text) +
                                   Int32.Parse(b31125.Text) + Int32.Parse(b31150.Text) +
                                   Int32.Parse(b31175.Text) + Int32.Parse(b31110.Text) +
                                   Int32.Parse(b311na.Text);

                sql_update.Add("UPDATE [Plan1$D37:D37] SET F1 = " + b31100.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E37:E37] SET F1 = " + b31125.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F37:F37] SET F1 = " + b31150.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G37:G37] SET F1 = " + b31175.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H37:H37] SET F1 = " + b31110.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I37:I37] SET F1 = " + b311na.Text + "; ");

                lbl311.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl311.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b31200.Text))
                    b31200.Text = "0";

                if (string.IsNullOrEmpty(b31225.Text))
                    b31225.Text = "0";

                if (string.IsNullOrEmpty(b31250.Text))
                    b31250.Text = "0";

                if (string.IsNullOrEmpty(b31275.Text))
                    b31275.Text = "0";

                if (string.IsNullOrEmpty(b31210.Text))
                    b31210.Text = "0";

                if (string.IsNullOrEmpty(b312na.Text))
                    b312na.Text = "0";

                avaliacao.Lbl312 = Int32.Parse(b31200.Text) + 
                                   Int32.Parse(b31225.Text) + Int32.Parse(b31250.Text) +
                                   Int32.Parse(b31275.Text) + Int32.Parse(b31210.Text) +
                                   Int32.Parse(b312na.Text);

                sql_update.Add("UPDATE [Plan1$D38:D38] SET F1 = " + b31200.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E38:E38] SET F1 = " + b31225.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F38:F38] SET F1 = " + b31250.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G38:G38] SET F1 = " + b31275.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H38:H38] SET F1 = " + b31210.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I38:I38] SET F1 = " + b312na.Text + "; ");

                lbl312.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl312.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b32100.Text))
                    b32100.Text = "0";

                if (string.IsNullOrEmpty(b32125.Text))
                    b32125.Text = "0";

                if (string.IsNullOrEmpty(b32150.Text))
                    b32150.Text = "0";

                if (string.IsNullOrEmpty(b32175.Text))
                    b32175.Text = "0";

                if (string.IsNullOrEmpty(b32110.Text))
                    b32110.Text = "0";

                if (string.IsNullOrEmpty(b321na.Text))
                    b321na.Text = "0";
                    

                avaliacao.Lbl321 = Int32.Parse(b32100.Text) + 
                                   Int32.Parse(b32125.Text) + Int32.Parse(b32150.Text) +
                                   Int32.Parse(b32175.Text) + Int32.Parse(b32110.Text) +
                                   Int32.Parse(b321na.Text);

                sql_update.Add("UPDATE [Plan1$D42:D42] SET F1 = " + b32100.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E42:E42] SET F1 = " + b32125.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F42:F42] SET F1 = " + b32150.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G42:G42] SET F1 = " + b32175.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H42:H42] SET F1 = " + b32110.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I42:I42] SET F1 = " + b321na.Text + "; ");

                lbl321.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl321.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b32200.Text))
                    b32200.Text = "0";

                if (string.IsNullOrEmpty(b32225.Text))
                    b32225.Text = "0";

                if (string.IsNullOrEmpty(b32250.Text))
                    b32250.Text = "0";

                if (string.IsNullOrEmpty(b32275.Text))
                    b32275.Text = "0";

                if (string.IsNullOrEmpty(b32210.Text))
                    b32210.Text = "0";

                if (string.IsNullOrEmpty(b322na.Text))
                    b322na.Text = "0";

                avaliacao.Lbl322 = Int32.Parse(b32200.Text) + 
                                   Int32.Parse(b32225.Text) + Int32.Parse(b32250.Text) +
                                   Int32.Parse(b32275.Text) + Int32.Parse(b32210.Text) +
                                   Int32.Parse(b322na.Text);

                sql_update.Add("UPDATE [Plan1$D43:D43] SET F1 = " + b32200.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E43:E43] SET F1 = " + b32225.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F43:F43] SET F1 = " + b32250.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G43:G43] SET F1 = " + b32275.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H43:H43] SET F1 = " + b32210.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I43:I43] SET F1 = " + b322na.Text + "; ");

                lbl322.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl322.Foreground = Brushes.Red;
                contador_erros++;
            }

            try
            {
                if (string.IsNullOrEmpty(b33100.Text))
                    b33100.Text = "0";

                if (string.IsNullOrEmpty(b33125.Text))
                    b33125.Text = "0";

                if (string.IsNullOrEmpty(b33150.Text))
                    b33150.Text = "0";

                if (string.IsNullOrEmpty(b33175.Text))
                    b33175.Text = "0";

                if (string.IsNullOrEmpty(b33110.Text))
                    b33110.Text = "0";

                if (string.IsNullOrEmpty(b331na.Text))
                    b331na.Text = "0";
               
                avaliacao.Lbl331 = Int32.Parse(b33100.Text) + 
                                   Int32.Parse(b33125.Text) + Int32.Parse(b33150.Text) +
                                   Int32.Parse(b33175.Text) + Int32.Parse(b33110.Text) +
                                   Int32.Parse(b331na.Text);

                sql_update.Add("UPDATE [Plan1$D48:D48] SET F1 = " + b33100.Text + "; ");
                sql_update.Add("UPDATE [Plan1$E48:E48] SET F1 = " + b33125.Text + "; ");
                sql_update.Add("UPDATE [Plan1$F48:F48] SET F1 = " + b33150.Text + "; ");
                sql_update.Add("UPDATE [Plan1$G48:G48] SET F1 = " + b33175.Text + "; ");
                sql_update.Add("UPDATE [Plan1$H48:H48] SET F1 = " + b33110.Text + "; ");
                sql_update.Add("UPDATE [Plan1$I48:I48] SET F1 = " + b331na.Text + "; ");

                lbl331.Foreground = Brushes.Black;
            }
            catch (Exception)
            {
                lbl331.Foreground = Brushes.Red;
                contador_erros++;
            }
            
            try
            {
                avaliacao.Obs = new TextRange(obsBox.Document.ContentStart, obsBox.Document.ContentEnd).Text;
                sql_update.Add("UPDATE [Plan1$A51:A51] SET F1 = '" + avaliacao.Obs + "'; ");
            }
            catch (Exception)
            {
                contador_erros++;
            }

            if (contador_erros != 0)
                return;

            try
            {
                SelecionarArquivo();
                connection = new ExcelConnection(caminho);
                connection.Open();
                foreach (string sql in sql_update)
                {
                    connection.SQL(sql);
                }     
                
            }
            catch (OleDbException)
            {
                erro2();
            }

            connection.Close();

            MessageBox.Show("Avaliação realizada.", "Avaliação", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void erro()
        {
            System.Windows.MessageBox.Show("Erro de acesso ao banco de dados.\n"
                          + "Entre em contato com o desenvolvedor.", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void erro2()
        {
            System.Windows.MessageBox.Show("Falha na inserção de dados.\n"
                          + "Verifique a planilha escolhida.", "Erro",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
