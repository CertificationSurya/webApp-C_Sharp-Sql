using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

// for timer
using System;

namespace MyStore.Pages.Clients
{
	public class Index1Model : PageModel
	{
		public ClientInfo clientInfo = new ClientInfo();

		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			// error check and handle

			if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

			// save new client data into database
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				// instance of SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// opening connection
					connection.Open();

					// creating sql query
					String sql = "INSERT INTO clients " +
								 "(name, email, phone, address) VALUES " +
								 "(@name, @email, @phone, @address);";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// replace (toBeReplaced, replacingValues)
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception error)
			{
				errorMessage = error.Message;
				return;
			}

			clientInfo.name = "";
			clientInfo.phone = "";
			clientInfo.email = "";
			clientInfo.address = "";

			successMessage = "New Client Added Correctly";

			// To Allow Delay - through Js

			// Using JavaScript to redirect after 3s delay
			// string script = "<script>" +
			// 					"setTimeout(function()" +
			// 						" { window.location.href = '/Clients/Index'; " +
			// 					"}, 3000);" +
			// 				"</script>";
			// Response.WriteAsync(script);

			Response.Redirect("/Clients/Index");
		}
	}
}
