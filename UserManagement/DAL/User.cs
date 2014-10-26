using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.DAL
{
	class User
	{
		public const string stringConnSql = "Server=ALOCCI; Database=Utenti; integrated security=true";

		#region Properties
		public int IdUser { get; private set; }
		public string Name { get; private set; }
		public string Surname { get; private set; }
		public string Email { get; private set; }
		public string Password { get; private set; }
		public Address Address { get; private set; }
		#endregion


		public User(int idUser, string name, string surname, string email, string passw, Address address)
		{
			IdUser = idUser;
			Name = name;
			Surname = surname;
			Email = email;
			Password = passw;
			Address = address;
		}

		public static User Create(string name, string surname, string email, string passw, Address address)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("AddUser", sqlConnection);
			commandSql.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.Add("@Nome", SqlDbType.VarChar).Value = name;
			commandSql.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = surname;
			commandSql.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
			commandSql.Parameters.Add("@Passw", SqlDbType.VarChar).Value = passw;
			commandSql.Parameters.Add("@Residenza", SqlDbType.VarChar).Value = address.Residenza;
			commandSql.Parameters.Add("@Via", SqlDbType.VarChar).Value = address.Via;
			commandSql.Parameters.Add("@Numero", SqlDbType.VarChar).Value = address.Num;
			commandSql.Parameters.Add("@out", SqlDbType.Int);
			commandSql.Parameters["@out"].Direction = ParameterDirection.Output;

			try
			{
				commandSql.ExecuteNonQuery();
			}

			catch (System.Data.SqlClient.SqlException i)
			{
				Console.WriteLine(i.ToString());
			}
			catch (System.InvalidOperationException e)
			{
				Console.WriteLine(e.ToString());
			}

			int id = (int)commandSql.Parameters["@out"].Value;
			User person = new User(id, name, surname, email, passw, address);
			return person;
		}

		public static void deleteUser(string email)
		{
			string errMessage = String.Empty;
			//CONNESSIONE AL DB
			SqlConnection sqlConnection1 = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection1.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("deleteUser", sqlConnection1);
			commandSql.CommandType = CommandType.StoredProcedure;

			commandSql.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;

			try
			{
				commandSql.ExecuteNonQuery();
			}

			catch (System.Data.SqlClient.SqlException i)
			{
				errMessage = (i.ToString());
			}
			catch (System.InvalidOperationException e)
			{
				errMessage = (e.ToString());
			}
			//CHIUSURA CONNESSIONE SQL
			sqlConnection1.Close();
		}

		public static List<User> findUser(string name, string Surname)
		{
			List<User> listUser = new List<User>();

			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("findUser", sqlConnection);
			commandSql.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.AddWithValue("@Nome", name);
			commandSql.Parameters.AddWithValue("@Cognome", Surname);
			SqlDataReader reader = commandSql.ExecuteReader();

			int id;
			string firstName;
			string surname;
			string pass;
			string email;
			string street;
			string number;
			string country;

			while (reader.Read())
			{
				id = reader.GetInt32(0);
				firstName = reader.GetString(1);
				surname = reader.GetString(2);
				pass = reader.GetString(3);
				email = reader.GetString(4);
				country = reader.GetString(5);
				street = reader.GetString(6);
				number = reader.GetString(7);

				User persona = new User(id, firstName, surname, pass, email, new Address(street, country, number));
				listUser.Add(persona);
			}
			return listUser;
		}

		public static User FindByEmail(string emailAddress)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("select * from Users where EmailAddress='"+emailAddress+"'", sqlConnection);
			commandSql.CommandType = CommandType.Text;

			//Inserimento Dati nella Tabella
			//commandSql.Parameters.AddWithValue("@EmailAddress", emailAddress);
			//commandSql.Parameters.Add("@Nome", SqlDbType.VarChar).Value = emailAddress;

			SqlDataReader reader = commandSql.ExecuteReader();

			if (!reader.Read()) return null;
			else
			{
				User user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), new Address(reader.GetString(5), reader.GetString(6), reader.GetString(7)));
				return user;
			}
		}

		public static void modifiedDataAccess(string email, string passw, int id)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("modifiedDataAccess", sqlConnection);
			commandSql.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
			commandSql.Parameters.Add("@Password", SqlDbType.VarChar).Value = passw;
			commandSql.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;

			try
			{
				commandSql.ExecuteNonQuery();
			}

			catch (System.Data.SqlClient.SqlException i)
			{
				Console.WriteLine(i.ToString());
			}
			catch (System.InvalidOperationException e)
			{
				Console.WriteLine(e.ToString());
			}

		}



		public static void modifiedData(string name, string surname, int id)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("modifiedData", sqlConnection);
			commandSql.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
			commandSql.Parameters.Add("@Surname", SqlDbType.VarChar).Value = surname;
			commandSql.Parameters.Add("@Id", SqlDbType.Int).Value = id;
			try
			{
				commandSql.ExecuteNonQuery();
			}

			catch (System.Data.SqlClient.SqlException i)
			{
				Console.WriteLine(i.ToString());
			}
			catch (System.InvalidOperationException e)
			{
				Console.WriteLine(e.ToString());
			}

		}


		public static void modifiedAddress(string country, string street, string number, int id)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("modifiedAddress", sqlConnection);
			commandSql.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.Add("@Country", SqlDbType.VarChar).Value = country;
			commandSql.Parameters.Add("@Street", SqlDbType.VarChar).Value = street;
			commandSql.Parameters.Add("@Number", SqlDbType.VarChar).Value = number;
			commandSql.Parameters.Add("@Id", SqlDbType.VarChar).Value = id;

			try
			{
				commandSql.ExecuteNonQuery();
			}

			catch (System.Data.SqlClient.SqlException i)
			{
				Console.WriteLine(i.ToString());
			}
			catch (System.InvalidOperationException e)
			{
				Console.WriteLine(e.ToString());
			}

		}


		//METODO PER EFFETTUARE IL LOGIN, CONTROLLA CHE EMAIL E PASSW SONO PRESENTI NEL DB
		//E RITORNA, SE ESISTE, L'USER CORRISPONDENTE 
		public static User Load(string emailAddress)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand sqlCommand = new SqlCommand("LoadUser", sqlConnection);
			sqlCommand.CommandType = CommandType.StoredProcedure;

			//Inserimento Dati nella Tabella
			sqlCommand.Parameters.AddWithValue("@EmailAddress", emailAddress);

			SqlDataReader reader = sqlCommand.ExecuteReader();

			if (!reader.HasRows)
				return null;

			int id;
			string firstName;
			string surname;
			string password;
			string street;
			string number;
			string country;

			reader.Read();

			id = reader.GetInt32(0);
			firstName = reader.GetString(1);
			surname = reader.GetString(2);
			///email = reader.GetString(3); L'indirizzo email ce l'abbiamo già
			password = reader.GetString(4);
			country = reader.GetString(5);
			street = reader.GetString(6);
			number = reader.GetString(7);

			User client = new User(id, firstName, surname, emailAddress, password, new Address(country, street, number));

			return client;
		}


		//il metodo controlla che l'indirizzo email inserito sia già presente nel DB, se è già utilizzato ritorna false in caso contrario true
		public static bool checkEmail(string email)
		{
			//CONNESSIONE AL DB
			SqlConnection sqlConnection = new SqlConnection(stringConnSql);
			//APERTURA CONNESSIONE
			sqlConnection.Open();
			//Comando Per l'utilizzo del DB con Stored Procedure
			SqlCommand commandSql = new SqlCommand("select * from Users where EmailAddress='" + email + "'", sqlConnection);
			commandSql.CommandType = CommandType.Text;

			//Inserimento Dati nella Tabella
			commandSql.Parameters.AddWithValue("@Email", email);
			SqlDataReader reader = commandSql.ExecuteReader();

			if (!reader.Read()) return true;
			else return false;
		}
	}
}
