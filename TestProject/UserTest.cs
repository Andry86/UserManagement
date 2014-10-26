using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using UserManagement;

namespace TestProject
{
	[TestClass]
	public class UserTest
	{
		[TestInitialize]
		public void Init()
		{
			string stringconn = "Server=ALOCCI;Database=Utenti;integrated security=true";
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringconn);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			SqlCommand Csp = new SqlCommand("delete from Users", sqlConnection);
			Csp.ExecuteReader();
		}

		[TestMethod]
		public void CreazioneUtente()
		{
			string message;
			string name = "gigi";
			string surname = "zonu";
			string country = "cagliari";
			string street = "roma";
			string number = "1";
			string password = "111Acbmm";
			string email = "andrea@tiscali.it";
			string email1 = "slo@liero.it";
			string passwors_n1 = "111Acbmm";
			string email2 = "ale@avanade.com";
			string password_n2 = "111Acbmm";

			List<UserManagement.User> people;

			UserManagement.Address address = new UserManagement.Address(country, street, number);

			UserManagement.User.NewUser(name, surname, password, email, address, out message);

			UserManagement.User user = UserManagement.User.FindByEmail(email, out message);

			//Check fields
			Assert.IsNotNull(user);
			Assert.AreEqual(name, user.Name);
			Assert.AreEqual(surname, user.Surname);


			//Check count
			people = UserManagement.User.FindByName(name, surname, out message);

			Assert.IsTrue(people.Count == 1);

			//Add second user
			User.NewUser(name, surname, passwors_n1, email1, address, out message);

			User utente = UserManagement.User.FindByEmail(email1,out message);

			//Check fields
			Assert.IsNotNull(utente);
			Assert.AreEqual(name, utente.Name);
			Assert.AreEqual(surname, utente.Surname);


			//Check count
			people = User.FindByName(name, surname,out message);

			Assert.IsTrue(people.Count == 2);

			//Add third user
			User.NewUser(name, surname, password_n2, email2, address, out message);
			//creo tre utenti con lo stesso nome e cognome, poi effettuo una ricerca utilizzando nome e cognome, infine controllo il
			//numero di risultati trovati che deve essere uguale a 3

			people = UserManagement.User.FindByName(name, surname, out message);

			//check count list
			Assert.IsTrue(people.Count == 3);
		}

		[TestMethod]
		public void cancellaUtente()
		{
			string message;
			string name = "gigi";
			string surname = "zonu";
			string country = "cagliari";
			string street = "nazionale";
			string number = "1";
			string password = "123";
			string email = "andrea@tiscali.it";
			UserManagement.Address address = new UserManagement.Address(country, street, number);
			
			//Add new user
			UserManagement.User.NewUser(name, surname, password, email, address, out message);

			//delete the user
			UserManagement.User.DeleteUser(email, out message);
			//
			User myuser=	User.FindByEmail(email,out message);
			
			Assert.IsTrue(myuser == null);
		}


		[TestMethod]
		public void ModificaUtente()
		{
			//creo un nuovo utente, effettuo il login, modifico il nome 
			//e verifico che il nome sia effetivamente cambiato
			string message;
			string name = "gigi";
			string surname = "zonu";
			string luogo = "cagliari";
			string street = "nazionale";
			string number = "1";
			string password = "111Abcdf";
			string email = "andrea@tiscali.it";
			System.Console.WriteLine("welcome");

			UserManagement.Address address = new UserManagement.Address(luogo, street, number);

			UserManagement.User.NewUser(name, surname, password, email, address, out message);
			UserManagement.User utente = UserManagement.User.Authenticate(email, password, out message);

			string newName="ale";
			string newSurname="zonu";
			utente.ModifiedData(newName,newSurname , out message);
			UserManagement.User userfind=	UserManagement.User.FindByEmail(email, out message);
			

			System.Console.WriteLine(message);
			//Check fields
			Assert.IsNotNull(userfind);
			Assert.AreEqual(newName, userfind.Name);
			Assert.AreEqual(newSurname, userfind.Surname);

		}

		[TestMethod]
		public void cercaUtente()
		{
			//creo due utenti con lo stesso nome, effettuo una ricerca utilizzando come parametro
			//il nome, verifico che il numero dei risultati della ricerca sia uguale a 2
			//ossia il numero di utenti creati con lo stesso nome
			string message;
			string name = "gigi";
			string surname = "zonu";
			string country = "cagliari";
			string street = "nazionale";
			string number = "1";
			string password = "123Almas";
			string email = "andrea@tiscali.it";
			string email1 = "slo@liero.it";
			string pass1 = "111Abcdf";
			System.Console.WriteLine("welcome");

			UserManagement.Address address = new UserManagement.Address(country, street, number);

			UserManagement.User.NewUser(name, surname, password, email, address, out message);
			UserManagement.User.NewUser(name, surname, pass1, email1, address, out message);
			List<UserManagement.User> listResult = new List<UserManagement.User>();
			listResult = UserManagement.User.FindByName(name, "", out message);
			
			//check result find list count
			Assert.IsTrue(listResult.Count == 2);
			//check the fields
			Assert.AreEqual(listResult[0].EmailAddress, email);
			Assert.AreEqual(listResult[1].EmailAddress, email1);
		}
	}
}