using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackJack
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PaquetDeCarte paquet;
        Carte carte, carteCroupier;
        int score, scoreCroupier, argent, argentDepose, argentMiser, benefice;
        bool lost, validation, miseValide, win, start = true, miseFocus;

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                theGrid.Focus();
                ValideMise();
                miseFocus = false;
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("2-10 : valeur faciale\n" +
                "valet damme roi : 10\n" +
                "As : 11");
            theGrid.Focus();
        }

        private void TheGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            theGrid.Focus();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(!miseFocus)
            {
                if (e.Key == Key.Return)
                {
                    if (validation)
                    {
                        ClickCroupierCard();
                    }
                    else if (miseValide)
                    {
                        ClickOwnCard();
                    }
                    else
                    {
                        Start();
                    }
                }
            }
        }

        private void Mise_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!start)
            {
                Mise.Text = "";
                
            }
            else if (miseValide)
            {
                Mise.Text = argentMiser.ToString();
            }
        }

        private void Mise_GotFocus(object sender, RoutedEventArgs e)
        {
            miseFocus = true;
        }

        private void Mise_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(argent < 100)
            {
                argentDepose += 100;
                argent = 100;
                MessageBox.Show("Opération réussie");
                Argent.Text = argent.ToString();
            }
            else
            {
                MessageBox.Show("Vous pouvez recaver uniquement quand vous possedez moins de 100$");
            }
            theGrid.Focus();

        }

        private void RecommencerButton_Click(object sender, RoutedEventArgs e)
        {
            Start();
            theGrid.Focus();
        }

        private void ValiderMise_Click(object sender, RoutedEventArgs e)
        {
            ValideMise();
        }

        private void ValiderScore_Click(object sender, RoutedEventArgs e)
        {
            if (miseValide && !lost && !win)
            {
                validation = true;
                WinBlock.Text = "Au tour du croupier";
            }
            theGrid.Focus();

        }

        public MainWindow()
        {
            InitializeComponent();
            lost = validation = miseValide = win = miseFocus = false;
            start = true;
            argent = 100;
            argentDepose = 100;
            Argent.Text = argent.ToString();
            WinBlock.Text = "Entrez une mise";
            paquet = new PaquetDeCarte();
            benefice = 0;
            

        }

        private void Carte1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickOwnCard();
        }

        private void Croupier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickCroupierCard();
        }

        private void ValideMise()
        {
            int oldArgentMise = argentMiser;
            if (!start)
            {
                MessageBox.Show("Appuyer sur start pour demarrer une partie");
            }
            else
            {
                if (!int.TryParse(Mise.Text, out argentMiser) || argentMiser <= 0)
                {
                    MessageBox.Show("Montant invalide");
                }
                else
                {
                    if (miseValide)
                    {
                        MessageBox.Show("Mise déja enregistrée");
                        argentMiser = oldArgentMise;
                        Mise.Text = argentMiser.ToString();
                    }
                    else
                    {
                        if (argent >= argentMiser)
                        {
                            if (!validation && !miseValide)
                            {
                                miseValide = true;
                                WinBlock.Text = "A vous de jouer";
                                theGrid.Focus();
                            }
                            else
                            {
                                MessageBox.Show("appuyer sur Start pour lancer une nouvelle partie");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Vous n'avez pas assez d'argent");
                        }
                    }
                }
            }
        }

        private void Start()
        {
            if(start == false)
            {
                if (argent > 0)
                {
                    start = true;
                    paquet = new PaquetDeCarte();
                    score = scoreCroupier = 0;
                    lost = validation = false;
                    Carte1.Source = new BitmapImage(new Uri("verso2.jpg", UriKind.Relative));
                    Croupier.Source = new BitmapImage(new Uri("verso2.jpg", UriKind.Relative));
                    ScoreBlock.Text = ScoreCroupier.Text = "0";
                    Argent.Text = argent.ToString();
                    miseValide = win = false;
                    miseFocus = false;
                    WinBlock.Text = "Entrez une mise";
                }
                else
                {
                    MessageBox.Show("Vous n'avez pas d'argent");
                }
            }
            theGrid.Focus();
        }

        private void ClickOwnCard()
        {
            if (!lost && !validation && miseValide && !win && start)
            {
                carte = paquet.getRandomCarte();
                Carte1.Source = new BitmapImage(new Uri(carte.path, UriKind.Relative));
                if (carte.valeur < 5) score += 2;
                else if (carte.valeur < 9) score += 3;
                else if (carte.valeur < 13) score += 4;
                else if (carte.valeur < 17) score += 5;
                else if (carte.valeur < 21) score += 6;
                else if (carte.valeur < 25) score += 7;
                else if (carte.valeur < 29) score += 8;
                else if (carte.valeur < 33) score += 9;
                else if (carte.valeur < 49) score += 10;
                else score += 11;

                if (score > 21)
                {
                    lost = true;
                    WinBlock.Text = "Perdu !\n Appuyer sur Start pour lancer une nouvelle partie";
                    argent -= argentMiser;
                    benefice -= argentMiser;
                    start = miseValide = false;
                }
                else if (score == 21)
                {
                    win = true;
                    WinBlock.Text = "BlackJack !\n Appuyer sur Start pour lancer une nouvelle partie";
                    argent += (argentMiser * 2);
                    benefice += (argentMiser * 2);
                    start = miseValide = false;
                }


            }
            ScoreBlock.Text = score.ToString();
            Argent.Text = argent.ToString();
            BeneficeBox.Text = benefice.ToString();
            theGrid.Focus();
        }

        private void ClickCroupierCard()
        {
            if (!lost && validation && !win && start)
            {
                carteCroupier = paquet.getRandomCarte();
                Croupier.Source = new BitmapImage(new Uri(carteCroupier.path, UriKind.Relative));
                if (carteCroupier.valeur < 5) scoreCroupier += 2;
                else if (carteCroupier.valeur < 9) scoreCroupier += 3;
                else if (carteCroupier.valeur < 13) scoreCroupier += 4;
                else if (carteCroupier.valeur < 17) scoreCroupier += 5;
                else if (carteCroupier.valeur < 21) scoreCroupier += 6;
                else if (carteCroupier.valeur < 25) scoreCroupier += 7;
                else if (carteCroupier.valeur < 29) scoreCroupier += 8;
                else if (carteCroupier.valeur < 33) scoreCroupier += 9;
                else if (carteCroupier.valeur < 49) scoreCroupier += 10;
                else scoreCroupier += 11;

                if (score == scoreCroupier)
                {
                    WinBlock.Text = "Egalité\n Appuyer sur Start pour lancer une nouvelle partie";
                    start = miseValide = false;
                    win = true;
                }
                else if (scoreCroupier > 21)
                {
                    WinBlock.Text = "Vous avez gagné !\n Appuyer sur Start pour lancer une nouvelle partie";
                    argent += argentMiser;
                    benefice += argentMiser;
                    start = miseValide = false;
                    win = true;
                }
                else if (scoreCroupier > score)
                {
                    lost = true;
                    argent -= argentMiser;
                    benefice -= argentMiser;
                    WinBlock.Text = "Perdu !\n Appuyer sur Start pour lancer une nouvelle partie";
                    start = miseValide = false;
                }
                Argent.Text = argent.ToString();
                BeneficeBox.Text = benefice.ToString();
                ScoreCroupier.Text = scoreCroupier.ToString();
                theGrid.Focus();
            }
            theGrid.Focus();
        }
    }
}
