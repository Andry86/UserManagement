using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{


			string em = "andr666eatiscali.it";
			string code = Utility.codePassword(em);

			string ee = "andr666eatiscali.it";
			ee = Utility.codePassword(ee);
			System.Console.WriteLine(code + "\t" + ee);

			// string message;
			// string nome = "anlberto";
			// string cognome = "cani";
			// string luogo = "milano";
			// string pass = "zizi";
			// string email = "alberto@tiscali.it";
			//UserManagement.Address address = new UserManagement.Address("aqq", "qq", "za");

			// UserManagement.User.newUser(nome, cognome,pass , email, address, out message);

			// email = "slof@libero.com";
			// UserManagement.User.newUser(nome, cognome, pass, email, address, out message);

			//   UserManagement.User.deleteUser(pass, email);
			//List<UserManagement.User> list;
			//list=UserManagement.User.findUser("anlberto", "");

			// foreach(UserManagement.User u in list)
			//     System.Console.WriteLine(u.ID+"\t"+u.Name+"\t"+u.Surname+"\n");
			// System.Console.ReadLine();
			// list[0].modifiedDate("riki", "cani");
			// System.Console.WriteLine(list[0].Name + list[0].Surname);
			// System.Console.ReadLine();
			string message;
			string nome = "gigi";
			string cognome = "zonu";
			string luogo = "cagliari";
			string street = "nazionale";
			string number = "1";
			string password = "123";
			string email = "andrea@tiscali.it";
			UserManagement.Address address = new UserManagement.Address(luogo, street, number);

			//   UserManagement.User utente = UserManagement.User.Authenticate(email, pass, out message);
			UserManagement.User.NewUser(nome, cognome, password, email, address, out message);
			System.Console.WriteLine(message);

			UserManagement.User.FindByEmail(email,out message);
			//  string temp = utente.Name;
			//UserManagement.User.deleteUser(email,out message);
			//UserManagement.User.newUser(nome, cognome, pass, email, address, out message);


			//utente.modifiedData("ale", "zonu", out message);

			//System.Console.WriteLine(message+temp+"\t\t"+utente.Name);
			System.Console.ReadLine();
		}
	}
}
