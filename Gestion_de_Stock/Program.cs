using System;
using System.Text.Json;
using Saving;

namespace Stockage
{
    public class Produit
    {
        public string CodeProduit { get; set; }
        public string Nom { get; set; }
        public double PrixUnitaire { get; set; }
        public int QuantiteEnStock { get; set; }

        public Produit(string codeproduit, string nom, double prixunitaire, int quantiteenstock)
        {


            CodeProduit = codeproduit;
            Nom = nom;
            QuantiteEnStock = quantiteenstock;
            PrixUnitaire = prixunitaire;


        }
    }
    public class Stock
    {
        public List<List<Produit>> Contain = new List<List<Produit>>();
        public void Ajouter_Produit()
        {
            List<Produit> Stockage = new List<Produit>();
            try
            {
                Console.WriteLine("rentrer le code du produit");
                string codeproduitdouble = Console.ReadLine();
                Console.WriteLine("rentrer le nom du produit");
                string nom = Console.ReadLine();
                Console.WriteLine("rentrer le prix du produit");
                double prixunitaireint = double.Parse(Console.ReadLine());
                Console.WriteLine("rentrer la quantité restante du produit");
                int quantiteenstock = int.Parse(Console.ReadLine());

                Produit produit = new Produit(codeproduitdouble, nom, prixunitaireint, quantiteenstock);
                Stockage.Add(produit);
                Contain.Add(Stockage);
                Console.WriteLine("Produit ajouter avec succès");
            }
            catch (FormatException)
            {
                Console.WriteLine("l'un des elements n'est pas au bon format");

            }

        }

        public void Maj_Details()
        {
            Console.WriteLine("Quel Produit voulez vous modifier ? (numero de la ligne)");
            Console.WriteLine("Annuler : -1");
            try
            {
                int choix = int.Parse(Console.ReadLine());
                if (choix == -1)
                {
                    return;
                }
                else if (choix > Contain.Count)
                {
                    Console.WriteLine("le produit n'existe pas dans la liste");
                }
                else
                {
                    Console.WriteLine("Rentrer le code du produit");
                    string codeProduit = Console.ReadLine();
                    Console.WriteLine("Rentrer le nom du produit");
                    string nom = Console.ReadLine();
                    Console.WriteLine("Rentrer le prix du produit");
                    double prixUnitaire = double.Parse(Console.ReadLine());
                    Console.WriteLine("Rentrer la quantité restante du produit");
                    int quantiteEnStock = int.Parse(Console.ReadLine());

                    Produit produit = new Produit(codeProduit, nom, prixUnitaire, quantiteEnStock);
                    Contain[choix - 1] = new List<Produit> { produit };
                    Console.WriteLine("Produit modifié avec succès");
                }
            }
            catch
            {
                Console.WriteLine("le format n'est pas bon");
            }
        }


        public void supprimer_produit()
        {
            try
            {
                Console.WriteLine("quel produit voulez vous supprimer ? (donner la ligne)");
                Console.WriteLine("-1 : retour");
                int choix = int.Parse(Console.ReadLine());

                if (choix == -1)
                {
                    return;
                }
                else if (choix <= 0 || choix > Contain.Count)
                {
                    Console.WriteLine("le produit n'existe pas");
                }
                else
                {
                    Contain.RemoveAt(choix - 1);
                    Console.WriteLine("le produit a été supprimé avec succès");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("le format n'est pas le bon");
            }
        }

        public void Afficher_Produits()
        {
            if (Contain.Count == 0)
            {
                Console.WriteLine("il n'y a aucun produit enregistrer");
                Console.WriteLine();
                return;
            }
            Console.WriteLine("Liste des produits : ");
            for (int i = 0; i < Contain.Count; i++)
            {
                Console.WriteLine($"ligne{i + 1}");
                foreach (Produit produit in Contain[i])
                {
                    Console.Write($"Code Produit: {produit.CodeProduit}/ ");
                    Console.Write($"Nom: {produit.Nom}/ ");
                    Console.Write($"Prix Unitaire: {produit.PrixUnitaire}/ ");
                    Console.Write($"Quantité en Stock: {produit.QuantiteEnStock}/ ");
                    Console.WriteLine();
                }

            }
        }
        public void SauvegarderDonnees(string nomFichier)
        {
            try
            {
                if (!nomFichier.EndsWith(".txt"))
                {
                    nomFichier += ".txt";
                }
                using (StreamWriter writer = new StreamWriter(nomFichier))
                {
                    foreach (var produits in Contain)
                    {
                        foreach (var produit in produits)
                        {
                            writer.Write($"Code Produit: {produit.CodeProduit}/ ");
                            writer.Write($"Nom: {produit.Nom}/ ");
                            writer.Write($"Prix Unitaire: {produit.PrixUnitaire}/ ");
                            writer.Write($"Quantité en Stock: {produit.QuantiteEnStock}/ ");
                            writer.WriteLine();
                        }
                    }
                }

                Console.WriteLine("Données sauvegardées avec succès dans le repertoire de votre script.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            Stock stock = new Stock();
            DataBase data = new DataBase();

            data.LoadData(stock);

            while (true)
            {
                data.SaveData(stock);


                Console.WriteLine("bienvenu dans votre gestionnaire de produit :");
                Console.WriteLine("1-Ajouter un produit au stoc");
                Console.WriteLine("2-Mettre a jour les details d'un produits");
                Console.WriteLine("3-Supprimer un produit");
                Console.WriteLine("4-Afficher le detail des produits");
                Console.WriteLine("5-Sauvegarder les données");


                string choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        stock.Ajouter_Produit();
                        break;
                    case "2":
                        stock.Maj_Details();
                        break;
                    case "3":
                        stock.supprimer_produit();
                        break;
                    case "4":
                        stock.Afficher_Produits();
                        break;
                    case "5":
                        Console.WriteLine("Entrer le nom de votre fichier de sauvegarde");
                        string nomFichier = Console.ReadLine();
                        stock.SauvegarderDonnees(nomFichier);
                        break;
                    default:
                        Console.WriteLine("erreur : veuiller rentrer 1 2 3 4 ou 5 seulement");
                        break;
                }

            }
        }
    }
    public class DataBase
    {

        string FilePath = "data.json";

        public void SaveData(Stock stock)
        {
            string DataJson = JsonSerializer.Serialize(stock.Contain);
            File.WriteAllText(FilePath, DataJson);
        }

        public void LoadData(Stock stock)
        {
            try
            {
                string DataJson = File.ReadAllText(FilePath);
                stock.Contain = JsonSerializer.Deserialize<List<List<Produit>>>(DataJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
                Console.WriteLine();
            }
        }
    }
}



