using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
	public class EditModel : PageModel
	{
		public ClientInfo clientInfo = new ClientInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
			String id = Request.Query["id"];

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				// instance of SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// opening connection
					connection.Open();

					// creating sql query
					String sql = "SELECT * FROM clients WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// replace (toBeReplaced, replacingValues)
						command.Parameters.AddWithValue("@id", id);

						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								//  "" + integer value  = conversion to string data type
								clientInfo.id = "" + reader.GetInt32(0);
								clientInfo.name = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.address = reader.GetString(4);
							}
						}
					}
				}
			}
			catch (Exception error)
			{
				errorMessage = error.Message;
			}
		}

		public void OnPost()
		{
			clientInfo.id = Request.Form["id"];
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				// instance of SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// opening connection
					connection.Open();

					// creating sql query. NOTE:- @nameVal is basically parameter [a temporary namespace]
					String sql = "UPDATE clients " +
								 "SET name=@name, email=@email, phone=@phone, address=@address " +
								 "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// replace (toBeReplaced, replacingValues)
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();
					}
				}
			}

			catch (Exception error)
			{
				errorMessage = error.Message;
				return;
			}

			Response.Redirect("/Clients/Index");
		}
	}
}
