using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserManagement
{
	public class User
	{
		#region
		public int ID { get; private set; }
		public string Name { get; private set; }
		public string Surname { get; private set; }
		public string EmailAddress { get; private set; }
		public string Password { get; private set; }
		public Address Address { get; private set; }
		public string hashPassw { get; private set; }
		#endregion

		private User(int id, string name, string surname, string password, string email, Address address)
		{
			this.ID = id;
			this.Name = name;
			this.Surname = surname;
			this.Password = password;
			this.EmailAddress = email;
			Address = address;
		}



		public static void NewUser(string name, string surname, string password, string email, Address address, out string message)
		{
			if (name == String.Empty || surname == String.Empty || password == String.Empty || email == String.Empty)
			{
				message = "Insert all fields";
				MessageBox.Show("I campi name, surname, password, email sono obbligatori.");
				return;
			}
			//Controllo che l'indirizzo email non sia già presente nel DB
			if (DAL.User.checkEmail(email) == false)
			{
				message = "The email address is alreay in use.";
				MessageBox.Show("L'indirizzo email è già presente nel DB. Riprova");
				return;
			}
			//controlla che il formato dell'indirizzo email inserito sia valido
			if (!Utility.IsValidMailAddress(email))
			{
				message = "The email address you entered is not a valid email address";
				MessageBox.Show("The email address you entered is not a valid email address");
				return;
			}
	
			if (!Utility.ValidatePassword(password))
			{
				message = "The password must contain between 8 and 15 characters, at least one number, at least one lower case letter and at least one uppercase letter";
				MessageBox.Show("The password must contain between 8 and 15 characters, at least one number, at least one lower case letter and at least one uppercase letter");
				return;
			}

			//Codifico la Password prima di inserirla nel DB
			password = Utility.codePassword(password);
			DAL.User dalUser;
			DAL.Address addressDAL = new DAL.Address(address.Residenza, address.Via, address.Num);
			dalUser = DAL.User.Create(name, surname, email, password, addressDAL);

			Address Address = new Address(dalUser.Address.Residenza, dalUser.Address.Via, dalUser.Address.Num);
			User user = new User(dalUser.IdUser, dalUser.Name, dalUser.Surname, dalUser.Password, dalUser.Email, Address);
			if (user != null)
			{
				MessageBox.Show("User was created successfully");
				message = "User was created successfully";
			}
			else
			{
				MessageBox.Show("Error. Creation failed");
				message = "Error occured. Creation failed";
			}
		}

		public static void DeleteUser(string email, out string message)
		{
			if (email == String.Empty)
			{
				message = "The field cannot be empty";
				MessageBox.Show("The field cannot be empty");
				return;
			}
			DAL.User.deleteUser(email);
			message = "Changes done successfully";
		}

		public void ModifiedAddress(string country, string street, string num)
		{
			this.Address.Residenza = country;
			this.Address.Via = street;
			this.Address.Num = num;

			DAL.User.modifiedAddress(country, street, num, this.ID);
		}

		public void ModifiedData(string name, string surname, out string message)
		{
			if (name == String.Empty || surname == String.Empty)
			{
				//errore il nome non può essere una stringa vuota
				MessageBox.Show("The 'Name'and 'Surname' fields can not be empty");
				message = "The 'Name'and 'Surname' fields can not be empty";
				return;
			}
			else
			{
				UserManagement.DAL.User.modifiedData(name, surname, this.ID);
				this.Name = name;
				this.Surname = surname;
				message = "Changes done successfully";
				MessageBox.Show("Changes done successfully");
			}
		}


		public void ModifiedDataAccess(string email, string password)
		{
			if (email == String.Empty || password == String.Empty)
			{
				//errore il nome non può essere una stringa vuota
				MessageBox.Show("Devi fornire sia email che password");
				return;
			}
			else
			{
				password = Utility.codePassword(password);
				UserManagement.DAL.User.modifiedDataAccess(email, password, this.ID);
				MessageBox.Show("Changes done successfully");

			}
		}

		//metodo per trovare un utente 
		public static List<User> FindByName(string name, string surname, out string message)
		{
			if (name == String.Empty && surname == String.Empty)
			{
				message = "Insert a field of research";
				MessageBox.Show("Insert a field of research");
			}
			List<User> list = new List<User>();
			List<UserManagement.DAL.User> listDAL = UserManagement.DAL.User.findUser(name, surname);
			if (listDAL.Count == 0)
			{
				message = "No result found for your search";		//"No users found.";
				MessageBox.Show("No users found.");
				return null;
			}
			else
			{
				foreach (UserManagement.DAL.User user in listDAL)
				{
					User utente = new User(user.IdUser, user.Name, user.Surname, user.Password, user.Email, new Address(user.Address.Residenza, user.Address.Via, user.Address.Num));
					list.Add(utente);
				}
			}
			message = "Found "+list.Count+" users";
			return list;
		}


		public static User FindByEmail(string emailAddress, out string message)
		{
			message = String.Empty;
			User user=null;
			if (String.IsNullOrEmpty(emailAddress))
			{
				message = "Insert the filed Email Address";
				MessageBox.Show("Insert the filed Email Address");
				return user;
			}
			UserManagement.DAL.User userDAL = UserManagement.DAL.User.FindByEmail(emailAddress);
			if (userDAL == null)
			{
				message = "User not found";
				MessageBox.Show("User not found");
				return null;
			}
			else
			{
				user = new User(userDAL.IdUser, userDAL.Name, userDAL.Surname, userDAL.Password, userDAL.Email, new Address(userDAL.Address.Residenza, userDAL.Address.Via, userDAL.Address.Num));
				return user;
			}
		}

		//metodo per effettuare il login, prende in ingresso email e password e controlla ci sia una corrispondenza nel DB
		//ritorna l'utente corrispondente
		public static User Authenticate(string emailAddress, string password, out string returnMessage)
		{
			password = Utility.codePassword(password);
			DAL.User dalUser;
			dalUser = DAL.User.Load(emailAddress);

			if (dalUser == null)
			{
				returnMessage = "User not found.";
				MessageBox.Show("User not found.");
				return null;
			}

			if (password != dalUser.Password)
			{
				returnMessage = "The password you entered is incorrect.Please try again";
				MessageBox.Show("The password you entered is incorrect.Please try again");
				return null;
			}

			Address address = new Address(dalUser.Address.Residenza, dalUser.Address.Via, dalUser.Address.Num);
			User user = new User(dalUser.IdUser, dalUser.Name, dalUser.Surname, dalUser.Password, dalUser.Email, address);

			returnMessage = string.Empty;
			return user;
		}
	}
}