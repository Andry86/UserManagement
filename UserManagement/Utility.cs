using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserManagement
{
	class Utility
	{
		//il metodo prende in ingresso una stringa e restituisce una stringa che rappresenta l'hashcode della stringa in input
		public static string codePassword(string input)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5CryptoServiceProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
			//Converte la stringa di input in un array di byte e calcola hash
			byte[] passwordHash;
			passwordHash = md5CryptoServiceProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

			//Crea un nuovo StringBuilder per raccogliere i byte e crea una stringa.
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (byte b in passwordHash)
			{
				stringBuilder.Append(b.ToString("x2").ToLower());
			}
			return stringBuilder.ToString();
		}

		//funzione che controlla che la stringa email sia in un formato indirizzo email valido 
		public static bool IsValidMailAddress(string emailAddress)
		{
			string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
						@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$";
			return (Regex.IsMatch(emailAddress, pattern));
		}

		//un'altra funzione per verificare se una stringa è in un formato indirizzo email
		bool IsValidEmail(string email)
		{
			//se viene sollevata l'eccezione la stringa non è un indirizzo email valido
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}
		//il metodo controlla che la password scelta abbia una lunghezza compresa tra 8 e 15, che
		//contenga almeno un numero, almeno una lettera maiuscola e almeno una lettera minuscola 
	public	static bool ValidatePassword(string password)
		{
			const int MIN_LENGTH = 8;
			const int MAX_LENGTH = 15;

			if (password == null) throw new ArgumentNullException();

			bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
			bool hasUpperCaseLetter = false;
			bool hasLowerCaseLetter = false;
			bool hasDecimalDigit = false;

			if (meetsLengthRequirements)
			{
				foreach (char c in password)
				{
					if (char.IsUpper(c)) hasUpperCaseLetter = true;
					else if (char.IsLower(c)) hasLowerCaseLetter = true;
					else if (char.IsDigit(c)) hasDecimalDigit = true;
				}
			}

			bool isValid = meetsLengthRequirements
							&& hasUpperCaseLetter
							&& hasLowerCaseLetter
							&& hasDecimalDigit
							;
			return isValid;

		}

	}
}
